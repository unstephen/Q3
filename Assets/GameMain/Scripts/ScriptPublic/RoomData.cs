using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
	/*
记录牌局房间数据
*/
	public class RoomData
	{
		public ReactiveProperty<int> gId;
		public ReactiveProperty<int> win_pId;
		public ReactiveProperty<int> id;
		public ReactiveProperty<int> bid;
		public ReactiveProperty<string> name;	
		public ReactiveCollection<PlayerOther> roomPlayers;
		public ReactiveProperty<PlayerSelf> playerSelf;
		public ReactiveCollection<RoomSeat> roomSeats;

		public List<Player> allPlayers
		{
			get
			{
				List<Player> ret = new List<Player>();
				ret.Add(playerSelf.Value);
				foreach (var other in roomPlayers)
				{
					if (other != null)
					{
						ret.Add(other);
					}
				}

				ret = ret.OrderBy(x => x.uiPos.Value).ToList();
				return ret;
			}
		}

		int _clubId;
	
		public void InitData(int gId,int roomId, string roomName, int clubId = 0)
		{
			id = new ReactiveProperty<int>(roomId);
			win_pId = new ReactiveProperty<int>(0);
			this.gId = new ReactiveProperty<int>(gId);
			bid = new ReactiveProperty<int>(0);
			name = new ReactiveProperty<string>(roomName);
			_clubId = clubId;
		
			roomPlayers = new ReactiveCollection<PlayerOther>();
			
			roomSeats = new ReactiveCollection<RoomSeat>();
			for (int i = 0; i < 6; i++)
			{
				roomSeats.Add(new RoomSeat(){pos = (byte)i});
			}
    		//加入自己
    		playerSelf = new ReactiveProperty<PlayerSelf>(new PlayerSelf());
    	
    		var roleSelf = GameManager.Instance.GetRoleData();
			playerSelf.Value.id = roleSelf.pId;
			playerSelf.Value.name = roleSelf.name;
    		playerSelf.Value.InitData();
			
//			//test数据
//			var player1 = new PlayerOther();
//			player1.InitData(1001,"刘亦菲",1000);
//			AddPlayer(player1);
//		
//			var player2 = new PlayerOther();
//			player2.InitData(1002,"奶茶妹妹",1000);
//			AddPlayer(player2);
//		
//			var player3 = new PlayerOther();
//			player3.InitData(1003,"东哥",1000);
//			AddPlayer(player3);
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
			Log.Info("room清楚玩家");
			if(roomPlayers==null)
				return;
			
			foreach (var player in roomPlayers)
			{
				player.Clear();
			}
			playerSelf.Value.Clear();
			roomPlayers.Clear();
			roomPlayers = null;
		}

		public void SetPlayerData(int pId, string userId, string userLoc, int score)
		{
			var player = GetPlayer(pId);
			if (player==null)
			{
				player = new PlayerOther();
				player.id.Value = pId;
			}
			
			player.name.Value = userId;
			player.score.Value = score;
			player.userLoc.Value = userLoc;
		}

		public Player GetPlayer(int pId)
		{
			foreach (var player in roomPlayers)
			{
				if (player.id.Value == pId)
					return player;
			}

			return null;
		}

		public void SetPos(int pid,byte pos,byte state)
		{
			RoomManager.Instance.rData.roomSeats[pos].pid = pid;
			RoomManager.Instance.rData.roomSeats[pos].pos = pos;
			RoomManager.Instance.rData.roomSeats[pos].state = state;
		}
	}
/// <summary>
/// 座位信息
/// </summary>
	public class RoomSeat
	{
		public byte pos;
		public byte state;
		public int pid;
	}

}
