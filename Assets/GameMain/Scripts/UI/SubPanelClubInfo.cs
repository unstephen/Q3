using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubPanelClubInfo : UGuiComponent
{
    string[] toggleStr = new string[3] { "Toggle1", "Toggle2", "Toggle3" };
    List<Toggle> toggleList = new List<Toggle>();
    List<Text> labelList = new List<Text>();

    List<ClubData> clubList = new List<ClubData>();

    SubPanelClubInfoList panelInfoList;

    Text text_id;
    Text text_name;
    Text text_info;
    Text text_vip;
    Text text_count;

    private int curIndex;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();

        for (int i = 0; i < toggleStr.Length; i++)
        {
            Toggle temp = link.Get<Toggle>(toggleStr[i]);
            toggleList.Add(temp);

            Text tempText = link.Get<Text>("Label" + i);
            labelList.Add(tempText);

            int index = i;
            link.SetEvent(toggleStr[i], UIEventType.Click, _ => OnClickChange(index));
        }

        link.SetEvent("ButtonBack", UIEventType.Click, BackToSearch);

        panelInfoList = link.AddComponent<SubPanelClubInfoList>("Page");

        text_id = link.Get<Text>("id_Text");
        text_name = link.Get<Text>("name_Text");
        text_info = link.Get<Text>("info_Text");
        text_vip = link.Get<Text>("vip_Text");
        text_count = link.Get<Text>("count_Text");

        curIndex = 0;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        RefreshToggles();
    }

    private void BackToSearch(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role != null)
        {
            role.curClubId.SetValueAndForceNotify(0);
        }
    }

    public void RefreshToggles()
    {
        clubList.Clear();

        RoleData role = GameManager.Instance.GetRoleData();
        foreach (var item in role.myClubList)
        {
            clubList.Add(item);
        }

        for (int i = 0; i < labelList.Count; i++)
        {
            if (i < clubList.Count)
            {
                labelList[i].text = clubList[i].club_name;
            }
            else
            {
                labelList[i].text = "暂无";
            }
        }
    }

    public void OnClickChange(int index)
    {
        RoleData role = GameManager.Instance.GetRoleData();

        if (index < role.myClubList.Count)
        {
            if (index == curIndex)
            {
                return;
            }

            curIndex = index;

            ClubData temp = role.myClubList[index];
            text_id.text = temp.club_id;
            text_name.text = temp.club_name;
            text_info.text = temp.ongoing_games;
            text_vip.text = temp.vip_level;
            text_count.text = temp.member_number;


        }
    }
}
