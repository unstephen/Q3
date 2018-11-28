using System.Collections.Generic;
using GameFramework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class TableForm : UGuiForm
    {
        ProcedureMain main;
        List<PlayerHeadInfo> playerWigets = new List<PlayerHeadInfo>();
        private const int PlayerMax = 6;
        public Button BtnSeat;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            for (int i = 0; i < PlayerMax; i++)
            {
                playerWigets.Add(link.AddComponent<PlayerHeadInfo>("playerHead"+i));
            }

            BtnSeat = link.Get<Button>("BtnSeat");
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnSeat", UIEventType.Click, OnClickSeat);
        }

        private void OnClickSeat(object[] args)
        {
            playerWigets[0].SetPlayerData(GameManager.Instance.GetRoleData());
            RoomManager.Instance.Self.Value.SetPos(0);
        }

        public void OnClickExit(params object[] args)
        {
            Close();
            main.ChangeGame(GameMode.Lobby);
        }


        public PlayerHeadInfo GetPlayerHeadUI(int pos)
        {
            if (pos >= 0 && pos < playerWigets.Count)
            {
                return playerWigets[pos];
            }

            return null;
        }
#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);


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
    
    public class PlayerHeadInfo : UGuiComponent
    {
        private string _PlayerName;
        private Text textName;
        public Transform cardPos;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            textName = link.Get<Text>("TextName");
            textName.text = "";
            cardPos = link.Get<Transform>("cardPos");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        public void SetPlayerData(RoleData role)
        {
            disPosable.Clear();
            _PlayerName = name;
          
            role.name.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textName).AddTo(disPosable);
   
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }
    }
}
