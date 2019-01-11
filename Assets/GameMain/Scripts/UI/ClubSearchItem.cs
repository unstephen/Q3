using GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class ClubSearchItem : UGuiComponentClone
{
    Text idstr;
    Text nameStr;
    Text ownerStr;
    public int index;

    int curClubId;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        idstr = link.Get<Text>("id");
        nameStr = link.Get<Text>("name");
        ownerStr = link.Get<Text>("owner");

        link.SetEvent("btnJoin", UIEventType.Click, OnClickJoin);

        //link.SetEvent("goodButton", UIEventType.Click, _ => OnClick());
    }

    public void SetItemInfo(SearchClubBaseData data, int index)
    {
        this.index = index;

        idstr.text = data.club_id;
        nameStr.text = data.club_name;
        ownerStr.text = data.create_user_name;

        curClubId = int.Parse(data.club_id);
    }

    public void OnClickJoin(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        Http_MsgBase myClub = NetWorkManager.Instance.CreateGetMsg<Http_MsgBase>(GameConst._applyClub,
GameManager.Instance.GetSendInfoStringList<Send_RequestClub>(role.id.Value, role.token.Value, curClubId));
    }
}