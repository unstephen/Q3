using System;
using UniRx;
using UnityEngine;

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
}