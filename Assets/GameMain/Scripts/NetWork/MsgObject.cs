using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

/// <summary>
/// 接受的基础数据
/// </summary>
public class Http_MsgBase
{
    /// <summary>
    /// 消息编号
    /// </summary>
    public int code;

    /// <summary>
    /// 错误码
    /// </summary>
    public string errMsg;
}

public class WS_MsgBase
{
    public int state;
}

public class Recv_Login_Data
{
    public string user_id;
    public string access_token;
}
public class Recv_Login : Http_MsgBase
{
    public Recv_Login_Data data;
}

/// <summary>
/// 进入大厅
/// </summary>
public class Recv_MainPage_Data
{
    public string nick_name;
    public string head_image_url;
    public string account_balance; //账户余额
}

public class Recv_Get_MainPage : Http_MsgBase
{
    public Recv_MainPage_Data data;
}
/// <summary>
/// 创建房间
/// </summary>
public class Recv_CreateRoom_Data
{
    public int GID;
    public int room_id;
}

public class Recv_CreateRoom : Http_MsgBase
{
    public Recv_CreateRoom_Data data;
}
/// <summary>
/// 商城
/// </summary>
public struct GoodsData
{
    public string goods_id;
    public string type;
    public string goods_name;
    public string price;
}

public struct Recv_Shop_Data
{
    public int total;
    public List<GoodsData> list;
}

public class Recv_Get_Shop : Http_MsgBase
{
    public Recv_Shop_Data data;
}
/// <summary>
/// 订单
/// </summary>
public struct Recv_Order_Data
{
    public string appid;
    public string partnerid;
    public string prepayid;
    public string noncestr;
    public string sign;
}

public class Recv_Post_Order : Http_MsgBase
{
    Recv_Order_Data data;
}

public struct Recv_CheckOrder_Data
{
    public int result_code;
    public string result_msg;
}

public class Recv_Get_CheckOrder : Http_MsgBase
{
    Recv_CheckOrder_Data data;
}

/// <summary>
/// 我的俱乐部
/// </summary>
public struct ClubData
{
    public string club_id;
    public string club_name;
    public int vip_level;
    public int member_number;
    public int ongoing_games;
}

public struct Recv_Get_MyClub_Data
{
    public int total;
    public List<ClubData> list;
}

public class Recv_Get_MyClub : Http_MsgBase
{
    public Recv_Get_MyClub_Data data;
}
/// <summary>
/// 俱乐部详情
/// </summary>
public struct RunGameData
{
    public string room_id;
    public string room_name;
    public int base_score;
    public int player_number;
    public int room_seat_number;
}

public struct ManagerData
{
    public string user_id;
    public string nick_name;
    public string head_image_url;
}

public struct ApplicantsData
{
    public string apply_id;
    public string user_id;
    public string nick_name;
    public string head_image_url;
}

public struct Recv_Get_ClubInfo_Data
{
    public string club_id;
    public string club_name;
    public string club_description;
    public bool allow_by_search;
    public int vip_level;
    public int member_number;
    public int ongoing_game_number;
    public List<RunGameData> ongoing_games;
    public List<ManagerData> managers;
    public List<ApplicantsData> applicants;
}

public class Recv_Get_ClubInfo : Http_MsgBase
{
    public Recv_Get_ClubInfo_Data data;
}
/// <summary>
/// 创建俱乐部
/// </summary>

public struct Recv_Post_CreatClub_Data
{
    public string club_id;
    public string club_name;
    public string club_description;
    public bool allow_by_search;
    public int vip_level;
    public int member_number;
    public int ongoing_game_number;
    public List<ManagerData> managers;
}
public class Recv_Post_CreatClub : Http_MsgBase
{
    public Recv_Post_CreatClub_Data data;
}

/// <summary>
/// 搜索俱乐部
/// </summary>
public struct SearchClubData
{
    public int total;
    public int page;
    public int page_size;
    public List<SearchClubBaseData> list;
}

public struct SearchClubBaseData
{
    public string club_id;
    public string club_name;
    public string create_user_name;
}

public class Recv_Get_SearchClub : Http_MsgBase
{
    public SearchClubData data;
}
/// <summary>
/// 申请俱乐部
/// </summary>
public class Recv_Post_ApplyClub : Http_MsgBase
{

}
/// <summary>
/// 处理俱乐部申请
/// </summary>
public class Recv_Post_HandleRequest : Http_MsgBase
{

}
/// <summary>
/// 历史战绩（总览）
/// </summary>
public struct HistoryAllBaseData
{
    public string type;
    public int games_count;
    public int profit;
}

