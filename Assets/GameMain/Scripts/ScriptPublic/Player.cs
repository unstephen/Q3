using System;

/*
记录牌局内的玩家数据
*/
public class Player
{
	public int id;
	public string name;	
	private uint _money;
	
	
	public void InitData(int playerId, string playerName, uint money)
	{
		id = playerId;
		name = playerName;
		_money = money;
	}
}
