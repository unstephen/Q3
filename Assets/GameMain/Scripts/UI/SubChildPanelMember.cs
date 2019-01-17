using GamePlay;
using System.Collections.Generic;

public class SubChildPanelMember : UGuiComponent
{
    ClubMemberItem memberItem;
    List<ClubMemberItem> memberItemList;

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

        InitLocal((List<ManagerData>)userData);
    }

    public void InitLocal(List<ManagerData> list)
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

                memberItemList[curIndex].SetItemInfo(item, curIndex);
            }
            else
            {
                ClubMemberItem tempItem = memberItem.Clone() as ClubMemberItem;
                if (tempItem)
                {
                    tempItem.SetActive(true);
                    tempItem.OpenUI();
                    tempItem.SetItemInfo(item, curIndex);

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