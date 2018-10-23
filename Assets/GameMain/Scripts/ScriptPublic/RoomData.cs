using System;
using System.Collections.Generic;
using UnityEngine;

/*
记录牌局房间数据
*/
public class RoomData
{
	public int id;
	public string name;
	
	List<Player> roomPlayers;
	
	int _clubId;
	
	public void InitData(int roomId, string roomName, int clubId = 0)
	{
		id = roomId;
		name = roomName;
		_clubId = clubId;
		
		roomPlayers = new List<Player>();
	}

	public void AddPlayer(Player player)
	{
		if (CheckJoinRoom(player.id))
		{
			roomPlayers.Add(player);
		}
	}
	
	private bool CheckJoinRoom(int clubId)
	{
		if (_clubId == 0)
			return true;
		else
		{
			if (clubId == _clubId)
			{
				return true;
			}
			else
			{
				Debug.Log("The game only allows club members to join!");
			}
		}
        return false;
	}
}