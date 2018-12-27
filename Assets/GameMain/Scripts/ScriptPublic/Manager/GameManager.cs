using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoSingleton<GameManager>
{
    RoleData roleData;
    Send_MsgBase sendBaseData;

    public List<GoodsData> goodsList;

    public void InitRoleData(string id, string token)
    {
        GetRoleData().InitData(id, token);

        if (sendBaseData == null)
        {
            sendBaseData = new Send_MsgBase();
        }
    }

    public RoleData GetRoleData()
    {
        if (roleData == null)
        {
            roleData = new RoleData();
        }

        return roleData;
    }
    
    public bool IsSelf(int pid)
    {
        return roleData.pId.Value == pid;
    }

    public List<string> GetSendInfoStringList<T>(params object[] args) where T : Send_MsgBase, new()
    {
        var temp = new T();

        return temp.CreateSendInfo(args);
    }
    
    public void SetGoosList()
    {
        if (goodsList == null)
        {
            goodsList = new List<GoodsData>();
        }
        else
        {
            goodsList.Clear();
        }
    }

    #region Network
       /// <summary>
    /// 链接错误
    /// </summary>
    public int errorStateCode=0;
    /// <summary>
    /// 处理返回消息,更新数据信息
    /// </summary>
    /// <param name="code"></param>
    /// <param name="json"></param>
    public void ParseMessage(int code, string json)
    {

        
//        switch (code)
//        {
//            case Protocal.JOINROOMFAILED:
//                MsgBase error = LitJson.JsonMapper.ToObject<MsgBase>(json);
//                errorStateCode = error.state;
//               break;
//            case Protocal.LOGIN_Ret:
//                loginMsg = JsonMapper.ToObject<S2C_LoginMsg>(json);
//                break;
//            case Protocal.CreateFKRoom_Ret:
//                roomMsg = JsonMapper.ToObject<S2C_CreateRoom>(json);
//                break;
//            case Protocal.GETGAMEADRESS_Ret:
//                gameAdress = JsonMapper.ToObject<S2C_GetGameAdress>(json);
//
//                break;
//            case Protocal.RoomMatch_Ret:
//                gameAdress = JsonMapper.ToObject<S2C_GetGameAdress>(json);
//                if (gameAdress.state == 0)
//                {
//                    AppConst.GameSocketAddress = gameAdress.gsAddr;
//                    gameAdress.mode = "JB";
//                        gameAdress.gameType = 2;
//                    //gameMode=GameEnum.GameMode.goldMj;
//                }
//                break;
//            case Protocal.JOINGOLDROOM:
//                gameAdress = JsonMapper.ToObject<S2C_GetGameAdress>(json);
//                if (gameAdress.state == 0)
//                {
//                    AppConst.GameSocketAddress = gameAdress.gsAddr;
//                }
//                break;
//
//            case Protocal.SELFJOINROOM:
//              
//                break;
//
//            case Protocal.OUTCARDPLAYER:
//                curOutMjPlayer = JsonMapper.ToObject<S2C_OutCardPlayer>(json);
//                curOutPlayer=curOutMjPlayer.outCardPlayer;
//                break;
//            case Protocal.PLAYERDATA:
//               
//                playerData = JsonMapper.ToObject<S2C_PlayerData>(json);
//                foreach (PlayerInfo player in playerData.players)
//                {
//                    GamePlayer gamePlayer = new GamePlayer();
//                    gamePlayer.headIcon = player.headPic;
//                    gamePlayer.index = player.index;
//                    gamePlayer.nickName = player.nickName;
//                    gamePlayer.uid = player.uid;
//                    gamePlayer.sex = player.sex;
//                    gamePlayers[player.index] = gamePlayer;
//                }
//                break;
//            case Protocal.PLAYERCARDS:
//                playerCards = JsonMapper.ToObject<S2C_PlayerCards>(json);
//                break;
//            case Protocal.GRABPLAY_Broadcast:
//                grabPlay = JsonMapper.ToObject<S2C_GRABPLAY>(json);
//                break;
//            case Protocal.GRABRESULT_Broadcast:
//                grabResult = JsonMapper.ToObject<S2C_GRABPLAYRESULT>(json);
//                break;
//            case Protocal.GRABFINALRESULT:
//                grabFinalResult = JsonMapper.ToObject<S2C_GRABFINALRESULT>(json);
//                break;
//            case Protocal.DOPLAYER:
//                doPlayer = JsonMapper.ToObject<S2C_DoPlayer>(json);
//                break;
//            case Protocal.OUTCARD_Broadcast:
//                OutCards = JsonMapper.ToObject<S2C_OutCard>(json);
//                break;
//            case Protocal.REMIND:
//                remind = JsonMapper.ToObject<S2C_OutCard>(json);
//                break;
//            case Protocal.PASS_Broadcast:
//                pass = JsonMapper.ToObject<S2C_Pass>(json);
//                break;
//            case Protocal.DOUBLECARD:
//                doubleCard = JsonMapper.ToObject<S2C_DoubleCard>(json);
//                break;
//            case Protocal.RESULT:
//                this.result = JsonMapper.ToObject<S2C_Result>(json);
//                loginMsg.marks.roomId = 0;
//                reconnectData = null;
//                break;
//            case Protocal.NEXT:
//                nextRound = JsonMapper.ToObject<S2C_NEXT>(json);
//                break;
//            case Protocal.PLAYERINFO:
//                playerInfo = JsonMapper.ToObject<S2C_PlayerInfo>(json);
//                GamePlayer gamelayer = new GamePlayer();
//                gamelayer.nickName = playerInfo.nickName;
//                gamelayer.index = playerInfo.index;
//                gamelayer.headIcon = playerInfo.headPic;
//                gamelayer.uid = playerInfo.uid;
//                gamelayer.sex = playerInfo.sex;
//                gamePlayers[playerInfo.index] = gamelayer;
//               
//                break;
//            case Protocal.PLAYERREADY_Broadcast:
//                break;
//            case Protocal.RECORD:
//                record = JsonMapper.ToObject<Record>(json);
//                break;
//            case Protocal.SHOP_Ret:
//                shop = JsonMapper.ToObject<Shop>(json);
//                break;
//            case Protocal.DAILYSIGN_Ret:
//                curSignItem = JsonMapper.ToObject<S2C_Sign>(json);
//                luckyInfo.signDay = curSignItem.signDay;
//                loginMsg.status.RefreshCoin( curSignItem.coin );
//                loginMsg.status.RefreshCard( curSignItem.card );
//                break;
//            case Protocal.INVITEINFO:
//                inviteInfo = JsonMapper.ToObject<S2C_InvitInfo>(json);
//                break;
//            case Protocal.INVITE:
//                inviteResult = JsonMapper.ToObject<MsgBase>(json);
//                break;
//            case Protocal.GETMAILS:
//                mail = JsonMapper.ToObject<S2C_Mail>(json);
//                break;
//            case Protocal.DELMAILS:
//                delMail = JsonMapper.ToObject<S2C_DelMail>(json);
//                mail.mails = delMail.mails;
//                break;
//            case Protocal.YUNWAYY_Broadcast:
//                yy = JsonMapper.ToObject<S2C_YY>(json);
//                break;
//            case Protocal.VoteToDissmissResult_Broadcast:
//                playerVoterResult = JsonMapper.ToObject<PlayerVoteResult>(json);
//                break;
//            case Protocal.OutRoom_Broadcast:
//                voteResult = JsonMapper.ToObject<S2C_VoteResult>( json );
//                break;
//            case Protocal.JOINEDROOM:
//                otherPlayer = JsonMapper.ToObject<S2C_PlayerJoinRoom>(json);
//                 GamePlayer temp = new GamePlayer();
//                temp.index = otherPlayer.playerIndex;
//                temp.headIcon = otherPlayer.playerInfo.headPic;
//                temp.nickName = otherPlayer.playerInfo.nickName;
//                temp.uid = otherPlayer.playerInfo.uid;
//                temp.sex = otherPlayer.playerInfo.sex;
//                gamePlayers[temp.index] = temp;
//                otherPlayers[otherPlayer.playerIndex]=otherPlayer;
//                //GameData.instance.roomInitInfo.players.Add(otherPlayer.playerInfo);
//                break;
//            case Protocal.EXPRESSION_Broadcast:
//                expression = JsonMapper.ToObject<S2C_Expression>(json);
//                break;
//            case Protocal.GAMERECONNECT:
//                gameAdress = JsonMapper.ToObject<S2C_GetGameAdress>(json);
//                AppConst.GameSocketAddress = gameAdress.gsAddr;
//                break;
//            case Protocal.RECONNECTDATA:
//                reconnectData = JsonMapper.ToObject<ReconnectData>(json);
//                //-------------
//                playerData = new S2C_PlayerData( );
//                playerData.allRound = reconnectData.allRound;
//                playerData.curRound = reconnectData.curRound;
//                playerData.creatorId = reconnectData.creatorId;
//                playerData.dealer = reconnectData.dealer;
//                playerData.maxLimit = reconnectData.maxLimit;
//                playerData.mode = reconnectData.mode;
//                playerData.payMode = reconnectData.payMode;
//                playerData.playerIndex = reconnectData.playerIndex;
//                playerData.players = new List<PlayerInfo>( reconnectData.players.Count );
//                for( int i = 0; i < reconnectData.players.Count; i++ ) {
//                    playerData.players.Add( reconnectData.players[i] );
//                }
//                foreach( PlayerInfo player in playerData.players ) {
//                    GamePlayer gamePlayer = new GamePlayer( );
//                    gamePlayer.headIcon = player.headPic;
//                    gamePlayer.index = player.index;
//                    gamePlayer.nickName = player.nickName;
//                    gamePlayer.uid = player.uid;
//                    gamePlayer.sex = player.sex;
//                    gamePlayers[player.index] = gamePlayer;
//                }
//            //---------------
//                grabFinalResult = new S2C_GRABFINALRESULT( );
//                grabFinalResult.farmer = reconnectData.dealer;
//                grabFinalResult.bCards = reconnectData.bCards;
//                break;
//            case Protocal.SCROLLINFO:
//                scrollInfo = JsonMapper.ToObject<ScrollInfo>(json);
//                break;
//            case Protocal.DisMissRoom_Broadcast:
//                startVote = JsonMapper.ToObject<StartVote>( json );
//                break;
//             case Protocal.VOTERESULT:
//                voteResult=JsonMapper.ToObject<S2C_VoteResult>(json);
//                if (voteResult.isSuccess)
//                {
//                    loginMsg.marks.roomId = 0;
//                }
//                break;   
//            case Protocal.MAINRECONNECTRESPONSE:
//            case Protocal.INGAMERECONNECT:
//                reconnectResponse=JsonMapper.ToObject<ReconnectResponse>(json);
//                loginMsg.status.RefreshCoin( reconnectResponse.coin);
//                loginMsg.status.RefreshCard( reconnectResponse.card);
//            break;
//
//            case Protocal.PLAYEROUT:
//                playerOut=JsonMapper.ToObject<S2C_PlayerOut>(json);
//            break;
//
//            case Protocal.TUOGUAN:
//                tgInfo=JsonMapper.ToObject<S2C_TG>(json);
//            break;
//
//            case Protocal.TotalResult:
//                totalResult = JsonMapper.ToObject<TotalResult_ddz>(json);
//                //gamePlayers = null;
//                break;
//            case Protocal.BUYGOLDRESULT:
//               // Debug.LogError("gold");
//                S2C_GoldChange result = JsonMapper.ToObject<S2C_GoldChange>(json);
//                GameData.Inst.loginMsg.status.RefreshCoin( result.coin);
//                if (GameData.Inst.loginMsg.status.coin < 0)
//                {
//                    GameData.Inst.loginMsg.status.RefreshCoin( 0);
//                }
//                break;
//            case Protocal.BUYFKRESULT:
//               // Debug.LogError("fk");
//                S2C_BuyFKResult fkresult = JsonMapper.ToObject<S2C_BuyFKResult>(json);
//                GameData.Inst.loginMsg.status.RefreshCard( fkresult.card);
//                break;
//            case Protocal.GetTaskInfo_Ret:
//                //获取任务信息返回
//                getTaskInfo = JsonMapper.ToObject<S2C_GetTaskInfo>( json );
//                break;
//            case Protocal.CashTaskPrize_Ret:
//                //领取任务奖励返回
//                cashTaskPrize = JsonMapper.ToObject<S2C_CashTaskPrize>( json );
//                loginMsg.status.RefreshCoin( cashTaskPrize.coin);
//                break;
//            case Protocal.GetLuckyInfo_Ret:
//                luckyInfo = JsonMapper.ToObject<S2C_LuckyInfo>(json);
// 
//                break;
//            case Protocal.LuckyDraw_Ret:
//                luckyDrawRet = JsonMapper.ToObject<S2C_LuckyDraw_Ret>(json);
//                loginMsg.status.RefreshCoin( luckyDrawRet.coin);
//                break;
//            case Protocal.RewritePersonSign_Ret:
//                rewitePersonSign = JsonMapper.ToObject<S2C_RewritePersonSign>(json);
//                loginMsg.info.RefreshNickName(rewitePersonSign.notice);
//                break;
//            case Protocal.PayRequest_Ret:
//                payRequest = JsonMapper.ToObject<S2C_PayRequest>(json);
//                break;
//            case Protocal.CheckPay_Ret:
//                checkPay = JsonMapper.ToObject<S2C_CheckPay>(json);
//                break;
//            default:
//            break;
//
//        }

    }
    #endregion
}