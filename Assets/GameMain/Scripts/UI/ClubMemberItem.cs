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

    int index;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        head = link.Get<Image>("Head");
        textId = link.Get<Text>("TextId");
        textName = link.Get<Text>("TextName");

        index = 0;
    }

    public void SetItemInfo(ManagerData data, int index)
    {
        this.index = index;

        textId.text = data.user_id;
        textName.text = data.nick_name;
    }
}
