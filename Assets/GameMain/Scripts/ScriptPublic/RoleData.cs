using System;
using System.Collections.Generic;
using UniRx;

public struct GameRecord
{
    public ReactiveProperty<string> gameName;
	public ReactiveProperty<int> score;

	public GameRecord(string Name,int Score)
	{
		gameName = new ReactiveProperty<string>(Name);
		score = new ReactiveProperty<int>(Score);
	}
}
/*
记录登录的角色数据	
*/
public class RoleData
{
	public ReactiveProperty<int> id;
	public ReactiveProperty<string> name;
	
	private ReactiveProperty<uint> _Money;	
	private ReactiveProperty<int> _clubId;
	
	public ReactiveCollection<GameRecord> recordList; //对局记录，最多保存50条，超出上限的删除最近的一条
	private ReactiveProperty<int> recoreLimite;
	
	public void InitData(int roleId, string roleName, uint money, int clubId)
	{
		id = new ReactiveProperty<int>(roleId);
		name = new ReactiveProperty<string>(roleName);
		_Money = new ReactiveProperty<uint>(money);
		_clubId = new ReactiveProperty<int>(clubId);
		
		recordList = new ReactiveCollection<GameRecord>();
	}
	
	public void AddRecord(string name, int score)
	{
		CheckAddRecord();
		
		GameRecord tempRecord = new GameRecord();
		tempRecord.gameName.Value = name;
		tempRecord.score.Value = score;
		recordList.Add(tempRecord);
	}
	
	public void CheckAddRecord()
	{
		if (recordList.Count >= 50)
		{
			recordList.RemoveAt(0);
		}
	}
	
	public GameRecord GetRecordByIndex(int index)
	{
		return recordList[index];
	}
}