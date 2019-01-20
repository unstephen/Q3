using GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class ClubSearchItem : UGuiComponentClone
{
    Text idstr;
    Text nameStr;
    Text ownerStr;
    Text result;
    Button handle;
    public int index;

    string curClubId;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        idstr = link.Get<Text>("id");
        nameStr = link.Get<Text>("name");
        ownerStr = link.Get<Text>("owner");
        result = link.Get<Text>("Result");
        handle = link.Get<Button>("btnJoin");

        link.SetEvent("btnJoin", UIEventType.Click, OnClickJoin);

        //link.SetEvent("goodButton", UIEventType.Click, _ => OnClick());
    }

    public void SetItemInfo(SearchClubBaseData data, int index)
    {
        this.index = index;

        idstr.text = data.club_id;
        nameStr.text = data.club_name;
        ownerStr.text = data.create_user_name;

        RoleData role = GameManager.Instance.GetRoleData();
        bool bShow = role.CheckRequestClub(data.club_id);
        handle.gameObject.SetActive(!bShow);
        result.gameObject.SetActive(bShow);

        curClubId = data.club_id;
    }

    public void OnClickJoin(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        Http_MsgBase myClub = NetWorkManager.Instance.CreateGetMsg<Http_MsgBase>(GameConst._applyClub,
GameManager.Instance.GetSendInfoStringList<Send_RequestClub>(role.id.Value, role.token.Value, curClubId));

        handle.gameObject.SetActive(false);
        result.gameObject.SetActive(true);
    }
}