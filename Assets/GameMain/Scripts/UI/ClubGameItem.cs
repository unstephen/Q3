using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClubGameItem : UGuiComponentClone
{
    Text textId;
    Text textName;
    Text textScore;
    Text textNum;
    Text textSeatNum;

    int index;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        textId = link.Get<Text>("TextId");
        textName = link.Get<Text>("TextName");
        textScore = link.Get<Text>("TextScore");
        textNum = link.Get<Text>("TextNum");
        textSeatNum = link.Get<Text>("TextSeatNum");

        index = 0;
    }

    public void SetItemInfo(RunGameData data, int index)
    {
        this.index = index;

        textId.text = data.room_id;
        textName.text = data.room_name;
        textScore.text = data.base_score.ToString();
        textNum.text = data.player_number.ToString(); ;
        textSeatNum.text = data.room_seat_number.ToString();
    }
}
