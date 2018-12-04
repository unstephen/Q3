public class Protocal
{
    ///BUILD TABLE
    public const int Connect = 101;     //连接服务器
    public const int Exception = 102;     //异常掉线
    public const int Disconnect = 103;     //正常断线   
    public const int LOGIN = 91001;

    public const int LOGIN_Ret = 19001;
    /// <summary>
    /// 创建房间请求，只有房卡场
    /// </summary>
    public const int CreateFKRoom = 91002;

    /// <summary>
    /// 创建房间返回
    /// </summary>
    public const int CreateFKRoom_Ret = 19002;


    /// <summary>
    /// 加入房间,重连都走这条
    /// </summary>
    public const int GETGAMEADRESS = 91003;

    /// <summary>
    /// 加入房间返回
    /// </summary>
    public const int GETGAMEADRESS_Ret = 19003;

    /// <summary>
    /// 加入
    /// </summary>
    public const int JoinRoom = 92001;



    /// <summary>
    /// 匹配房间请求
    /// </summary>
    public const int RoomMatch = 91004;

    /// <summary>
    /// 匹配房间返回
    /// </summary>
    public const int RoomMatch_Ret = 19004;

    /// <summary>
    /// 加入金币房间
    /// </summary>
    public const int JOINGOLDROOM = 92001;
    /// <summary>
    /// 广播某个玩家加入房间
    /// </summary>
    public const int JOINEDROOM = 11106;
    /// <summary>
    ///返回初始数据给加入房间的玩家 
    /// </summary>
    public const int SELFJOINROOM = 11107;

    /// <summary>
    ///玩家出牌，其他玩家出牌的监听
    /// </summary>
    public const int PLAYSENDMJ = 11003;
    /// <summary>
    /// 广播该哪个玩家出牌
    /// </summary>
    public const int OUTCARDPLAYER = 11102;


    /// <summary>
    /// 单局结算
    /// </summary>
    public const int SINGLERESULT = 11108;

    /// <summary>
    /// 房间信息推送
    /// </summary>
    public const int PLAYERDATA = 121001;

    /// <summary>
    ///推送玩家手牌
    /// </summary>
    public const int PLAYERCARDS = 121101;
    /// <summary>
    /// 游戏开始 12000
    /// </summary>
    public const int ALLREADY = 12000;
    /// <summary>
    /// 广播玩家开始抢牌
    /// </summary>
    public const int GRABPLAY_Broadcast = 121102;

    /// <summary>
    /// 广播玩家抢牌
    /// </summary>
    public const int GRABRESULT_Broadcast = 121103;

    /// <summary>
    /// 玩家抢牌请求
    /// </summary>
    public const int GRABRESULT = 120103;



    /// <summary>
    /// 确定地主
    /// </summary>
    public const int GRABFINALRESULT = 122101;
    /// <summary>
    /// 可以开始加倍 12105
    /// </summary>
    public const int STARTBET = 12105;

    /// <summary>
    /// 通知玩家出牌
    /// </summary>
    public const int DOPLAYER = 121104;
    /// <summary>
    /// 玩家出牌
    /// </summary>
    public const int OUTCARD = 120105;

    /// <summary>
    /// 玩家出牌广播
    /// </summary>
    public const int OUTCARD_Broadcast = 121105;

    /// <summary>
    /// 出牌 12003
    /// </summary>
    public const int REMIND = 12003;
    /// <summary>
    /// 玩家过牌
    /// </summary>
    public const int PASS = 120106;

    /// <summary>
    /// 玩家过牌广播
    /// </summary>
    public const int PASS_Broadcast = 121106;

    /// <summary>
    /// 出牌 12007
    /// </summary>
    public const int DOUBLECARD = 12107;
    /// <summary>
    /// 回合结算
    /// </summary>
    public const int RESULT = 122102;

    public const int NEXT = 11011;
    /// <summary>
    /// 其他玩家加入房间推送
    /// </summary>
    public const int PLAYERINFO = 121002;
    /// <summary>
    /// 准备
    /// </summary>
    public const int PLAYERREADY = 120009;

    /// <summary>
    /// 玩家准备状态广播
    /// </summary>
    public const int PLAYERREADY_Broadcast = 121009;

    /// <summary>
    /// 记录 91204
    /// </summary>
    public const int RECORD = 19204;

    /// <summary>
    /// 请求记录
    /// </summary>
    public const int GetRECORD = 91204;

    /// <summary>
    /// 商店 
    /// </summary>
    public const int SHOP = 91203;

    /// <summary>
    /// 商店 返回
    /// </summary>
    public const int SHOP_Ret = 19203;

    /// <summary>
    /// 每日签到数据 10018
    /// </summary>
    public const int DAILYINFO = 10018;

    public const int DAILYSIGN = 91209;

    public const int DAILYSIGN_Ret = 19209;

    /// <summary>
    /// 邀请 10021
    /// </summary>
    public const int INVITEINFO = 10021;
    /// <summary>
    /// 邀请 10022
    /// </summary>
    public const int INVITE = 10022;
    /// <summary>
    /// 获取邮件 10023
    /// </summary>
    public const int GETMAILS = 10023;
    /// <summary>
    /// 获取邮件 10024
    /// </summary>
    public const int DELMAILS = 10024;
    /// <summary>
    /// 查看邮件 10025
    /// </summary>
    public const int OPENMAIL = 10025;

    /// <summary>
    /// 玩家发起语音请求
    /// </summary>
    public const int YUNWAYY = 120201;

    /// <summary>
    ///玩家语音广播
    /// </summary>
    public const int YUNWAYY_Broadcast = 121201;

    /// <summary>
    /// 玩家发起默认文字请求
    /// </summary>
    public const int Chat = 120202;

    /// <summary>
    /// 玩家默认文字广播
    /// </summary>
    public const int Chat_Broadcast = 121202;

    /// <summary>
    /// 玩家发起默认表情请求
    /// </summary>
    public const int EX = 120203;

    /// <summary>
    /// 玩家默认b表情广播
    /// </summary>
    public const int EX_Broadcast = 121203;

    /// <summary>
    /// 玩家离开房间请求
    /// </summary>
    public const int QUITROOM = 120004;

    /// <summary>
    /// 玩家离开房间广播（开始前） 说明：如果是自己　要主动退出房间　服务器会同时断开连接
    /// 30秒不准备或者钱不够或者钱达到上限会踢
    /// </summary>
    public const int PLAYEROUT = 121004;

    /// <summary>
    /// 房间解散广播
    /// </summary>
    public const int DISMISSROOM_Ret = 122001;
    
    /// <summary>
    /// 服务器踢出 
    /// </summary>
    public const int PLAYERKICKOUT = 10038;

    /// <summary>
    /// 发起投票 
    /// </summary>
    public const int STARTVOTE = 120006;

    /// <summary>
    /// 房主请求解散房间（开始前） 
    /// </summary>
    public const int DISMISSROOM = 120005;

    /// <summary>
    /// 玩家解散房间请求广播
    /// </summary>
    public const int DisMissRoom_Broadcast = 121006;

    /// <summary>
    /// 玩家解散房间投票请求
    /// </summary>
    public const int VotetoDissolve_send = 120007;

    /// <summary>
    /// 玩家解散房间广播投票
    /// </summary>
    public const int VoteToDissmissResult_Broadcast = 121007;

    /// <summary>
    /// 房间解散广播结果
    /// </summary>
    public const int OutRoom_Broadcast = 121008;

    /// <summary>
    /// 投票结果 
    /// </summary>
    public const int VOTERESULT = 10015;
    /// <summary>
    /// 消耗金币 
    /// </summary>
    public const int COSTCOIN = 11116;
    /// <summary>
    /// 表情 
    /// </summary>
    public const int EXPRESSION = 120203;

    /// <summary>
    /// 玩家默认表情广播 
    /// </summary>
    public const int EXPRESSION_Broadcast = 121203;

    /// <summary>
    /// 重连后发送游戏服地址，roomid等 ,开始前的重连
    /// </summary>
    public const int GAMERECONNECT = 10059;
    /// <summary>
    /// 后台切回来后的重连
    /// </summary>
    public const int INGAMERECONNECT = 11051;
    /// <summary>
    /// 重连,这里指的是不重新走登陆流程的重连
    /// </summary>
    public const int RECONNECT = 120903;

    /// <summary>
    /// 玩家重连
    /// </summary>
    public const int PlayerReconnect_Ret = 121903;

    /// <summary>
    /// 斗地主重连的数据
    /// </summary>
    public const int RECONNECTDATA = 121904;
    /// <summary>
    /// 跑马灯信息
    /// </summary>
    public const int SCROLLINFO = 19202;

    /// <summary>
    /// 玩家加入房间返回
    /// </summary>
    public const int JOINROOMFAILED = 29001;

    /// <summary>
    /// 请求重连
    /// </summary>
    public const int MAINRECONNECT = 91901;
    /// <summary>
    /// 返回大厅
    /// </summary>
    public const int MAINRECONNECTRESPONSE = 19901;
    /// <summary>
    /// 请求托管
    /// </summary>
    public const int TUOGUAN = 11009;
    /// <summary>
    /// 购买
    /// </summary>
    public const int APPBUY = 10027;
    /// <summary>
    /// 玩家房卡数量推送
    /// </summary>
    public const int BUYFKRESULT = 19101;
    /// <summary>
    /// 玩家金币数量推送
    /// </summary>
    public const int BUYGOLDRESULT = 19102;
    /// <summary>
    /// 断线
    /// </summary>
    public const int OFFLINE = 121902;
    /// <summary>
    /// 重连回来
    /// </summary>
    public const int ONLINE = 121901;
    /// <summary>
    /// 总结算
    /// </summary>
    public const int TotalResult = 122103;

    /// <summary>
    /// 获取版本信息
    /// </summary>
    public const int GETVERSION = 91201;

    /// <summary>
    /// 获取版本信息返回
    /// </summary>
    public const int GetVersion_Ret = 19201;

    /// <summary>
    /// 兑换
    /// </summary>
    public const int EXCHANGE = 10041;

    /// <summary>
    /// 获取任务信息
    /// </summary>
    public const int GetTaskInfo = 91207;

    /// <summary>
    /// 获取任务信息返回
    /// </summary>
    public const int GetTaskInfo_Ret = 19207;

    /// <summary>
    /// 领取任务奖励
    /// </summary>
    public const int CashTaskPrize = 91208;

    /// <summary>
    /// 领取任务奖励返回
    /// </summary>
    public const int CashTaskPrize_Ret = 19208;

    /// <summary>
    /// 获取转盘信息请求
    /// </summary>
    public const int GetLuckyInfo = 91205;

    /// <summary>
    /// 获取转盘信息返回
    /// </summary>
    public const int GetLuckyInfo_Ret = 19205;

    /// <summary>
    /// 转盘抽奖请求
    /// </summary>
    public const int LuckyDrawResponse = 91206;

    /// <summary>
    /// 转盘抽奖返回
    /// </summary>
    public const int LuckyDraw_Ret = 19206;

    /// <summary>
    /// 修改个性签名
    /// </summary>
    public const int RewritePersonSign = 91210;

    /// <summary>
    /// 修改个性签名
    /// </summary>
    public const int RewritePersonSign_Ret = 19210;

    /// <summary>
    /// 玩家申请支付请求
    /// </summary>
    public const int PayRequest = 91301;

    /// <summary>
    /// 玩家支付返回
    /// </summary>
    public const int PayRequest_Ret = 19301;

    /// <summary>
    /// 玩家查询订单状态
    /// </summary>
    public const int CheckPay = 91303;

    /// <summary>
    /// 玩家查询订单返回
    /// </summary>
    public const int CheckPay_Ret = 19303;
}