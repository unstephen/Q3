using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RequestItem : UGuiComponentClone
{
    private string applyId;

    public Text id;
    public Text name;
    public Image headIcon;
    public Text result;

    Button btnrefuse;
    Button btnaccept;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        id = link.Get<Text>("id");
        name = link.Get<Text>("name");
        headIcon = link.Get<Image>("head");
        result = link.Get<Text>("result");
        result.enabled = false;

        btnrefuse = link.Get<Button>("refuse");
        btnaccept = link.Get<Button>("accept");
        link.SetEvent("refuse", UIEventType.Click, _ => OnHandleRequest(false));
        link.SetEvent("accept", UIEventType.Click, _ => OnHandleRequest(true));
    }

    public void OnHandleRequest(bool accept)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role == null)
        {
            Debug.Log("role is null in requestitem handle");
            return;
        }
        Recv_Post_HandleRequest handle = NetWorkManager.Instance.CreateGetMsg<Recv_Post_HandleRequest>(GameConst._handleRequest,
GameManager.Instance.GetSendInfoStringList<Send_AgreenClub>(role.id.Value, role.token.Value, applyId, accept ? "accept" : "refuse"));
        if (handle != null && handle.code == 0)
        {
            result.text = accept ? "已接受" : "已拒绝";
            result.enabled = true;

            btnrefuse.enabled = false;
            btnaccept.enabled = false;
        }
    }
}
