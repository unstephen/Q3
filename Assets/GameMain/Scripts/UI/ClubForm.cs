using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using UniRx;

namespace GamePlay
{
    public class ClubForm : UGuiForm
    {
        SubPanelClubInfo clubInfo;
        SubPanelSerachClub searchClub;
        SubPanelCreateClub createClub;

        List<UGuiComponent> panelList = new List<UGuiComponent>();

        int panelIndex; // -1：創建 0：搜索 大于1：信息

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            if (GameManager.Instance.GetRoleData().curClubId.Value == 0)
            {
                panelIndex = 1;
            }
            else
            {
                panelIndex = 2;
            }

            GUILink uiLink = GetComponent<GUILink>();
            clubInfo = uiLink.AddComponent<SubPanelClubInfo>("SubClubInfo");
            panelList.Add(clubInfo);
            searchClub = uiLink.AddComponent<SubPanelSerachClub>("SubSerachClub");
            panelList.Add(searchClub);
            createClub = uiLink.AddComponent<SubPanelCreateClub>("SubCreateClub");
            panelList.Add(createClub);

            uiLink.SetEvent("ButtonClose", UIEventType.Click, OnClickExit);
        }

        void RefreshGoods()
        {

        }

        public void OnClickExit(params object[] args)
        {
            Close();
        }

        public void OnStartButtonClick(params object[] args)
        {

        }


        public void OnQuitButtonClick(params object[] args)
        {
            Log.Debug("quit{0}", 111111);
        }


#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            //            PlayerStateInit s = new PlayerStateInit();
            //            s.star
            RoleData role = GameManager.Instance.GetRoleData();
            if (role == null)
            {
                Debug.LogError("this role is null in clubform!");
            }

            role.curClubId.Subscribe(x =>
            {
                panelIndex = x < 0 ? 2 : (x == 0 ? 1 : 0);
                if (panelList != null)
                {
                    for (int i = 0; i < panelList.Count; i++)
                    {
                        if (i == panelIndex)
                        {
                            panelList[i].OpenUI();
                        }
                        else
                        {
                            panelList[i].HideUI();
                        }
                    }
                }
                //clubInfo.SetActive(x == 2);
                //searchClub.SetActive(x == 1);
                //createClub.SetActive(x == 0);
            }).AddTo(disPosable);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);
        }
    }
}
