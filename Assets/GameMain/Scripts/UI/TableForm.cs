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
        public List<PlayerHeadInfo> playerWigets = new List<PlayerHeadInfo>();
        private const int PlayerMax = 6;
        public Button BtnSeat,BtnStartGame;
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
            BtnStartGame = link.Get<Button>("BtnStartGame");
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnSeat", UIEventType.Click, OnClickSeat);
            link.SetEvent("BtnStartGame", UIEventType.Click, OnClickStartGame);
            
            List<Vector3> tempUITrans = new List<Vector3>();
            var cam = Camera.main;
            foreach (var w in playerWigets)
            {
                tempUITrans.Add(cam.ScreenToWorldPoint(w.cardPos.position));
            }
            CardManager.Instance.InitCardPos(tempUITrans);
        }

        private void OnClickStartGame(object[] args)
        {
            BtnSeat.gameObject.SetActive(false);
            BtnStartGame.gameObject.SetActive(false);
            CardManager.Instance.DoDealCards();
        }

        private void TestSetPlayerData()
        {
            for (int i = 1; i <= RoomManager.Instance.rData.roomPlayers.Count; i++)
            {
                playerWigets[i].SetPlayerData(RoomManager.Instance.rData.roomPlayers[i-1]);
            }
        }
        private void OnClickSeat(object[] args)
        {
            //发送坐下
            NetWorkManager.Instance.Send(Protocal.SITIN,RoomManager.Instance.rData.id.Value);
            playerWigets[0].SetPlayerData(RoomManager.Instance.Self.Value);
            RoomManager.Instance.Self.Value.SetPos(0);
            for (int i = 1; i <= RoomManager.Instance.rData.roomPlayers.Count; i++)
            {
                RoomManager.Instance.rData.roomPlayers[i-1].SetPos(i);
            }
        }

        public void OnClickExit(params object[] args)
        {
            Close(true);
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
            TestSetPlayerData();
            ResetWiget();
        }

        private void ResetWiget()
        {
            BtnSeat.gameObject.SetActive(true);
            BtnStartGame.gameObject.SetActive(true);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {

            base.OnClose(userData);
            BtnSeat.interactable = true;
            foreach (var wight in playerWigets)
            {
                wight.Reset();
            }
            
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

        public void SetPlayerData(Player role)
        {
            disPosable.Clear();
            _PlayerName = name;
          
            role.name.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textName).AddTo(disPosable);
   
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            Log.Debug("PlayerHeadInfo 关闭");
        }

        public void Reset()
        {
            textName.text = "";
        }
    }
}
