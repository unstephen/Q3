using GamePlay;
using System.Collections.Generic;

public class SubChildPanelMember : UGuiComponent
{
    ClubMemberItem memberItem;
    List<ClubMemberItem> memberItemList;

    string clubId;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        if (userData == null)
        {
            return;
        }

        GUILink link = GetComponent<GUILink>();
        memberItem = link.GetComponent<ClubMemberItem>("Managers");
        memberItem.SetActive(false);
        if (memberItemList == null)
        {
            memberItemList = new List<ClubMemberItem>();
        }

        clubId = (string)userData;
        //InitLocal((List<ManagerData>)userData);
    }

    public void ShowMembers(int page)
    {
        RoleData role = GameManager.Instance.GetRoleData();

        List<ClubMemberData> members = role.GetClubMemberSById(clubId, page);
        if (memberItem == null)
        {
            Recv_Post_AllClubMember allMember = NetWorkManager.Instance.CreateGetMsg<Recv_Post_AllClubMember>(GameConst._mainPage,
                GameManager.Instance.GetSendInfoStringList<Send_GetAllMember>(role.id.Value, role.token.Value, clubId, page, GameConst.pageSize));
            if (allMember != null && allMember.code == 0)
            {
                role.InitClubMemberList(clubId, allMember);

                InitLocal(allMember.data.list);
            }
        }
        else
        {
            InitLocal(members);
        }
    }

    public void InitLocal(List<ClubMemberData> list)
    {
        if (list == null || list.Count == 0)
        {
            return;
        }

        int curIndex = 0;
        foreach (var item in list)
        {
            if (curIndex < memberItemList.Count)
            {
                if (!memberItemList[curIndex].isActiveAndEnabled)
                {
                    memberItemList[curIndex].SetActive(true);
                }

                memberItemList[curIndex].SetItemInfo(item, curIndex, clubId);
            }
            else
            {
                ClubMemberItem tempItem = memberItem.Clone() as ClubMemberItem;
                if (tempItem)
                {
                    tempItem.SetActive(true);
                    tempItem.OpenUI();
                    tempItem.SetItemInfo(item, curIndex, clubId);

                    memberItemList.Add(tempItem);
                }
            }

            curIndex++;
        }

        if (curIndex < memberItemList.Count)
        {
            for (int i = curIndex; i < memberItemList.Count; ++i)
            {
                memberItemList[i].SetActive(false);
            }
        }
    }
}