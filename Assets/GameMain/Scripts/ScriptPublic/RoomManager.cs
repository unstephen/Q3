using System;
using UniRx;
using UnityEngine;

/*
牌局逻辑处理类
*/
public class RoomManager
{
	private static RoomManager _Instance = null;

	/// <summary>Get the singleton instance</summary>
	public static RoomManager Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = new RoomManager();
			}
			return _Instance;
		}
	}
	
	ReactiveProperty<RoomData> rData = new ReactiveProperty<RoomData>(new RoomData());
	
	public void Init(int id, string name, int clubId)
	{
		rData.Value.InitData(id, name, clubId);
	}
}