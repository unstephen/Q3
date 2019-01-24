using System;
using UniRx;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
	
	public class PlayerSelf : Player
	{
		public override void OnSeat()
		{
			base.OnSeat();
			
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(true);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(true);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
		}
		
		public override void OnShowCard()
		{
		   ShowCard();
		
		}

		public override void OnEnterRoom()
		{
			base.OnEnterRoom();
			if (tableUI != null)
			{
				tableUI.BtnSeat.gameObject.SetActive(true);
				tableUI.BtnStartGame.gameObject.SetActive(false);
				tableUI.BtnCancelReady.gameObject.SetActive(false);
				tableUI.BtnLeaveSeat.gameObject.SetActive(false);
				tableUI.BetPanel.SetActive(false);
				tableUI.BtnBanker0.gameObject.SetActive(false);
			}
		}

		public override void OnDeal()
		{
			base.OnDeal();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
		}
		
		public override void OnStart()
		{
			base.OnStart();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
		}
		

		public override void OnSeatPre()
		{
			base.OnSeatPre();
			//可带积分选择UI
			tableUI.OpenSelectScore(balance.Value);
		}

		public override void OnGameReady()
		{
			base.OnGameReady();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(true);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
			NetWorkManager.Instance.Send(Protocal.READY,RoomManager.Instance.rData.gId.Value);
		}

		public override void OnBid()
		{
			base.OnBid();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(true);
		}

		public override void OnBet()
		{
			base.OnBet();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(true);
			tableUI.SliderBet.maxValue = Mathf.Min(balance.Value,RoomManager.Instance.rData.maxBet) ;
		}

		public override void OnSettle()
		{
			base.OnSettle();
			
			
			if (winFlag.Value==1)
			{
				tableUI.DoShowWinEffect();
			}
			else if(winFlag.Value==-1)
			{
				tableUI.DoShowLoseEffect();
			}
			else
			{
				tableUI.DoShowTieEffect();
			}
		}

		public override void OnEnd()
		{
			base.OnEnd();
			tableUI.BtnSeat.gameObject.SetActive(false);
			tableUI.BtnStartGame.gameObject.SetActive(false);
			tableUI.BtnCancelReady.gameObject.SetActive(false);
			tableUI.BtnLeaveSeat.gameObject.SetActive(false);
			tableUI.BetPanel.SetActive(false);
			tableUI.BtnBanker0.gameObject.SetActive(false);
		}
	}
}

