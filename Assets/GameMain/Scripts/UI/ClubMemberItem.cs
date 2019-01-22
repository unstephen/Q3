using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubMemberItem : UGuiComponentClone
{
    Image head;
    Text textId;
    Text textName;
    Text textManager;
    Text btnSetManager;

    int index;
    bool isManager;
    string clubId;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        head = link.Get<Image>("Head");
        textId = link.Get<Text>("TextId");
        textName = link.Get<Text>("TextName");
        textManager = link.Get<Text>("TextManager");

        link.SetEvent("ButtonHandle", UIEventType.Click, OnDeleteMember);
        btnSetManager = link.Get<Text>("TextSetManager");
        link.SetEvent("ButtonManager", UIEventType.Click, OnSetManager);

        index = 0;
    }

    private void OnDeleteMember(params object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        Http_MsgBase mainPage = NetWorkManager.Instance.CreateGetMsg<Http_MsgBase>(GameConst._mainPage,
            GameManager.Instance.GetSendInfoStringList <Send_HandleManager>(role.id.Value, role.token.Value, clubId, textId.text));

        if (mainPage != null && mainPage.code == 0)
        {

        }
    }

    private void OnSetManager(params object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        Http_MsgBase mainPage = NetWorkManager.Instance.CreateGetMsg<Http_MsgBase>(GameConst._mainPage,
            GameManager.Instance.GetSendInfoStringList<Send_DeleteMember>(role.id.Value, role.token.Value, clubId, textId.text, isManager ? "del" : "add"));

        if (mainPage != null && mainPage.code == 0)
        {
            isManager = !isManager;

            textManager.text = isManager ? "管理员" : "";
            btnSetManager.text = isManager ? "删除管理员" : "设为管理员";
        }
    }

    public void SetItemInfo(ManagerData data, int index, bool showManager = false)
    {
        this.index = index;

        textId.text = data.user_id;
        textName.text = data.nick_name;

        isManager = showManager;
        textManager.text = showManager ? "管理员" : "";
        btnSetManager.text = showManager ? "删除管理员" : "设为管理员";
    }
}
