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
	public ReactiveProperty<int> pId = new ReactiveProperty<int>();

    public ReactiveProperty<string> token;

	public ReactiveProperty<string> name;
	private ReactiveProperty<int> _Money;	
	public ReactiveProperty<int> curClubId; // 当前查看的clubid

    public List<ClubData> myClubList;

	public ReactiveCollection<GameRecord> recordList; //对局记录，最多保存50条，超出上限的删除最近的一条
	private ReactiveProperty<int> recoreLimite;
	
	public void InitData(string roleId, string token)
	{
		id = new ReactiveProperty<int>(Convert.ToInt32(roleId));
        this.token = new ReactiveProperty<string>(token);

        recordList = new ReactiveCollection<GameRecord>();
        myClubList = new List<ClubData>();
	}
	
    public void InitBaseData(int clubId)
    {
        curClubId.SetValueAndForceNotify(clubId);
    }

    public void SetRoleProperty(Recv_MainPage_Data pageData)
    {
        if (pageData != null)
        {
            name = new ReactiveProperty<string>(pageData.nick_name);
            _Money = new ReactiveProperty<int>(int.Parse(pageData.account_balance));
            curClubId = new ReactiveProperty<int>(1);
        }
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

    public void AddMyClubListData(Recv_Get_MyClub_Data data)
    {
        if (myClubList == null)
        {
            myClubList = new List<ClubData>();
        }
        else
        {
            myClubList.Clear();
        }
        if (data.list == null)
        {
            curClubId.SetValueAndForceNotify(0);
        }
        else
        {
            foreach (var item in data.list)
            {
                myClubList.Add(item);
            }
            curClubId.SetValueAndForceNotify(myClubList.Count > 0 ? int.Parse(myClubList[0].club_id) : 0);
        }
    }
}