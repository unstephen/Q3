using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubPanelClubInfo : UGuiComponent
{
    string[] toggleStr = new string[3] { "ClubBtn_1", "ClubBtn_2", "ClubBtn_3" };
    List<Button> toggleList = new List<Button>();
    List<Text> labelList = new List<Text>();

    List<ClubData> clubList = new List<ClubData>();

    SubPanelClubInfoList panelInfoList;
    subPanelBaseClubInfo panelInfo;

    private int curIndex;
    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();

        for (int i = 0; i < toggleStr.Length; i++)
        {
            Button temp = link.Get<Button>(toggleStr[i]);
            toggleList.Add(temp);

            Text tempText = link.Get<Text>("Text_" + i);
            labelList.Add(tempText);

            int index = i;
            link.SetEvent(toggleStr[i], UIEventType.Click, _ => OnClickChange(index));
        }

        link.SetEvent("ButtonBack", UIEventType.Click, BackToSearch);

        panelInfoList = link.AddComponent<SubPanelClubInfoList>("Page");
        panelInfo = link.AddComponent<subPanelBaseClubInfo>("PanelBaseInfo");

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

        if (clubList.Count > 0)
        {
            panelInfo.OpenUI(0);
            panelInfo.transform.SetSiblingIndex(1);
        }
        else
        {
            panelInfo.HideUI();
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

            panelInfo.transform.SetSiblingIndex(index + 1);
            panelInfo.ShowBaseInfo(index);
        }

    }
}
