﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening.Plugins;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class TableForm : UGuiForm
    {
        ProcedureMain main;
        public List<PlayerHeadInfo> playerWigets = new List<PlayerHeadInfo>();
        private const int PlayerMax = 6;
        public Button BtnSeat,BtnStartGame,BtnBanker0,BtnCancelReady,BtnLeaveSeat;
        private PanelSelectScore selectScoreUI;
        public GameObject BetPanel,timerPanel;
        public Slider SliderBet;
        private Text TextBet;
        private Text TextTime;
        private Text TextRoomId;
        private Text TextMinBet0;
        private Text TextMinBet1;
        private Text TextMinBet2;
        private Text TextMinBet3;
        private Text timerTxt;
        private Image timerImg;
        private GameObject BL_lose, BL_win,BL_tie,PanelSettle;
        //TODO:聊天
        private Transform PanelChat,TalkPopup;
        private Text TalkPopText;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            for (int i = 0; i < PlayerMax; i++)
            {
                playerWigets.Add(link.AddComponent<PlayerHeadInfo>("playerHead"+i));
            }

            selectScoreUI = link.AddComponent<PanelSelectScore>("PanelSelectScore");
            BtnSeat = link.Get<Button>("BtnSeat");
            BtnStartGame = link.Get<Button>("BtnStartGame");
            BtnBanker0 = link.Get<Button>("BtnBanker0");
            BtnCancelReady = link.Get<Button>("BtnCancelReady");
            BtnLeaveSeat = link.Get<Button>("BtnLeaveSeat");
            BetPanel = link.Get("BetPanel");
            SliderBet = link.Get<Slider>("SliderBet");
            TextBet = link.Get<Text>("TextBet");
            TextTime = link.Get<Text>("TextTime");
            TextRoomId = link.Get<Text>("TextRoomId");
            TextMinBet0 = link.Get<Text>("TextMinBet0");
            TextMinBet1 = link.Get<Text>("TextMinBet1");
            TextMinBet2 = link.Get<Text>("TextMinBet2");
            TextMinBet3 = link.Get<Text>("TextMinBet3");
            timerTxt = link.Get<Text>("timerTxt");
            BL_lose = link.Get("BL_lose");
            BL_win = link.Get("BL_win");
            BL_tie = link.Get("BL_tie");
            
            timerPanel = link.Get("timer");
            timerImg = link.Get<Image>("timerImg");
            PanelSettle = link.Get("PanelSettle");
            
            PanelChat = link.Get("PanelChat").transform;
            TalkPopup = link.Get("TalkPopup").transform;
            TalkPopText = link.Get<Text>("TalkPopText");
            
            
            
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnSeat", UIEventType.Click, OnClickSeat);
            link.SetEvent("BtnStartGame", UIEventType.Click, OnClickStartGame);
            link.SetEvent("BtnCancelReady", UIEventType.Click, OnClickCancelReady);
            link.SetEvent("BtnLeaveSeat", UIEventType.Click, OnClickLeaveSeat);
            link.SetEvent("BtnBanker0", UIEventType.Click, OnClickBid);
            link.SetEvent("BtnBet", UIEventType.Click, OnClickBet);
            link.SetEvent("BtnMinBet0", UIEventType.Click, OnClickMinBet0);
            link.SetEvent("BtnMinBet1", UIEventType.Click, OnClickMinBet1);
            link.SetEvent("BtnMinBet2", UIEventType.Click, OnClickMinBet2);
            link.SetEvent("BtnMinBet3", UIEventType.Click, OnClickMinBet3);
            link.SetEvent("BtnSettleOK", UIEventType.Click, OnClickSettleOK);
            //GM按钮，模拟服务器回消息回调
            link.SetEvent("GMNotifyStart", UIEventType.Click, GMNotifyStart);
            link.SetEvent("GMAddPlayer", UIEventType.Click, GMAddPlayer);
            link.SetEvent("GMNotifyWin", UIEventType.Click, GMNotifyWin);
            link.SetEvent("GMNotifyEnd", UIEventType.Click, GMNotifyEnd);
            link.SetEvent("GMNotifyBetRet", UIEventType.Click, GMNotifyBetRet);
            link.SetEvent("GMNotifyBet", UIEventType.Click, GMNotifyBet);
            link.SetEvent("GMNotifyBidRet", UIEventType.Click, GMNotifyBidRet);
            link.SetEvent("GMNotifyBid", UIEventType.Click, GMNotifyBid);
            link.SetEvent("GMNotifyPrivate", UIEventType.Click, GMNotifyPrivate);
            link.SetEvent("GMNotifyDraw", UIEventType.Click, GMNotifyDraw);
            link.SetEvent("GMNotifyJoin", UIEventType.Click, GMNotifyJoin);
            //聊天
            for (int i = 1; i <= 5; i++)
            {
                string talkUIName = "Talk"+i;
                string talkContent = link.Get<Text>(talkUIName).text;
                link.SetEvent(talkUIName, UIEventType.Click, _=>OnClickTalk(talkContent)); 
            }
            
            
            
            
            List<Vector3> tempUITrans = new List<Vector3>();
            var cam = Camera.main;
            foreach (var w in playerWigets)
            {
                tempUITrans.Add(cam.ScreenToWorldPoint(w.cardPos.position));
            }
            CardManager.Instance.InitCardPos(tempUITrans);
        }


        private void OnClickTalk(string talkContent)
        {
            NetWorkManager.Instance.Send(Protocal.CHAT,RoomManager.Instance.rData.gId.Value,talkContent);
        }

        private void OnClickSettleOK(object[] args)
        {
            //桌面的牌飞到桌子中间
            CardManager.Instance.OnGameEnd();
            
            PanelSettle.SetActive(false);
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                player.state = EPlayerState.Seat;
            }
        }

        public void PopTalk(Transform pos,string talkContent)
        {
            TalkPopup.SetParent(pos);
            TalkPopup.gameObject.SetActive(true);
            TalkPopText.text = talkContent;
            Observable.TimerFrame(50).Subscribe(x => TalkPopup.gameObject.SetActive(false));
        }

        private void GMNotifyJoin(object[] args)
        {
            RoomManager.Instance.Self.Value.SetPos(0);
            RoomManager.Instance.Self.Value.state = EPlayerState.Seat;
            RoomManager.Instance.Self.Value.score.Value = 55555;

        }

        private void GMNotifyDraw(object[] args)
        {
            RoomManager.Instance.Self.Value.handCardsData.Add(1);
            RoomManager.Instance.Self.Value.handCardsData.Add(2);
            RoomManager.Instance.Self.Value.handCardsData.Add(3);
            Log.Debug("给自己发牌数据");
        }

        private void GMNotifyPrivate(object[] args)
        {
            var player = RoomManager.Instance.GetPlayerByPos(5);
            if (player != null)
            {
                player.handCardsData.Add(4);
                player.handCardsData.Add(5);
                player.handCardsData.Add(6);
            }
            Log.Debug("给他人发牌数据");
        }

        private void GMNotifyBid(object[] args)
        {
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                player.state = EPlayerState.Playing;
            }
        }

        private void GMNotifyBidRet(object[] args)
        {
            RoomManager.Instance.rData.bid.Value = RoomManager.Instance.Self.Value.id.Value;
        }

        private void GMNotifyBetRet(object[] args)
        {
            Log.Debug("NOTIFY_BET<<<<<");
            //所有人状态变为发牌
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                if (player != null)
                {
                    player.bet.Value = 222;
                    Log.Debug("{0}下注{1}",player.name.Value,222);
                }
            }
        }

        private void GMNotifyEnd(object[] args)
        {
            Log.Debug("NOTIFY_END<<<<<");
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                player.state = EPlayerState.End;
            }
        }

        private void GMNotifyWin(object[] args)
        {
            Log.Debug("NOTIFY_WIN<<<<<");
            int amount = 66666;
            var player = RoomManager.Instance.Self.Value;
            if (player != null)
            {
                player.score.Value += amount;
                player.SetData<VarBool>(Constant.PlayerData.Settle,amount>0);
                player.state = EPlayerState.Settle;
            }
        }

        private void GMAddPlayer(object[] args)
        {
            Log.Debug("NOTIFY_JOIN<<<<<");
            int pId = 88;
        
            byte pos = 5;
            int score = 20000;
            //通过PID获取玩家的基本信息
            NetWorkManager.Instance.Send(Protocal.PLAYER_INFO,pId);
            
            {
                var player = RoomManager.Instance.rData.GetPlayer(pId) as PlayerOther;
                if (player == null)
                {
                    player = new PlayerOther();
                    player.id.Value = pId;
                    player.name.Value = "测试1";
                    player.SetPos(pos);
                    player.state = EPlayerState.Seat;
                    player.score.Value = score;
                    RoomManager.Instance.rData.roomPlayers.Add(player);
                }
            }
        }

        private void GMNotifyStart(object[] args)
        {
            Log.Debug("NOTIFY_START<<<<<");
            //所有人状态变为发牌
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                player.state = EPlayerState.Deal;
            }
        }

        
        public void DoShowTieEffect()
        {
            PanelSettle.SetActive(true);
            BL_tie.SetActive(true);
            BL_tie.transform.DOScale(2, 1).SetEase(Ease.InFlash);
        }
        public void DoShowWinEffect()
        {
            PanelSettle.SetActive(true);
            BL_win.SetActive(true);
            BL_win.transform.DOScale(2, 1).SetEase(Ease.InFlash);
        }
        public void DoShowLoseEffect()
        {
            PanelSettle.SetActive(true);
            BL_lose.SetActive(true);
            BL_lose.transform.DOScale(2, 1).SetEase(Ease.InFlash);
        }

        private void OnClickBet(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.BET_REQ,RoomManager.Instance.rData.gId.Value, (int)SliderBet.value);
            BetPanel.SetActive(false);
        }
        
        private void OnClickMinBet0(object[] args)
        {
            Log.Debug("下注分数={0}",RoomManager.Instance.rData.minBet);
            NetWorkManager.Instance.Send(Protocal.BET_REQ,RoomManager.Instance.rData.gId.Value, RoomManager.Instance.rData.minBet);
            BetPanel.SetActive(false);
        }
        
        private void OnClickMinBet1(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.BET_REQ,RoomManager.Instance.rData.gId.Value, RoomManager.Instance.rData.minBet*2);
            BetPanel.SetActive(false);
        }
        
        private void OnClickMinBet2(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.BET_REQ,RoomManager.Instance.rData.gId.Value, RoomManager.Instance.rData.minBet*3);
            BetPanel.SetActive(false);
        }
        
        private void OnClickMinBet3(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.BET_REQ,RoomManager.Instance.rData.gId.Value, RoomManager.Instance.rData.minBet*4);
            BetPanel.SetActive(false);
        }


        private void OnClickLeaveSeat(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.LEAVE,RoomManager.Instance.rData.gId.Value);
        }

        private void OnClickCancelReady(object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.READY_CANCEL,RoomManager.Instance.rData.gId.Value);
        }

        private void OnClickStartGame(object[] args)
        {
            BtnSeat.gameObject.SetActive(false);
            BtnStartGame.gameObject.SetActive(false);
            RoomManager.Instance.Self.Value.state = EPlayerState.GamePrepare;
            //CardManager.Instance.DoDealCards();
        }

        public void SetPlayerData(Player player)
        {
            if (player.id.Value == RoomManager.Instance.Self.Value.id.Value)
            {
                playerWigets[0].SetPlayerData(player);
                player.uiPos.Value = 0;
            }
            else
            {
                for (int i = 1; i < playerWigets.Count; i++)
                {
                    if (playerWigets[i].pId == 0)
                    {
                        playerWigets[i].SetPlayerData(player);
                        player.uiPos.Value = (byte)i;
                        break;
                    }
                }
            }

        }
        private void OnClickSeat(object[] args)
        {
            //发送坐下
            NetWorkManager.Instance.Send(Protocal.BALANCE_INFO);
            
//            playerWigets[0].SetPlayerData(RoomManager.Instance.Self.Value);
//            RoomManager.Instance.Self.Value.SetPos(0);
//            for (int i = 1; i <= RoomManager.Instance.rData.roomPlayers.Count; i++)
//            {
//                RoomManager.Instance.rData.roomPlayers[i-1].SetPos(i);
//            }
        }

        public void OpenTalkPanel()
        {
            
        }

        public void OnClickExit(params object[] args)
        {
            NetWorkManager.Instance.Send(Protocal.UNWATCH,RoomManager.Instance.rData.gId.Value);
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
    
            
            RoomManager.Instance.rData.roomPlayers.ObserveAdd().Subscribe(x => RegRxForPlayer(x.Value))
                .AddTo(disPosable);

            RoomManager.Instance.Self.Value.pos.ObserveEveryValueChanged(x => x.Value).Where(x=>x<10).Subscribe(x =>
                {
                    RegRxForPlayer(RoomManager.Instance.Self.Value);
                }).AddTo(disPosable);
            
            SliderBet.OnValueChangedAsObservable().SubscribeToText(TextBet).AddTo(disPosable);
            //下注积分不能超过带入当前截止带入的剩余积分。
            RoomManager.Instance.Self.Value.score.Subscribe(x => SliderBet.maxValue = x).AddTo(disPosable);
            RoomManager.Instance.rData.id.SubscribeToText(TextRoomId).AddTo(disPosable);
            RoomManager.Instance.rData.maxCoolTime.Select(x => x > 0).Subscribe(x =>
            {
                Log.Debug("倒计时开始{0}",x);
                timerPanel.SetActive(x);
            }).AddTo(disPosable);
            Observable.EveryFixedUpdate().Where(x => RoomManager.Instance.rData.timer.Value > 0).Subscribe(x =>
            {
                RoomManager.Instance.rData.timer.Value = RoomManager.Instance.rData.timer.Value - Time.fixedDeltaTime;
                if (RoomManager.Instance.rData.timer.Value <= 0)
                {
                    RoomManager.Instance.rData.timer.Value = -1;
                    RoomManager.Instance.rData.maxCoolTime.Value = -1;
                }

                timerImg.fillAmount = RoomManager.Instance.rData.timer.Value /  RoomManager.Instance.rData.maxCoolTime.Value;
                timerTxt.text = Mathf.CeilToInt(RoomManager.Instance.rData.timer.Value).ToString();
            }).AddTo(disPosable);
            
            TextMinBet0.text = RoomManager.Instance.rData.minBet.ToString();
            TextMinBet1.text = (RoomManager.Instance.rData.minBet*2).ToString();
            TextMinBet2.text = (RoomManager.Instance.rData.minBet*3).ToString();
            TextMinBet3.text = (RoomManager.Instance.rData.minBet*4).ToString();
            SliderBet.minValue = RoomManager.Instance.rData.minBet;
            ResetWiget();
        }

        private void RegRxForPlayer(Player player)
        {
            Log.Debug("RegRxForPlayer");
            SetPlayerData(player);
        }

        private void ResetWiget()
        {
            BtnSeat.gameObject.SetActive(true);
            BtnStartGame.gameObject.SetActive(false);
            BtnCancelReady.gameObject.SetActive(false);
            BtnLeaveSeat.gameObject.SetActive(false);
            BetPanel.SetActive(false);
            BtnBanker0.gameObject.SetActive(false);
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

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            DateTime NowTime = DateTime.Now.ToLocalTime();

            TextTime.text = NowTime.ToString("HH:mm");
        }

        public void OpenSelectScore(int balance)
        {
            selectScoreUI.OpenUI(balance);
        }

        private void OnClickBid(object[] args)
        {
            BtnBanker0.gameObject.SetActive(false);
            //发送抢庄,amount暂时=0
            int amount = 1;
            NetWorkManager.Instance.Send(Protocal.BID,RoomManager.Instance.rData.gId.Value,amount);
        }

        private void GMNotifyBet(object[] args)
        {
            Log.Debug("NOTIFY_BET<<<<<");
            foreach (var player in RoomManager.Instance.rData.allPlayers)
            {
                player.state = EPlayerState.Bet;
            }
        }

    }
    
    public class PlayerHeadInfo : UGuiComponent
    {
        private string _PlayerName;
        private Image headIcon;
        private Text textName;
        private Text textScore;
        public Transform cardPos;
        private Image bidIcon;
        public int pId;
        private Image imgReady;
        private Image scoreImg;
        private Text TextStyle;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            textName = link.Get<Text>("TextName");
            textScore = link.Get<Text>("TextScore");
            TextStyle = link.Get<Text>("TextStyle");
            cardPos = link.Get<Transform>("cardPos");
            bidIcon = link.Get<Image>("bid");
            headIcon = link.Get<Image>("headIcon");
            imgReady = link.Get<Image>("img_ready");
            scoreImg = link.Get<Image>("scoreImg");
            textName.text = "";
            textScore.text = "";
            scoreImg.gameObject.SetActive(false);
            headIcon.gameObject.SetActive(false);
            textScore.gameObject.SetActive(false);
            link.SetEvent("headIcon", UIEventType.Click, OnClickHead);
        }

        private void OnClickHead(object[] args)
        {
            if (pId == GameManager.Instance.GetRoleData().pId.Value)
            {
                
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        public void SetPlayerData(Player role)
        {
            role.headUI = this;
            pId = role.id.Value;
            disPosable.Clear();
            _PlayerName = name;
            headIcon.gameObject.SetActive(true);
            textScore.gameObject.SetActive(true);
            Log.Debug("{0} SetPlayerData {1}",gameObject.name,role.name);
            role.name.ObserveEveryValueChanged(x => x.Value).Select(x=>x+pId).SubscribeToText(textName).AddTo(disPosable);
            role.score.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textScore).AddTo(disPosable);
            RoomManager.Instance.rData.bid.ObserveEveryValueChanged(x => x.Value).Select(x => x == role.id.Value)
                .Subscribe(
                    x =>
                    {
                        bidIcon.gameObject.SetActive(x);
                    }).AddTo(disPosable);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        public void Reset()
        {
            textName.text = "";
            textScore.text = "";
            pId = 0;
            bidIcon.gameObject.SetActive(false);
            imgReady.gameObject.SetActive(false);
            scoreImg.gameObject.SetActive(false);
            headIcon.gameObject.SetActive(false);
            textScore.gameObject.SetActive(false);
        }

        public void OnGameEnd()
        {
            bidIcon.gameObject.SetActive(false);
        }

        public void OnGameReady()
        {
            imgReady.gameObject.SetActive(true);
        }

        public void OnEnd()
        {
            imgReady.gameObject.SetActive(false);
        }

        public void OnCardStyle(string styleName)
        {
            scoreImg.gameObject.SetActive(true);
            TextStyle.text = styleName;
        }
        
        public void OnStart()
        {
            imgReady.gameObject.SetActive(false);
        }

        public void OnSeat()
        {
            imgReady.gameObject.SetActive(false);
            scoreImg.gameObject.SetActive(false);
        }
    }
    public class PanelSelectScore : UGuiComponent
    {
        private Text TextScore;
        private Slider slider;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            TextScore = link.Get<Text>("TextScore");
            slider = link.Get<Slider>("Slider");
            link.SetEvent("ButtonOK", UIEventType.Click, OnButtonOK);
            link.SetEvent("ButtonClose", UIEventType.Click, OnButtonClose);
            
        }

        private void OnButtonClose(object[] args)
        {
            RoomManager.Instance.Self.Value.state = EPlayerState.Watch;
            OnClose(null);
        }

        private void OnButtonOK(object[] args)
        {
            byte pos = 0;
            int gId = RoomManager.Instance.rData.gId.Value;
            foreach (var seatData in RoomManager.Instance.rData.roomSeats)
            {
                if (seatData.pid <= 0)
                    pos = seatData.pos;
            }

            
           NetWorkManager.Instance.Send(Protocal.JOIN,gId,pos,(int)slider.value);
           OnClose(null);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            slider.OnValueChangedAsObservable().SubscribeToText(TextScore).AddTo(disPosable);
            int balance = (int)userData;
            slider.maxValue = balance;
        }


        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }
    }
}
