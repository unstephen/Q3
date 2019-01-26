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
    public ReactiveProperty<string> openId;

    public ReactiveProperty<string> name;
	private ReactiveProperty<int> _Money;	
	public ReactiveProperty<int> curClubId; // 当前查看的clubid

    public ReactiveCollection<ClubData> myClubList;

    public ReactiveCollection<GameRecord> recordList; //对局记录，最多保存50条，超出上限的删除最近的一条
	private ReactiveProperty<int> recoreLimite;

    private List<string> curRequestClubList = new List<string>();
    public Dictionary<string, RoleClubData> roleClubList = new Dictionary<string, RoleClubData>();
	
	public void InitData(string roleId, string token, string openId)
	{
		id = new ReactiveProperty<int>(Convert.ToInt32(roleId));
        this.token = new ReactiveProperty<string>(token);
        this.openId = new ReactiveProperty<string>(openId);

        recordList = new ReactiveCollection<GameRecord>();
        myClubList = new  ReactiveCollection<ClubData>();
	}
	
    public void InitBaseData(int clubId)
    {
        curClubId.SetValueAndForceNotify(clubId);
    }

    public void InitClubMemberList(string clubId, Recv_Post_AllClubMember data)
    {
        if (roleClubList == null)
        {
            return;
        }
        if (roleClubList.ContainsKey(clubId))
        {
            roleClubList[clubId].AddClubMember(data.data);
        }
        else
        {
            roleClubList.Add(clubId, new RoleClubData());
            roleClubList[clubId].AddClubMember(data.data);
        }
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
            myClubList = new ReactiveCollection<ClubData>();
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

    public string GetClubIdByIndex(int index)
    {
        if (index < myClubList.Count)
        {
            return myClubList[index].club_id;
        }

        return "";
    }

    private void AddRequestClub(string id)
    {
        if (curRequestClubList == null)
        {
            curRequestClubList = new List<string>();
        }

        curRequestClubList.Add(id);
    }

    public bool CheckRequestClub(string id)
    {
        foreach (var item in curRequestClubList)
        {
            if (item == id)
            {
                return true;
            }
        }

        return false;
        //return curRequestClubList.FindIndex(x => (x == id)) != -1;
    }

    public void DeleteClubMember()
    {

    }

    public void ClearData()
    {
        curRequestClubList.Clear();
        recordList.Clear();
        myClubList.Clear();
    }
}