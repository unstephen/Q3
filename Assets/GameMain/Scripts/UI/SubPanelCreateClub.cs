using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubPanelCreateClub : UGuiComponent
{
    InputField name;
    InputField info;
    Dropdown Search;

    string nameStr;
    string infoStr;
    bool canBeReach;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();
        name = link.Get<InputField>("nameInputField");
        name.onValueChanged.AddListener(NameChange);
        info = link.Get<InputField>("infoInputField");
        info.onValueChanged.AddListener(InfoChange);
        Search = link.Get<Dropdown>("Dropdown");
        Search.onValueChanged.AddListener(BeReachChange);

        nameStr = "";
        infoStr = "";
        canBeReach = true;

        link.SetEvent("create", UIEventType.Click, SendCreateInfo);
    }

    private void NameChange(string value)
    {
        nameStr = value;
    }

    private void InfoChange(string value)
    {
        infoStr = value;
    }

    private void BeReachChange(int value)
    {
        canBeReach = value == 0;
    }

    private void SendCreateInfo(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role == null)
        {
            return;
        }

        Recv_Post_CreatClub createClub = NetWorkManager.Instance.CreateGetMsg<Recv_Post_CreatClub>(GameConst._createClub,
    GameManager.Instance.GetSendInfoStringList<Send_Post_CreateClub>(role.id.Value, role.token.Value, nameStr, infoStr, canBeReach ? "true" : "false"));

        if (createClub != null && createClub.code == 0)
        {
            GameEntry.UI.OpenDialog(new DialogParams
            {
                Mode = 1,
                Title = "创建俱乐部 ",
                Message = "创建成功",
                ConfirmText = "确定",
                OnClickConfirm = delegate (object userData) { RefreshCurClubId(int.Parse(createClub.data.club_id)); }
            });
        }
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

    }

    private void RefreshCurClubId(int cludId)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role != null)
        {
            role.curClubId.SetValueAndForceNotify(cludId);
        }
    }
}
