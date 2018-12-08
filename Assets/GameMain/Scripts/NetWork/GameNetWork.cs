using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameNetWork
{


    private static GameNetWork _instance;
    public static GameNetWork instance
    {

        get
        {
            if (_instance == null)
            {
                _instance = new GameNetWork();
            }
            return _instance;
        }
    }

    public NetWorkManager netWorkManager
    {

        get { return NetWorkManager.Instance; }
    }


//    /// <summary>
//    /// 加入房间消息
//    /// </summary>
//    private C2S_GetGameAdress gameAddress;
//    /// <summary>
//    /// 请求游戏服地址
//    /// </summary>
//    public void GetGameAddress(int roomId = 0)
//    {
//        gameAddress = new C2S_GetGameAdress();
//        gameAddress.uid = GameData.Inst.loginMsg.uid;
//        if (roomId == 0)
//        {
//            gameAddress.roomId = GameData.Inst.roomMsg.roomId;
//
//        }
//        else
//        {
//            gameAddress.roomId = roomId;
//        }
//        gameAddress.coinNum = GameData.Inst.loginMsg.status.coin;
//        Packet packet = new Packet( Protocal.GETGAMEADRESS , gameAddress );
//        netWorkManager.EmitPacket( packet );
//    }

//    /// <summary>
//    /// 加入房间
//    /// </summary>
//    private C2S_JoinRoom joinMsg;
//
//
//    /// <summary>
//    /// 加入房间
//    /// </summary>
//    public void JoinRoom(int gametype)
//    {
//        JoinRoom( gametype, Protocal.JoinRoom );
//    }
//
//    private void JoinRoom(int gametype, int protocal ) {
//        joinMsg = new C2S_JoinRoom( );
//        joinMsg.uid = GameData.Inst.loginMsg.uid;
//        joinMsg.nickName = GameData.Inst.loginMsg.info.name;
//        joinMsg.roomId = GameData.Inst.gameAdress.roomId;
//        joinMsg.coin = GameData.Inst.loginMsg.status.coin;
//        if( GameData.Inst.wxUserInfo != null ) {
//            joinMsg.headPic = GameData.Inst.wxUserInfo.headimgurl;
//            joinMsg.sex = GameData.Inst.wxUserInfo.sex;
//        }
//        joinMsg.notice = GameData.Inst.loginMsg.info.notice;
//        Packet packet = new Packet(protocal, joinMsg );
//        netWorkManager.EmitPacket( packet, AppConst.GameSocketAddress );
//    }
//
//    public void JoinGoldRoom(int gametype)
//    {
//        JoinRoom( gametype, Protocal.JOINGOLDROOM );
//    }
//
//    private C2S_GETGOLDGAMEADDRESS joinGoldRoom;
//    /// <summary>
//    /// 加入金币房间
//    /// </summary>
//    public void GetGoldGameAddress(int scale)
//    {
//        joinGoldRoom = new C2S_GETGOLDGAMEADDRESS();
//        //Debug.LogError(GameData.instance.gameMode);
//        joinGoldRoom.gameType = 2;
//        joinGoldRoom.scale = scale;
//        joinGoldRoom.uid = GameData.Inst.loginMsg.uid;
//        Packet packet = new Packet( Protocal.RoomMatch , joinGoldRoom );
//        netWorkManager.EmitPacket( packet );
//
//    }
//
//    public void DdzReady(C2S_READY msg)
//    {
//        Packet packet = new Packet( Protocal.PLAYERREADY , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void SelectCharacter(int vidIdx)
//    {
//      
//
//    }
//
//    //ddz开始游戏
//    public void DDZFinishReady()
//    {
//        // Debug.Log("Ready");
//        C2S_STARTMJ msg = new C2S_STARTMJ();
//        msg.uid = GameData.Inst.playerUid;
//        if (GameData.Inst.gameMode == GameMode.Gold)
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        else
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        Packet packet = new Packet( Protocal.ALLREADY , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    //抢地主
//    public void Grab(int score)
//    {
//        C2S_GRABPLAY msg = new C2S_GRABPLAY();
//        msg.uid = GameData.Inst.playerUid;
//        if (GameData.Inst.gameMode == GameMode.Gold)
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        else
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        msg.score = score;
//        Packet packet = new Packet( Protocal.GRABRESULT , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//
//    public void OutCards(CaseInfo caseInfo)
//    {
//        C2S_OutCard msg = new C2S_OutCard();
//        msg.uid = GameData.Inst.playerUid;
//        if (GameData.Inst.gameMode == GameMode.Gold)
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        else
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        // msg.cards=cards;
//        msg.caseInfo = caseInfo;
//        Packet packet = new Packet( Protocal.OUTCARD , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    //提醒
//    public void DdzRemind()
//    {
//        C2S_STARTMJ msg = new C2S_STARTMJ();
//        msg.uid = GameData.Inst.playerUid;
//        if (GameData.Inst.gameMode == GameMode.Gold)
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        else
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        Packet packet = new Packet( Protocal.REMIND , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    //过牌
//    public void Pass()
//    {
//        C2S_STARTMJ msg = new C2S_STARTMJ();
//        msg.uid = GameData.Inst.playerUid;
//        if (GameData.Inst.gameMode == GameMode.Gold)
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        else
//        {
//            msg.roomId = GameData.Inst.gameAdress.roomId;
//        }
//        Packet packet = new Packet( Protocal.PASS , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void Next()
//    {
//
//        C2S_STARTMJ msg = new C2S_STARTMJ();
//        msg.uid = GameData.Inst.playerUid;
//        msg.roomId = GameData.Inst.roomId;
//        Packet packet = new Packet( Protocal.NEXT , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//
//    }
//
//    public void GetRecord()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.GetRECORD , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    public void GetShop()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.SHOP , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void GetDailySignInfo()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.DAILYINFO , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    public void GetLuckyInfo()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.GetLuckyInfo , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    public void LuckyDrawResponse()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.LuckyDrawResponse , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void Sign(int day)
//    {
//        C2S_Sign msg = new C2S_Sign();
//        msg.uid = GameData.Inst.playerUid;
//        msg.day = day;
//        Packet packet = new Packet( Protocal.DAILYSIGN , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void GetInviteInfo()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.INVITEINFO , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void Invite(int inviteid)
//    {
//        C2S_Invite msg = new C2S_Invite();
//        msg.uid = GameData.Inst.playerUid;
//        msg.invUid = inviteid;
//        Packet packet = new Packet( Protocal.INVITE , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void GetMail()
//    {
//        C2S_RECORD msg = new C2S_RECORD();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.GETMAILS , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void DelMail(List<int> ids)
//    {
//        C2S_DelMail msg = new C2S_DelMail();
//        msg.uid = GameData.Inst.playerUid;
//        msg.ids = ids;
//        Packet packet = new Packet( Protocal.DELMAILS , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void OpenMail(int id)
//    {
//        C2S_OpenMail msg = new C2S_OpenMail();
//        msg.uid = GameData.Inst.playerUid;
//        msg.id = id;
//        Packet packet = new Packet( Protocal.OPENMAIL , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    /// <summary>
//    /// 语音聊天
//    /// </summary>
//    /// <param name="url"></param>
//    /// <param name="path"></param>
//    public void YunwaUp(string url , string path)
//    {
//        C2S_YY msg = new C2S_YY();
//        msg.uid = GameData.Inst.playerUid;
//        msg.roomId = GameData.Inst.roomId;
//        msg.gameType = 2;
//        msg.url = url;
//        msg.path = path;
//        //msg.time= DateTime.Now.ToFileTime().ToString();
//        Packet packet = new Packet( Protocal.YUNWAYY , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    /// <summary>
//    /// 发送文字
//    /// </summary>
//    /// <param name="msg">信息</param>
//   public void Chat(string msg)
//    {
//        C2S_Chat chat = new C2S_Chat();
//        chat.uid = GameData.Inst.playerUid;
//        chat.roomId = GameData.Inst.roomId;
//        chat.str = msg;
//        Packet packet = new Packet(Protocal.Chat,chat);
//        netWorkManager.EmitPacket(packet,AppConst.GameSocketAddress);
//    }
//    /// <summary>
//    /// 发送表情
//    /// </summary>
//    /// <param name="msg"></param>
//    public void EX(string msg)
//    {
//        C2S_EX chat = new C2S_EX();
//        chat.uid = GameData.Inst.playerUid;
//        chat.roomId = GameData.Inst.roomId;
//        chat.str = msg;
//        Packet packet = new Packet(Protocal.EX, chat);
//        netWorkManager.EmitPacket(packet, AppConst.GameSocketAddress);
//    }
//    /// <summary>
//    /// 房主解散房间
//    /// </summary>
//    public void DisMissRoom()
//    {
//        LeaveRoom( Protocal.DISMISSROOM );
//    }
//    /// <summary>
//    /// 退出房间
//    /// </summary>
//    public void QuitRoom()
//    {
//        LeaveRoom( Protocal.QUITROOM );
//    }
//    /// <summary>
//    /// 离开房间
//    /// </summary>
//    /// <param name="protocal">协议号</param>
//    private void LeaveRoom(int protocal)
//    {
//        DissolveRoom msg = new DissolveRoom();
//        msg.roomId = GameData.Inst.roomId;
//        msg.uid = GameData.Inst.playerUid;
//        //msg.gameType=GameData.instance.gameType;
//        Packet packet = new Packet( protocal , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//
//
//    /// <summary>
//    /// 发起投票
//    /// </summary>
//    public void StartVote()
//    {
//        DissolveRoom msg = new DissolveRoom();
//        msg.roomId = GameData.Inst.roomId;
//        msg.uid = GameData.Inst.playerUid;
//        //msg.gameType=GameData.instance.gameType;
//        Packet packet = new Packet( Protocal.STARTVOTE , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    /// <summary>
//    /// 玩家解散房间投票
//    /// </summary>
//    /// <param name="agree">是否同意</param>
//    public void Vote(bool agree)
//    {
//        VoteToDismiss msg = new VoteToDismiss();
//        msg.roomId = GameData.Inst.roomId;
//        msg.uid = GameData.Inst.playerUid;
//        //msg.gameType=GameData.instance.gameType;
//        msg.isAgree = agree;
//        Packet packet = new Packet( Protocal.VotetoDissolve_send , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void SendExpression(string str)
//    {
//        Expression msg = new Expression();
//        msg.uid = GameData.Inst.playerUid;
//        msg.roomId = GameData.Inst.roomId;
//        msg.str = str;
//        //   msg.gameType=GameData.instance.gameType;
//        Packet packet = new Packet( Protocal.EXPRESSION , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void FocusOut()
//    {
//        OnFocusOut msg = new OnFocusOut();
//        msg.roomId = GameData.Inst.roomId;
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.INGAMERECONNECT , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void DDZFocusOut()
//    {
//        OnFocusOut msg = new OnFocusOut();
//        msg.roomId = GameData.Inst.roomId;
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.RECONNECT , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//
//    public void MainReconnect()
//    {
//        C2S_MainReconnect msg = new C2S_MainReconnect();
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.MAINRECONNECT , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void TuoGuan(bool ist)
//    {
//        C2S_TG msg = new C2S_TG();
//        msg.uid = GameData.Inst.playerUid;
//        msg.isT = ist;
//        msg.roomId = GameData.Inst.roomId;
//        Packet packet = new Packet( Protocal.TUOGUAN , msg );
//        netWorkManager.EmitPacket( packet , AppConst.GameSocketAddress );
//    }
//    //内购
//    public void InappPurchase(int id)
//    {
//        C2S_Buy msg = new C2S_Buy();
//        msg.shopId = id;
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.APPBUY , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void GetVersion()
//    {
//        // MsgBase msg = new MsgBase;
//        Packet packet = new Packet( Protocal.GETVERSION , null );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void Exchange(int shopid , string phone)
//    {
//        Exchage msg = new Exchage();
//        msg.shopId = shopid;
//        msg.phone = phone;
//        msg.uid = GameData.Inst.playerUid;
//        Packet packet = new Packet( Protocal.EXCHANGE , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    /// <summary>
//    /// 获取任务信息
//    /// </summary>
//    /// <param name="uid">玩家uid</param>
//    public void GetTaskInfo(int uid)
//    {
//        C2S_GetTaskInfo msg = new C2S_GetTaskInfo();
//        msg.uid = uid;
//        Packet packet = new Packet( Protocal.GetTaskInfo , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//    /// <summary>
//    /// 领取任务奖励
//    /// </summary>
//    /// <param name="uid">玩家uid</param>
//    /// <param name="taskid">任务id</param>
//    public void CashTaskPrize(int uid , int taskId)
//    {
//        C2S_CashTaskPrize msg = new C2S_CashTaskPrize();
//        msg.uid = uid;
//        msg.taskId = taskId;
//        Packet packet = new Packet( Protocal.CashTaskPrize , msg );
//        netWorkManager.EmitPacket( packet , AppConst.SocketAddress );
//    }
//
//    public void RewritePersonSign(string notice)
//    {
//        C2S_RewritePersonSign msg = new C2S_RewritePersonSign();
//        msg.uid = GameData.Inst.playerUid;
//        msg.notice = notice;
//        Packet packet = new Packet(Protocal.RewritePersonSign, msg);
//        netWorkManager.EmitPacket(packet, AppConst.SocketAddress);
//    }
//
//    public void PayRequest(string shopId,string tradeType)
//    {
//        C2S_PayRequest msg = new C2S_PayRequest();
//        msg.uid = GameData.Inst.playerUid;
//        msg.shopId = shopId;
//        msg.tradeType = tradeType;
//        Packet packet = new Packet(Protocal.PayRequest,msg);
//        netWorkManager.EmitPacket(packet, AppConst.SocketAddress);
//    }
//
//    public void CheckPay(string tradeNo)
//    {
//        C2S_CheckPay msg = new C2S_CheckPay();
//        msg.tradeNo = tradeNo;
//        Packet packet = new Packet(Protocal.CheckPay, msg);
//        netWorkManager.EmitPacket(packet, AppConst.SocketAddress);
//    }
}
