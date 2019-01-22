using System;
using System.Collections.Generic;

public class RoleClubData
{
    public List<ClubMemberData> memberList;

    public RoleClubData()
    {
        memberList = new List<ClubMemberData>();
    }

    public void InitData()
    {

    }

    public void AddClubMember(ClubAllMemberData data)
    {
        memberList.Clear();

        if (data.list == null)
        {
            return;
        }

        memberList.AddRange(data.list);
    }

    public List<ClubMemberData> showMemberList(int page)
    {
        if (page <= 0 || memberList == null || memberList.Count == 0)
            return null;

        //ÏÔÊ¾10Ìõ
        int begin = (page - 1) * GameConst.pageSize;
        if (begin >= memberList.Count)
            return null;

        List<ClubMemberData> temp = new List<ClubMemberData>();

        Array.Copy(memberList.ToArray(), begin, temp.ToArray(), 0, GameConst.pageSize);
        return temp;
    }

    public void ClearClubData()
    {
        memberList.Clear();
    }
}