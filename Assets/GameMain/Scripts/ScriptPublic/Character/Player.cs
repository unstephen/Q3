using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Resource;
using UniRx;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace GamePlay
{

	public enum EPlayerState
	{
		None,
		/// <summary>
		/// 才进房间
		/// </summary>
		Watch,
		/// <summary>
		/// 坐下中
		/// </summary>
		SeatPre,
		/// <summary>
		/// 坐下
		/// </summary>
		Seat,
		/// <summary>
		/// 准备
		/// </summary>
		GamePrepare,
		/// <summary>
		/// 游戏开始
		/// </summary>
		GameStart,
		/// <summary>
		/// 发牌设置
		/// </summary>
		Deal,
		/// <summary>
		/// 发牌表现
		/// </summary>
		Playing,
		/// <summary>
		/// 庄家
		/// </summary>
		Banker,
		/// <summary>
		/// 下注
		/// </summary>
		Bet,
		/// <summary>
		/// 当局结束
		/// </summary>
		End,
		/// <summary>
		/// 结算
		/// </summary>
		Settle,
	}
/*
记录牌局内的玩家数据
*/
	public class Player
	{
		public ReactiveProperty<int> id = new ReactiveProperty<int>();
		public ReactiveProperty<string> name = new ReactiveProperty<string>();
		public ReactiveProperty<string> userLoc = new ReactiveProperty<string>();
		/// <summary>
		/// 服务器给的逻辑位置，和服务器通信用
		/// </summary>
		public ReactiveProperty<byte> pos = new ReactiveProperty<byte>(255);
		/// <summary>
		/// 客户端UI的表现位置
		/// </summary>
		public ReactiveProperty<byte> uiPos = new ReactiveProperty<byte>(255);
		public ReactiveProperty<int> score = new ReactiveProperty<int>();
		public ReactiveProperty<int> balance = new ReactiveProperty<int>();
		public ReactiveProperty<int> clubId = new ReactiveProperty<int>();
		public ReactiveProperty<int> bet = new ReactiveProperty<int>();
		

		protected PlayerStateController stateController;
		public PlayerHeadInfo headUI;
		protected TableForm _tableUI;
		public TableForm tableUI
		{
			get
			{
				if (_tableUI == null)
				{
					_tableUI = GameEntry.UI.GetUIForm(UIFormId.TableForm) as TableForm;
				}

				return _tableUI;
			}
		}

		public EPlayerState state { get; set; }
		public List<CardItem> handCards = new List<CardItem>();
		public List<int> handCardsData = new List<int>();


		public void SetData<TData>(string name, TData data) where TData : Variable
		{
			stateController.fsm.SetData<TData>(name,data);
		}
		public TData GetData<TData>(string name) where TData : Variable
		{
			return stateController.fsm.GetData<TData>(name);
		}
		public void InitData()
		{
			state = EPlayerState.None;

			stateController = new PlayerStateController();
			
			//初始化玩家状态机
			stateController.Init(this, GameFrameworkEntry.GetModule<IFsmManager>(),
				new PlayerStateInit(),
				new PlayerStateEnterRoom(),
				new PlayerStateSeatPre(),
				new PlayerStateSeat(),
				new PlayerStateGameReady(),
				new PlayerStateStart(),
				new PlayerStateDeal(),
				new PlayerStatePlaying(),
				new PlayerStateBanker(),
				new PlayerStateBet(),
				new PlayerStateEnd(),
				new PlayerStateSettle()
			);
			stateController.Start<PlayerStateInit>();
		}

		/// <summary>
		/// 设置玩家座位号
		/// </summary>
		/// <param name="index"></param>
		public void SetPos(byte index)
		{
			pos.Value = index;
			RoomManager.Instance.rData.SetPos(id.Value, index, 1);
//			bool hasUI = false;
//			for (int i = 0; i < tableUI.playerWigets.Count; i++)
//			{
//				if (tableUI.playerWigets[i].pId == id.Value)
//				{
//					hasUI = true;
//					break;
//				}
//			}
//            if(!hasUI)
//            {
//	            for (int i = 0; i < tableUI.playerWigets.Count; i++)
//	            {
//		            if (tableUI.playerWigets[i].pId <= 0)
//		            {
//			            uiPos.Value = (byte) i;
//			            break;
//		            }
//	            }
//
//	            headUI = tableUI.GetPlayerHeadUI(uiPos.Value);   
//            }
		}

		public Vector3 GetCardAnchor()
		{
			return Camera.main.ScreenToWorldPoint(headUI.cardPos.position) ;
		}

		public virtual Vector3 GetOutCardPos(int index = 1, int count = 1)
		{
			if (count == 1)
				return GetCardAnchor();
			return GetCardAnchor()+ new Vector3(index* GameConst.OUT_CARD_SPAN, 0f, 0f );
		}

		public virtual void OnShowCard()
		{
			
		}
		public virtual void OnEnterRoom()
		{
			Log.Debug("Player OnEnterRoom name={0}", name);
			if (headUI != null)
			{
				headUI.Reset();
				headUI = null;
			}

			pos.Value = 255;
			uiPos.Value = 255;
		}


		public void Clear()
		{
			Log.Debug("Player Clear name={0}", name);
			if (stateController != null)
			{
				GameFrameworkEntry.GetModule<IFsmManager>().DestroyFsm(stateController.fsm);
				stateController = null;
			}

			if (headUI != null)
			{
				headUI.Reset();
				headUI = null;
			}
			id.Dispose();
			pos.Dispose();
			clubId.Dispose();
			if (this is PlayerOther)
			{
				RoomManager.Instance.rData.roomPlayers.Remove(this as PlayerOther);
			}
			foreach (var card in handCards)
			{
				Object.Destroy(card.gameObject);
			}
		}


		public virtual void OnSeat()
		{
			headUI.OnSeat();
		}
		
		public void GetCard( int id, Vector3 pos,Action<CardItem> callback, float scale=1 )
		{
			GameEntry.Resource.LoadAsset(AssetUtility.GetCardAsset(id.ToString()), new LoadAssetCallbacks(
				(assetName, asset, duration, userData) =>
				{
					var tempCard = GameObject.Instantiate(asset as GameObject);
					tempCard.transform.localScale = Vector3.one*scale;
					var itemScript = tempCard.GetComponent<CardItem>( );
					if( itemScript == null )
						itemScript = tempCard.AddComponent<CardItem>( );
					itemScript.Init( id );
#if UNITY_EDITOR
					itemScript.name = id.ToString( );
#endif
					tempCard.transform.position = pos;
					callback(itemScript);
					Log.Info("Load Card '{0}' OK.", id);
				},

				(assetName, status, errorMessage, userData) =>
				{
					Log.Error("Can not load card '{0}' from '{1}' with error message '{2}'.", id, assetName, errorMessage);
				}));
		
		}

		public virtual void OnDeal()
		{
			//发牌
		//	DoDealToPlayer();
		}
		
		public virtual void OnStart()
		{
			headUI.OnStart();
		}
		
		
		public void DoDealToPlayer()
		{
			Log.Info("开始发牌给{0}", name);
			GetCard(3,GetOutCardPos(),(x)=>handCards.Add(x));
			GetCard(4,GetOutCardPos(1,3),(x)=>handCards.Add(x));
			GetCard(5,GetOutCardPos(2,3),(x)=>handCards.Add(x));
		}

		public void GetCard(int id)
		{
			Log.Info("开始发牌{0}给{1}",id, name);
			GetCard(id,GetOutCardPos(handCards.Count,3),(x)=>
			{
				x.gameObject.SetActive(false);
				handCards.Add(x);
			});
		}
/// <summary>
/// 显示某个卡
/// </summary>
		public void ShowLastCard(int index)
		{
			Log.Info("ShowLastCard {0}=,count={1}",index,handCards.Count);
			handCards[index].gameObject.SetActive(true);
		}

		protected void ShowCard()
		{
			foreach (var card in handCards)
			{
				card.SetFrontActive(true);
			}
		}

		public virtual void OnSeatPre()
		{
			
			
		}

		public string GetCardStyleName()
		{
			string ret = String.Empty;
			bool bSame3 = true;
			var card0 = handCardsData[0];
			var card1 = handCardsData[1];
			var card2 = handCardsData[2];
			if(card0 == card1 && card0 == card2)
			{
				//三公
				if (bGongCard(card0))
				{
					//大三公
					return "大三公";
				}
				else
				{
					//小三公
					return "小三公";
				}
			}
			else if (bGongCard(card0)&&bGongCard(card1)&&bGongCard(card2))
			{
				//混三公
				return "混三公";
			}
			else
			{
				//点数
				var cp0 = (bGongCard(card0) || card0%10==0) ? 0 : card0;
				var cp1 = (bGongCard(card1) || card1%10==0) ? 0 : card1;
				var cp2 = (bGongCard(card2) || card2%10==0) ? 0 : card2;
				return string.Format("{0}点", cp0 + cp1 + cp2);
			}
		}

		private bool bGongCard(int card)
		{
			if (card%13 == 11 || card%13 == 12 || card%13 == 0)
				return true;
			else
				return false;
		}
        /// <summary>
        /// 等待游戲開始
        /// </summary>
		public virtual void OnGameReady()
        {
	        headUI.OnGameReady();
        }
        //抢庄
		public virtual void OnBid()
		{
		
		}
        //下注
		public virtual void OnBet()
		{
			
		}
        //结算
		public virtual void OnSettle()
		{
			//UI显示重置
			headUI.OnGameEnd();
			//桌面的牌飞到桌子中间
			CardManager.Instance.OnGameEnd();
		}
        //本局结束
		public virtual void OnEnd()
		{
			headUI.OnEnd();
		}
        //计算牌型
		public virtual void OnCardStyle()
		{
			headUI.OnCardStyle(GetCardStyleName());
		}
	}
}
