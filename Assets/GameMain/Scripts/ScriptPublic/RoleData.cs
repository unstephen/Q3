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
	public ReactiveProperty<string> id;
	public ReactiveProperty<int> pId = new ReactiveProperty<int>();
    public string headUrl;

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
		id = new ReactiveProperty<string>(roleId);
        this.token = new ReactiveProperty<string>(token);
        this.openId = new ReactiveProperty<string>(openId);

        recordList = new ReactiveCollection<GameRecord>();
        myClubList = new  ReactiveCollection<ClubData>();
	}
	
    public void InitBaseData(int clubId)
    {
        curClubId.SetValueAndForceNotify(clubId);
    }

    public void InitRoleClubList(string clubId, Recv_Get_ClubInfo data)
    {
        if (roleClubList == null)
        {
            return;
        }

        bool result = false;
        if (roleClubList.ContainsKey(clubId))
        {

        }
        else
        {
            roleClubList.Add(clubId, new RoleClubData());
        }

        foreach (var item in data.data.managers)
        {
            if (item.user_id == id.Value)
            {
                result = true;
                break;
            }
        }
        roleClubList[clubId].SetSelfManager(result);
    }

    public bool GetSlefManager(string clubId)
    {
        if (roleClubList.ContainsKey(clubId))
        {
            return roleClubList[clubId].selfManager;
        }

        return false;
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

    public List<ClubMemberData> GetClubMemberSById(string clubId, int page)
    {
        if (roleClubList.ContainsKey(clubId))
        {
            return roleClubList[clubId].showMemberList(page);
        }

        return null;
    }

    public void DeleteClubMember(string clubId, string memberId)
    {
        if (roleClubList.ContainsKey(clubId))
        {
            roleClubList[clubId].RemoveMember(memberId);
        }
    }

    public void ClearClubData()
    {
        curRequestClubList.Clear();
        myClubList.Clear();

        foreach (var item in roleClubList.Values)
        {
            item.ClearClubData();
        }
        roleClubList.Clear();
    }

    public void SetRoleProperty(Recv_MainPage_Data pageData)
    {
        if (pageData != null)
        {
            name = new ReactiveProperty<string>(pageData.nick_name);
            _Money = new ReactiveProperty<int>(int.Parse(pageData.account_balance));
            curClubId = new ReactiveProperty<int>(1);

            headUrl = pageData.head_image_url;
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

    public bool HasClub()
    {
        return myClubList != null && myClubList.Count > 0;
    }

    public string GetClubIdByIndex(int index)
    {
        if (index < myClubList.Count)
        {
            return myClubList[index].club_id;
        }

        return "";
    }

    public void AddRequestClub(string id)
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