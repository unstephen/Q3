using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using GameFramework.Network;
using GamePlay;
using UnityGameFramework.Runtime;

public class RecvHandler : IPacketHandler
{
    public void Handle(object sender, GameFramework.Network.Packet packet)
    {
        var basePacket = (BasePacket)packet;
        var args = basePacket.args;
        var msgId = MsgParse.PopByte(ref args);
        if (msgId != 23 && msgId != 38)
        {
            Log.Debug("<<<<<<<<<<<<<<<<<<<<{0}",msgId);
        }
        switch (msgId)
        {
            case Protocal.Error:
                RecvError(args);
                break;
            case Protocal.GOOD:
                RecvGood(args);
                break;
            case Protocal.RecvLogin:
                RecvLogin(args);
                break;
            case Protocal.NOTIFY_DRAW:
                RecvNotifyDraw(args);
                break;
            case Protocal.NOTIFY_PRIVATE:
                RecvNotifyPrivate(args);
                break;
            case Protocal.NOTIFY_SHARED:
                RecvNotifyShared(args);
                break;
            case Protocal.NOTIFY_JOIN:
                RecvNotifyJoin(args);
                break;
            case Protocal.NOTIFY_LEAVE:
                RecvNotifyLeave(args);
                break;
            case Protocal.NOTIFY_CHAT:
                RecvNotifyChat(args);
                break;
            case Protocal.NOTIFY_START:
                RecvNotifyStart(args);
                break;
            case Protocal.NOTIFY_END:
                RecvNotifyEnd(args);
                break;
            case Protocal.NOTIFY_WIN:
                RecvNotifyWin(args);
                break;
            case Protocal.NOTIFY_BET:
                RecvNotifyBet(args);
                break;
            case Protocal.NOTIFY_RAISE:
                RecvNotifyRaise(args);
                break;
            case Protocal.NOTIFY_CALL:
                RecvNotifyCall(args);
                break;
            case Protocal.NOTIFY_STATE:
                RecvNotifyState(args);
                break;
            case Protocal.NOTIFY_STAGE:
                RecvNotifyStage(args);
                break;
            case Protocal.NOTIFY_BUTTON:
                RecvNotifyButton(args);
                break;
            case Protocal.BALANCE:
                RecvBalance(args);
                break;
            case Protocal.NOTIFY_SB:
                RecvNotifySB(args);
                break;
            case Protocal.NOTIFY_BB:
                RecvNotifyBB(args);
                break;
            case Protocal.SEAT_STATE:
                RecvSeatState(args);
                break;
            case Protocal.PLAYER_SUMMARY:
                RecvPlayerSummary(args);
                break;
            default:
                break;
        }
    }

    private void RecvPlayerSummary(byte[] args)
    {
        int pId = MsgParse.PopInt(ref args);
        int score = MsgParse.PopInt(ref args);
        string userId = MsgParse.PopString(ref args);
        string userLoc = MsgParse.PopString(ref args);
        RoomManager.Instance.rData.SetPlayerData(pId,userId,userLoc,score);
    }

    //设置座位状态
    private void RecvSeatState(byte[] args)
    {
        int gid = MsgParse.PopInt(ref args);
        if (gid != RoomManager.Instance.rData.gId.Value)
            return;

        byte pos = MsgParse.PopByte(ref args);
        byte state =  MsgParse.PopByte(ref args);
        int pId = MsgParse.PopInt(ref args);
        Log.Debug("设置座位状态pos={0},pid={1}",pos,pId);
        RoomManager.Instance.rData.roomSeats[pos].pid = pId;
        RoomManager.Instance.rData.roomSeats[pos].state = state;
        RoomManager.Instance.rData.roomSeats[pos].pos = pos;
        if (pId > 0)
        {
            //通过PID获取玩家的基本信息
            NetWorkManager.Instance.Send(Protocal.PLAYER_INFO,pId);
        } 
    }

    private void RecvGood(byte[] args)
    {
        var cmd = MsgParse.PopByte(ref args);
        Log.Debug("Good={0}",cmd);
        switch (cmd)
        {
            case Protocal.WATCH:
            {
                NetWorkManager.Instance.Send(Protocal.SEAT_QUERY,RoomManager.Instance.rData.gId);    
            }
            break;
            case Protocal.LEAVE:
            {
                RoomManager.Instance.Self.Value.state = EPlayerState.Watch;
            }
            break;
            case Protocal.READY_CANCEL:
            {
                RoomManager.Instance.Self.Value.state = EPlayerState.Seat;
            }
            break;
            case Protocal.READY:
            {
                RoomManager.Instance.Self.Value.state = EPlayerState.GamePrepare;
            }
            break;
        }
    }


    //登录处理設置PID
    private void RecvLogin(byte[] args)
    {
        int PID = MsgParse.PopInt(ref args);
        RoleData role = GameManager.Instance.GetRoleData();
        role.pId.Value = PID;
        Log.Debug("login success,pid = {0}",PID);
        NetWorkManager.Instance.Send(Protocal.WATCH,RoomManager.Instance.rData.gId.Value);
        NetWorkManager.Instance.Send(Protocal.SEAT_QUERY,RoomManager.Instance.rData.gId.Value); 
    }
    
    private void RecvNotifyCall(byte[] args)
    {
        
    }

    private void RecvNotifyLeave(byte[] args)
    {
        int gid = MsgParse.PopInt(ref args);
        if (gid != RoomManager.Instance.rData.gId.Value)
            return;
        int pId = MsgParse.PopInt(ref args);
        Player player = GameManager.Instance.IsSelf(pId) ?RoomManager.Instance.Self.Value : RoomManager.Instance.rData.GetPlayer(pId);
        if (player != null)
        {
            player.state = EPlayerState.Watch;
        }
    }

