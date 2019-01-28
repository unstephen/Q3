using GamePlay;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SubPanelSerachClub : UGuiComponent
{
    ClubSearchItem searchItem;
    List<ClubSearchItem> searchItemList;
    Button btnMyClub;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();
        link.SetEvent("Create", UIEventType.Click, OnCreateClub);
        btnMyClub = link.Get<Button>("My");
        link.SetEvent("My", UIEventType.Click, OnMyClub);

        searchItem = link.AddComponent<ClubSearchItem>("ClubInfo");
        searchItem.SetActive(false);

        searchItemList = new List<ClubSearchItem>();
    }

    private void OnCreateClub(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role != null)
        {
            role.curClubId.SetValueAndForceNotify(-1);
        }
    }

    private void OnMyClub(object[] args)
    {
        RoleData role = GameManager.Instance.GetRoleData();
        if (role != null)
        {
            role.curClubId.SetValueAndForceNotify(1);
        }
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        //    RoleData role = GameManager.Instance.GetRoleData();
        //    Recv_Get_MainPage mainPage = NetWorkManager.Instance.CreateGetMsg<Recv_Get_MainPage>(GameConst._mainPage,
        //GameManager.Instance.GetSendInfoStringList<Send_Get_MainPage>(role.id.Value, role.token.Value));

        RoleData role = GameManager.Instance.GetRoleData();
        btnMyClub.gameObject.SetActive(role.HasClub());

        string jsonStr = File.ReadAllText("JsonTest/club_1.txt");
        Recv_Get_SearchClub shopData = LitJson.JsonMapper.ToObject<Recv_Get_SearchClub>(jsonStr);
        Debug.Log(jsonStr);

        if (shopData != null)
        {
            int curIndex = 0;
            foreach (var item in shopData.data.list)
            {
                if (curIndex < searchItemList.Count)
                {
                    if (!searchItemList[curIndex].isActiveAndEnabled)
                    {
                        searchItemList[curIndex].SetActive(true);
                    }

                    searchItemList[curIndex].SetItemInfo(item, curIndex);
                }
                else
                {
                    ClubSearchItem tempItem = searchItem.Clone() as ClubSearchItem;
                    if (tempItem)
                    {
                        tempItem.SetActive(true);
                        tempItem.OpenUI();
                        tempItem.SetItemInfo(item, curIndex);

                        searchItemList.Add(tempItem);
                    }
                }

                curIndex++;
            }

            if (curIndex < searchItemList.Count)
            {
                for (int i = curIndex; i < searchItemList.Count; ++i)
                {
                    searchItemList[i].SetActive(false);
                }
            }
        }
    }

    protected override void OnClose(object userData)
    {
        base.OnClose(userData);
    }
}
