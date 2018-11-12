using System;
using GameFramework;
using GameFramework.Fsm;
using GamePlay;
using UniRx;
using UnityGameFramework.Runtime;

/*
记录牌局内的玩家数据
*/
public class Player
{
	public ReactiveProperty<int> id;
	public ReactiveProperty<string> name;	
	public ReactiveProperty<int> pos;	
	public ReactiveProperty<int> clubId;	
	private ReactiveProperty<uint> _money;

	private PlayerStateController stateController;
	
	public void InitData(int PlayerId, string PlayerName, uint Money,int ClubId = 0)
	{
		id = new ReactiveProperty<int>(PlayerId);
		clubId = new ReactiveProperty<int>(ClubId);
		name = new ReactiveProperty<string>(PlayerName);
		_money = new ReactiveProperty<uint>(Money);
		pos = new ReactiveProperty<int>();
		
		stateController = new PlayerStateController();
		//初始化玩家状态机
		stateController.Init(this,GameFrameworkEntry.GetModule<IFsmManager>(),
				new PlayerStateInit(),
			    new PlayerStateEnterRoom());
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
		Log.Debug("Player OnEnterRoom name={0}",name);
	}
}
