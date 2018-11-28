using System;
using GameFramework;
using GameFramework.Fsm;
using UniRx;
using UnityGameFramework.Runtime;

namespace GamePlay
{

/*
记录牌局内的玩家数据
*/
	public class Player
	{
		public ReactiveProperty<int> id;
		public ReactiveProperty<string> name;
		public ReactiveProperty<int> pos;
		public ReactiveProperty<int> clubId;
		private ReactiveProperty<uint> money;

		private PlayerStateController stateController;

		private TableForm _tableUI;
		private TableForm tableUI
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

		public void InitData(int PlayerId, string PlayerName, uint Money, int ClubId = 0)
		{
			id = new ReactiveProperty<int>(PlayerId);
			clubId = new ReactiveProperty<int>(ClubId);
			name = new ReactiveProperty<string>(PlayerName);
			money = new ReactiveProperty<uint>(Money);
			pos = new ReactiveProperty<int>(-1);
		

			stateController = new PlayerStateController();
			//初始化玩家状态机
			stateController.Init(this, GameFrameworkEntry.GetModule<IFsmManager>(),
				new PlayerStateInit(),
				new PlayerStateEnterRoom(),
				new PlayerStateSeat()
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
		}


		public void OnSeat()
		{
			Log.Debug("Player OnSeat name={0}", name);
			tableUI.BtnSeat.interactable = false;
		}
	}
}
