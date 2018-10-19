using System;
using UnityEngine;

/*
牌局逻辑处理类
*/
public class RoomManager : MonoBehaviour
{
	RoomData rData;
	
	public void Init(int id, string name, int clubId)
	{
        rData = new RoomData();
		rData.InitData(id, name, clubId);
	}
}