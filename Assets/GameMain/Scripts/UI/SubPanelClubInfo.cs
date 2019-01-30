using GamePlay;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SubPanelClubInfo : UGuiComponent
{
    RoleData role;

    string[] toggleStr = new string[3] { "ClubBtn_1", "ClubBtn_2", "ClubBtn_3" };
    List<Button> toggleList = new List<Button>();
    string[] toggleInfoStr = new string[3] { "ToggleMembers", "ToggleGames", "ToggleRequest" };
    List<Toggle> toggleInfoList = new List<Toggle>();
    List<Text> labelList = new List<Text>();

    List<ClubData> clubList = new List<ClubData>();

    SubPanelClubInfoList panelInfoList; //俱乐部的按钮列表
    subPanelBaseClubInfo panelInfo; //俱乐部的详情面板

    //SubChildPanelMember childMember; //成员
    //SubChildPanelGames childGame; //游戏   
    //SubChildPanelRequest childRequest; //申请
    List<UGuiComponent> childPanelList = new List<UGuiComponent>();

    //全部的俱乐部数据
    List<Recv_Get_ClubInfo> recvClubInfoList = new List<Recv_Get_ClubInfo>();

    private int curIndex;
    private int curChildIndex;
    private bool slefManager;
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

        //panelInfoList = link.AddComponent<SubPanelClubInfoList>("Page");
        panelInfo = link.AddComponent<subPanelBaseClubInfo>("PanelBaseInfo");

        SubChildPanelMember childMember = link.AddComponent<SubChildPanelMember>("PanelMembers");
        childPanelList.Add(childMember);
        SubChildPanelGames childGame = link.AddComponent<SubChildPanelGames>("PanelGames");
        childPanelList.Add(childGame);
        SubChildPanelRequest childRequest = link.AddComponent<SubChildPanelRequest>("PanelRequest");
        childPanelList.Add(childRequest);

        for (int i = 0; i < toggleInfoStr.Length; i++)
        {
            Toggle temp = link.Get<Toggle>(toggleInfoStr[i]);
            toggleInfoList.Add(temp);

            int index = i;
            link.SetEvent(toggleInfoStr[i], UIEventType.Click, _ => OnClickInfoPanelChange(index));
        }

        curIndex = 0;
        curChildIndex = 0;
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        role = GameManager.Instance.GetRoleData();

        recvClubInfoList.Clear();
        for (int i = 0; i < role.myClubList.Count; i++)
        {
            Recv_Get_ClubInfo tempClub = NetWorkManager.Instance.CreateGetMsg<Recv_Get_ClubInfo>(GameConst._mainPage,
                GameManager.Instance.GetSendInfoStringList<Send_GetAllMember>(role.id.Value, role.token.Value, role.GetClubIdByIndex(i)));

            if (tempClub != null && tempClub.code == 0)
            {
                recvClubInfoList.Add(tempClub);
                role.InitRoleClubList(tempClub.data.club_id, tempClub);
            }
        }

        //string jsonStr1 = File.ReadAllText("JsonTest/myclubinfo_1.txt");
        //Recv_Get_ClubInfo myClub_1 = LitJson.JsonMapper.ToObject<Recv_Get_ClubInfo>(jsonStr1);
        //Debug.Log(jsonStr1);
        //recvClubInfoList.Add(myClub_1);
        //string jsonStr2 = File.ReadAllText("JsonTest/myclubinfo_2.txt");
        //Recv_Get_ClubInfo myClub_2 = LitJson.JsonMapper.ToObject<Recv_Get_ClubInfo>(jsonStr2);
        //Debug.Log(jsonStr2);
        //recvClubInfoList.Add(myClub_2);
        //string jsonStr3 = File.ReadAllText("JsonTest/myclubinfo_3.txt");
        //Recv_Get_ClubInfo myClub_3 = LitJson.JsonMapper.ToObject<Recv_Get_ClubInfo>(jsonStr3);
        //Debug.Log(jsonStr3);
        //recvClubInfoList.Add(myClub_3);

        RefreshToggles();
        InitClubInfoPanel(recvClubInfoList[0].data, role.GetSlefManager(recvClubInfoList[0].data.club_id));
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

            InitClubInfoPanel(recvClubInfoList[index].data, role.GetSlefManager(recvClubInfoList[index].data.club_id));
        }

    }

    public void OnClickInfoPanelChange(int index)
    {
        if (index == curChildIndex)
        {
            return;
        }

        curChildIndex = index;
        for (int i = 0; i < childPanelList.Count; i++)
        {
            childPanelList[i].SetActive(index == i);
        }
    }

    public void InitClubInfoPanel(Recv_Get_ClubInfo_Data data, bool showManager)
    {
        childPanelList[0].OpenUI(data.club_id);
        childPanelList[1].OpenUI(data.ongoing_games);
        childPanelList[2].OpenUI(data.applicants);

        childPanelList[1].HideUI();
        childPanelList[2].HideUI();

        toggleInfoList[0].isOn = true;
        toggleInfoList[1].isOn = false;
        toggleInfoList[2].isOn = false;
        toggleInfoList[2].gameObject.SetActive(showManager);

        curChildIndex = 0;
    }
}
