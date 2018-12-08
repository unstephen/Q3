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
/// 商城
/// </summary>
public struct GoodsData
{
    public string goods_id;
    public string type;
    public string goods_name;
    public string price;
}

public class Recv_Shop_Data
{
    public string total;
    public List<GoodsData> list;
}

public class Recv_Get_Shop : Http_MsgBase
{
    public Recv_Shop_Data data;
}
/// <summary>
/// 购买物品
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

public class Recv_Get_CheckPrder : Http_MsgBase
{
    public int result_code;
    public string result_msg;
}

/// <summary>
/// 我的俱乐部
/// </summary>
public class Recv_Get_MyClub : Http_MsgBase
{

}
/// <summary>
/// 俱乐部详情
/// </summary>
public class Recv_Get_ClubInfo : Http_MsgBase
{

}
/// <summary>
/// 创建俱乐部
/// </summary>
public class Recv_Post_CreatClub : Http_MsgBase
{

}
/// <summary>
/// 搜索俱乐部
/// </summary>
public class Recv_Get_SearchClub : Http_MsgBase
{

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
public class Recv_Get_History : Http_MsgBase
{

}
/// <summary>
/// 查询历史战绩
/// </summary>
public class Recv_Get_SearchHistory : Http_MsgBase
{

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

public class Send_Post_PurchaseItem : Send_MsgBase
{

}