public struct HistoryAll
{
    public List<HistoryAllBaseData> list;
}

public class Recv_Get_History : Http_MsgBase
{
    public HistoryAll data;
}
/// <summary>
/// 查询历史战绩
/// </summary>
public struct HistorySingleBaseData
{
    public string room_id;
    public string room_name;
    public string club_name;
    public int base_score;
    public string room_type;
}

public struct HistorySingle
{
    public int total;
    public int page;
    public int page_size;
    public string message;
    public List<HistorySingleBaseData> list;
}

public class Recv_Get_SearchHistory : Http_MsgBase
{
    public HistorySingle data;
}

public class Recv_SearchRoom_Data
{
    public int room_id;
    public string room_name;
    public string create_user;
    public int countdown;
}
public class Recv_SearchRoom : Http_MsgBase
{
    public Recv_SearchRoom_Data data;
}

public class Recv_JoinRoom_Data
{
    public int room_id;
    public int GID;
}
public class Recv_JoinRoom : Http_MsgBase
{
    public Recv_JoinRoom_Data data;
}

/// <summary>
/// 发送的基础数据
/// </summary>
public class Send_MsgBase
{
    public List<string> strList;

    public virtual List<string> CreateSendInfo(params object[] args)
    {
        if (strList == null)
        {
            strList = new List<string>();
        }
        else
        {
            strList.Clear();
        }

        strList.Add("user_id=" + args[0]);
        strList.Add("access_token=" + args[1]);

        //每次都要重新获取时间戳
        long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        string timeStr = "timestamp=" + time.ToString();
        strList.Add(timeStr);

        return strList;
    }
}

public class Send_Get_Login : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        return base.CreateSendInfo(args);
    }
}

public class Send_Get_MainPage : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        return base.CreateSendInfo(args);
    }
}

public class Send_Get_Shop : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        return base.CreateSendInfo(args);
    }
}



/// <summary>
/// 订单
/// </summary>
public class Send_Post_Order : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("goods_id=" + (string)args[2]);

        return temp;
    }
}

public class Send_Get_Checkorder : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("prepay_id=" + (string)args[2]);

        return temp;
    }
}

public class Send_Get_MyClub : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        return base.CreateSendInfo(args);
    }
}


public class Send_Get_ClubInfo : Send_MsgBase
{
 public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);
        temp.Add("club_id=" + (string)args[2]);
        return temp;
    }
}

/// <summary>
/// 搜索房间
/// </summary>
public class Send_Search_Room : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);
        temp.Add("room_id=" + (string)args[2]);
        return temp;
    }
}


public class Send_Post_CreateClub : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("club_name=" + args[2]);
        temp.Add("club_description=" + args[3]);
        temp.Add("allow_by_search=" + args[4]);

        return temp;
    }
}
/// <summary>
/// 加入房间
/// </summary>
public class Send_Join_Room : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("room_id=" + (string)args[2]);
        temp.Add("password=" + (string)args[3]);

        return temp;
    }
}
/// <summary>
/// 创建房间
/// </summary>
public class Send_Create_Room : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("club_id=" + (string)args[2]);
        temp.Add("room_name=" + (string)args[3]);
        temp.Add("password=" + (string)args[4]);
        temp.Add("base_score=" + (string)args[5]);
        temp.Add("game_type=" + (string)args[6]);
        temp.Add("room_seat_number=" + (string)args[7]);
        temp.Add("game_duration=" + (string)args[8]);

        return temp;
    }
}

public class Send_SearchClub : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("query=" + args[2]);
        temp.Add("page=" + (string)args[3]);
        temp.Add("page_size=" + (string)args[4]);

        return temp;
    }
}

public class Send_RequestClub : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("club_id=" + (string)args[2]);

        return temp;
    }
}

public class Send_AgreenClub : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("apply_id=" + args[2]);
        temp.Add("action=" + args[3]); // refuse/accept

        return temp;
    }
}

public class Send_SearchHistoryAll : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        return base.CreateSendInfo(args);
    }
}

public class Send_SearchHistory : Send_MsgBase
{
    public override List<string> CreateSendInfo(params object[] args)
    {
        List<string> temp = new List<string>();
        temp = base.CreateSendInfo(args);

        temp.Add("page=" + args[2]);
        temp.Add("page_size=" + args[3]);

        return temp;
    }
}