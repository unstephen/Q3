using System;
using System.Collections.Generic;

public struct GameRecord
{
    public string gameName;
	public int score;
}
/*
记录登录的角色数据	
*/
public class RoleData
{
	public int id;
	public string name;
	
	private uint _Money;	
	private int _clubId;
	
	public List<GameRecord> recordList; //对局记录，最多保存50条，超出上限的删除最近的一条
	private int recoreLimite;
	
	public void InitData(int roleId, string roleName, uint money, int clubId)
	{
		id = roleId;
		name = roleName;
		_Money = money;
		_clubId = clubId;
		
		recordList = new List<GameRecord>();
	}
	
	public void AddRecord(string name, int score)
	{
		CheckAddRecord();
		
		GameRecord tempRecord = new GameRecord();
		tempRecord.gameName = name;
		tempRecord.score = score;
		recordList.Add(tempRecord);
	}
	
	public void CheckAddRecord()
	{
		if (recordList.count >= 50)
		{
			recordList.RemoveAt(0);
		}
	}
	
	public GameRecord GetRecordByIndex(int index)
	{
		return GameRecord[index];
	}
}