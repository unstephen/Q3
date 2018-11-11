using System;
using UniRx;

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
	
	
	public void InitData(int PlayerId, string PlayerName, uint Money,int ClubId = 0)
	{
		id = new ReactiveProperty<int>(PlayerId);
		clubId = new ReactiveProperty<int>(ClubId);
		name = new ReactiveProperty<string>(PlayerName);
		_money = new ReactiveProperty<uint>(Money);
		pos = new ReactiveProperty<int>();
	}
/// <summary>
/// 设置玩家座位号
/// </summary>
/// <param name="index"></param>
	public void SetPos(int index)
	{
		pos.Value = index;
	}
}
