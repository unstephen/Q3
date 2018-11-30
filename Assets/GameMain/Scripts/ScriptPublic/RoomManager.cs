using System;
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
	
		public void Init(int id, string name, int clubId)
		{
			if (rData == null)
			{
				rData = new RoomData();
			}
			rData.InitData(id, name, clubId);
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
	}
}