    private void RecvNotifyChat(byte[] args)
    {
        
    }

    private void RecvNotifyStart(byte[] args)
    {
        int gid = MsgParse.PopInt(ref args);
        if (gid != RoomManager.Instance.rData.gId.Value)
            return;
        //所有人状态变为发牌
        foreach (var player in RoomManager.Instance.rData.allPlayers)
        {
            player.state = EPlayerState.Deal;
        }
    }

    private void RecvNotifyEnd(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        
        
        foreach (var player in RoomManager.Instance.rData.allPlayers)
        {
            player.state = EPlayerState.End;
        }
    }

    private void RecvNotifyWin(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        
        int pId = MsgParse.PopInt(ref args);
        int amount = MsgParse.PopInt(ref args);
        var player = RoomManager.Instance.rData.GetPlayer(pId);
        if (player != null)
        {
            player.score.Value += amount;
            player.SetData<VarBool>(Constant.PlayerData.Settle,amount>0);
            player.state = EPlayerState.Settle;
        }
    }

    private void RecvNotifyBet(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        
        int pId = MsgParse.PopInt(ref args);
        int amount = MsgParse.PopInt(ref args);
        var player = RoomManager.Instance.rData.GetPlayer(pId);
        if (player != null)
        {
            player.bet.Value = amount;
        }
    }

    private void RecvNotifyRaise(byte[] args)
    {
       
    }

    private void RecvNotifyState(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        int pId = MsgParse.PopInt(ref args);
        int state = MsgParse.PopByte(ref args);
        var player = RoomManager.Instance.rData.GetPlayer(pId);
        if (player != null && player!=RoomManager.Instance.Self.Value)
        {
            if (state == 2)
            {
                player.state = EPlayerState.GamePrepare;
            }
            else if (state == 3)
            {
                player.state = EPlayerState.Seat;
            }
        }
    }

    private void RecvNotifyStage(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        var stage = MsgParse.PopByte(ref args);
        /*
         * Stage:
               1 -  抢庄
               2 -  下注
               3 -  结束
         */
        foreach (var player in RoomManager.Instance.rData.allPlayers)
        {
            if (stage == 1)
            {
                player.state = EPlayerState.Playing;
            } 
            else if (stage == 2)
            {
                player.state = EPlayerState.Bet;
            }
            else if (stage == 3)
            {
                player.state = EPlayerState.End;
            }
        }
    }

    private void RecvNotifyButton(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        
        byte pos = MsgParse.PopByte(ref args);
        foreach (var player in RoomManager.Instance.rData.allPlayers)
        {
            if (player.pos.Value == pos)
            {
                RoomManager.Instance.rData.bid.Value = player.id.Value;
                return;
            }
        }
    }

    private void RecvBalance(byte[] args)
    {
        int balance = MsgParse.PopInt(ref args);
        int score = MsgParse.PopInt(ref args);
        var self = RoomManager.Instance.rData.playerSelf;
        self.Value.score.Value = score;
        self.Value.balance.Value = balance;
        self.Value.state = EPlayerState.SeatPre;
    }

    private void RecvNotifySB(byte[] args)
    {
        
    }

    private void RecvNotifyBB(byte[] args)
    {
        
    }

    private void RecvNotifyJoin(byte[] args)
    {
        int gId = MsgParse.PopInt(ref args);
        if (gId != RoomManager.Instance.rData.gId.Value)
            return;
        
        int pId = MsgParse.PopInt(ref args);
        
        byte pos = MsgParse.PopByte(ref args);
        int score = MsgParse.PopInt(ref args);
        //通过PID获取玩家的基本信息
        NetWorkManager.Instance.Send(Protocal.PLAYER_INFO,pId);
        if (pId == GameManager.Instance.GetRoleData().pId.Value)
        {
            RoomManager.Instance.Self.Value.SetPos(pos);
            RoomManager.Instance.Self.Value.state = EPlayerState.Seat;
            RoomManager.Instance.Self.Value.score.Value = score;    
        }
        else
        {
            var player = RoomManager.Instance.rData.GetPlayer(pId) as PlayerOther;
            if (player == null)
            {
                player = new PlayerOther();
                player.id.Value = pId;
                player.SetPos(pos);
                player.state = EPlayerState.Seat;
                player.score.Value = score;
                RoomManager.Instance.rData.roomPlayers[pos] = player;
            }
        }
    }

    private void RecvNotifyShared(byte[] args)
    {
       
    }

    private void RecvNotifyPrivate(byte[] args)
    {
       
    }

    private void RecvNotifyDraw(byte[] args)
    {
        
    }

    /// <summary>
    /// 错误返回
    /// </summary>
    private void RecvError(byte[] args)
    {
        //上次发给服务器的消息Id
        int LastSendId = MsgParse.PopByte(ref args);
        int errorId = MsgParse.PopByte(ref args);
        switch (errorId)
        {
            case 0:
                Log.Warning("未知错误");
                break;
            case 1:
                Log.Warning("登录失败，密码有误");
                break;
            case 2:
                Log.Warning("权限不足");
                break;
            case 3:
                Log.Warning("启动游戏失败");
                break;
            default:
                Log.Warning("未知错误");
                break;
        }
    }

    /// <summary>
/// 不用接口的id，廢棄
/// </summary>
    public int Id {
        get { return 1; }
    }

}

