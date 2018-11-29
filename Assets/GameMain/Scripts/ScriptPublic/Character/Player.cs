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
		/// 发牌
		/// </summary>
		Deal,
	}
/*
记录牌局内的玩家数据
*/
	public class Player
	{
		public ReactiveProperty<int> id;
		public ReactiveProperty<string> name;
		public ReactiveProperty<int> pos;
		public ReactiveProperty<int> clubId;
		protected ReactiveProperty<uint> money;

		protected PlayerStateController stateController;
		protected PlayerHeadInfo headUI;
		protected TableForm _tableUI;
		protected TableForm tableUI
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

		public void InitData(int PlayerId, string PlayerName, uint Money, int ClubId = 0)
		{
			id = new ReactiveProperty<int>(PlayerId);
			clubId = new ReactiveProperty<int>(ClubId);
			name = new ReactiveProperty<string>(PlayerName);
			money = new ReactiveProperty<uint>(Money);
			pos = new ReactiveProperty<int>(-1);
			state = EPlayerState.None;

			stateController = new PlayerStateController();
			//初始化玩家状态机
			stateController.Init(this, GameFrameworkEntry.GetModule<IFsmManager>(),
				new PlayerStateInit(),
				new PlayerStateEnterRoom(),
				new PlayerStateSeat(),
				new PlayerStateDeal()
			);
			stateController.Start<PlayerStateInit>();
		}

		/// <summary>
		/// 设置玩家座位号
		/// </summary>
		/// <param name="index"></param>
		public void SetPos(int index)
		{
			pos.Value = index;
			headUI = tableUI.GetPlayerHeadUI(index);
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

		public void OnEnterRoom()
		{
			Log.Debug("Player OnEnterRoom name={0}", name);
		}


		public void Clear()
		{
			Log.Debug("Player Clear name={0}", name);
			GameFrameworkEntry.GetModule<IFsmManager>().DestroyFsm(stateController.fsm);
			stateController = null;
			id.Dispose();
			pos.Dispose();
			clubId.Dispose();
			money.Dispose();
			foreach (var card in handCards)
			{
				Object.Destroy(card.gameObject);
			}
		}


		public virtual void OnSeat()
		{
			
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

		public void OnDeal()
		{
			Log.Info("开始发牌给{0}", name);
			GetCard(3,GetOutCardPos(),(x)=>handCards.Add(x));
			GetCard(4,GetOutCardPos(1,3),(x)=>handCards.Add(x));
			GetCard(5,GetOutCardPos(2,3),(x)=>handCards.Add(x));
		}

		public void ShowCard()
		{
			foreach (var card in handCards)
			{
				card.SetFrontActive(true);
			}
		}
	}
}
