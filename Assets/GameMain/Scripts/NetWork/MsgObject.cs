using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 接受的基础数据
/// </summary>
public class Recv_MsgBase
{
    /// <summary>
    /// 错误码
    /// </summary>
    public int state;
    /// <summary>
    /// 消息编号
    /// </summary>
    public int code;
}

public class Recv_Login : Recv_MsgBase
{
    public uint userId;
    public string token;
}

/// <summary>
/// 进入大厅
/// </summary>
public class Recv_Get_MainPage : Recv_MsgBase
{
    public string name;
    public string image_url;
    public uint balance; //账户余额
}
/// <summary>
/// 商城
/// </summary>
public struct GoodsData
{
    public int goodsId;
    public int goodsType;
    public int num;
    public float price;
}

public class Recv_Get_Shop : Recv_MsgBase
{
    public int goodsCount;
    public List<GoodsData> goodsList;
}
/// <summary>
/// 购买物品
/// </summary>
public class Recv_Post_PurchaseItem : Recv_MsgBase
{
    public int appId;
    public int merchantId;
    public int orderId;
    public string randomStr;
    public string sign;
}
/// <summary>
/// 我的俱乐部
/// </summary>
public class Recv_Get_MyClub : Recv_MsgBase
{

}
/// <summary>
/// 俱乐部详情
/// </summary>
public class Recv_Get_ClubInfo : Recv_MsgBase
{

}
/// <summary>
/// 创建俱乐部
/// </summary>
public class Recv_Post_CreatClub : Recv_MsgBase
{

}
/// <summary>
/// 搜索俱乐部
/// </summary>
public class Recv_Get_SearchClub : Recv_MsgBase
{

}
/// <summary>
/// 申请俱乐部
/// </summary>
public class Recv_Post_ApplyClub : Recv_MsgBase
{

}
/// <summary>
/// 处理俱乐部申请
/// </summary>
public class Recv_Post_HandleRequest : Recv_MsgBase
{

}
/// <summary>
/// 历史战绩（总览）
/// </summary>
public class Recv_Get_History : Recv_MsgBase
{

}
/// <summary>
/// 查询历史战绩
/// </summary>
public class Recv_Get_SearchHistory : Recv_MsgBase
{

}

/// <summary>
/// 发送的基础数据
/// </summary>
public class Send_MsgBase
{
    public int userId;
    public string access_token;
    public double linuxTimes;
}

public class Send_Get_MainPage : Send_MsgBase
{
    
}

public class Send_Get_Shop : Send_MsgBase
{

}

public class Send_Post_PurchaseItem : Send_MsgBase
{

}