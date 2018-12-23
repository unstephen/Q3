using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubItemI : UGuiComponentClone
{
    Text id;
    Text name;
    Text owner;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        id = link.Get<Text>("id");
        name = link.Get<Text>("name");
        owner = link.Get<Text>("owner");

        link.SetEvent("goodButton", UIEventType.Click, _ => OnClick());
    }

    public void SetClubInfo()
    {

    }

    public void OnClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.DialogForm, new DialogParams
        {
            Mode = 2,
            Title = "购买商品",
            Message = "是否花费",
            ConfirmText = "加入",
            //OnClickConfirm = delegate (object userData) { DoMessegeBox(true); },
            CancelText = "取消",
            //OnClickCancel = delegate (object userData) { DoMessegeBox(false); }
        });
    }
}
