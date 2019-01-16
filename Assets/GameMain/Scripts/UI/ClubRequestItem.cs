using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubRequestItem : UGuiComponentClone
{
    Image head;
    Text id;
    Text name;
    Text result;

    int index;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        id = link.Get<Text>("id");
        name = link.Get<Text>("name");
        result = link.Get<Text>("result");

        link.SetEvent("accept", UIEventType.Click, _ => SetRequestClick(true));
        link.SetEvent("refuse", UIEventType.Click, _ => SetRequestClick(false));

        index = 0;
    }

    private void SetRequestClick(bool accept)
    {

    }

    public void SetItemInfo(ApplicantsData data, int index)
    {
        this.index = index;

        id.text = data.apply_id;
        name.text = data.nick_name;
        //result.text = data.club_name;
    }
}
