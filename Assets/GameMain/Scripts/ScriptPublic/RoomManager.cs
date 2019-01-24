using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace GamePlay
{
	
/*
牌局逻辑处理类
*/
	public class RoomManager : MonoSingleton<RoomManager>
	{
		public RoomData rData;
	
		public void Init(int GID,int id, string name, int clubId,int max_score,int base_score)
		{
			if (rData == null)
			{
				rData = new RoomData();
			}
			rData.InitData(GID,id, name, clubId);
			rData.maxBet = max_score;
			rData.minBet = base_score;
		}

		public ReactiveProperty<PlayerSelf> Self
		{
			get
			{
				if (rData != null)
				{
					return rData.playerSelf;
				}

				return null;
			}
		}

		/// <summary>
		/// 开始发牌
		/// </summary>
		public void StartDealCard()
		{
	
//			//测试几秒后开牌
//			Observable.TimerFrame(30).Subscribe(x =>
//			{
//				Self.Value.ShowCard();
//				foreach (var other in rData.roomPlayers)
//				{
//					other.ShowCard();
//				}
//			});
		}

		public Player GetPlayerByPos(byte pos)
		{
			Player ret = null;
			foreach (var player in rData.allPlayers)
			{
				if (player.pos.Value == pos)
				{
					ret = player;
					break;
				}
			}

			return ret;
		}
		public int GetReadyPlayerCount()
		{
			int ret = 0;
			foreach (var player in rData.allPlayers)
			{
				if (player.pos.Value > 0)
				{
					ret++;
				}
			}

			return ret;
		}
	}
}
