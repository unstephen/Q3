using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryItem : UGuiComponentClone
{
    Text id;
    Text room_name;
    Text club_name;
    Text score;
    Text type;

    int index;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        id = link.Get<Text>("Textid");
        room_name = link.Get<Text>("Textname");
        club_name = link.Get<Text>("Textclub");
        score = link.Get<Text>("Textscore");
        type = link.Get<Text>("Texttype");

        index = 0;
    }

    public void SetItemInfo(HistorySingleBaseData data, int index)
    {
        this.index = index;

        id.text = data.room_id;
        room_name.text = data.room_name;
        club_name.text = data.club_name;
        score.text = data.base_score.ToString(); ;
        type.text = data.room_type;
    }
}
