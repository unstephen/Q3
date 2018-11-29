using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GamePlay
{
	/*
记录牌局房间数据
*/
	public class RoomData
	{
		public ReactiveProperty<int> id;
		public ReactiveProperty<string> name;	
		public ReactiveCollection<PlayerOther> roomPlayers;
		public ReactiveProperty<PlayerSelf> playerSelf;
	
		int _clubId;
	
		public void InitData(int roomId, string roomName, int clubId = 0)
		{
			id = new ReactiveProperty<int>(roomId);
			name = new ReactiveProperty<string>(roomName);
			_clubId = clubId;
		
			roomPlayers = new ReactiveCollection<PlayerOther>();
			//加入自己
			playerSelf = new ReactiveProperty<PlayerSelf>(new PlayerSelf());
		
			var roleSelf = GameManager.Instance.GetRoleData();
			playerSelf.Value.InitData(Convert.ToInt32(roleSelf.id.Value),roleSelf.name.Value,1000);
			//test数据
			var player1 = new PlayerOther();
			player1.InitData(1001,"刘亦菲",1000);
			AddPlayer(player1);
		
			var player2 = new PlayerOther();
			player2.InitData(1002,"奶茶妹妹",1000);
			AddPlayer(player2);
		
			var player3 = new PlayerOther();
			player3.InitData(1003,"东哥",1000);
			AddPlayer(player3);
		}

		public void AddPlayer(PlayerOther player)
		{
			if (CheckJoinRoom(player.clubId.Value))
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

		public void Clear()
		{
			foreach (var player in roomPlayers)
			{
				player.Clear();
			}
			playerSelf.Value.Clear();
			roomPlayers.Clear();
			roomPlayers = null;
		}
	}

}
