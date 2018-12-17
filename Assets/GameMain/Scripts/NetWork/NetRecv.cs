using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using GameFramework.Network;
using UnityGameFramework.Runtime;

public class RecvHandler : IPacketHandler
{
    public void Handle(object sender, GameFramework.Network.Packet packet)
    {
        var basePacket = (BasePacket)packet;
        var args = basePacket.args;
        var msgId = MsgParse.PopByte(ref args);
        switch (msgId)
        {
            case Protocal.Error:
                RecvError(args);
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
            default:
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
    }
    
    private void RecvNotifyCall(byte[] args)
    {
        
    }

    private void RecvNotifyLeave(byte[] args)
    {
        
    }

    private void RecvNotifyChat(byte[] args)
    {
        
    }

    private void RecvNotifyStart(byte[] args)
    {
        
    }

    private void RecvNotifyEnd(byte[] args)
    {
        
    }

    private void RecvNotifyWin(byte[] args)
    {
        
    }

    private void RecvNotifyBet(byte[] args)
    {
       
    }

    private void RecvNotifyRaise(byte[] args)
    {
       
    }

    private void RecvNotifyState(byte[] args)
    {
        
    }

    private void RecvNotifyStage(byte[] args)
    {
        
    }

    private void RecvNotifyButton(byte[] args)
    {
        
    }

    private void RecvBalance(byte[] args)
    {
        
    }

    private void RecvNotifySB(byte[] args)
    {
        
    }

    private void RecvNotifyBB(byte[] args)
    {
        
    }

    private void RecvNotifyJoin(byte[] args)
    {
        throw new NotImplementedException();
    }

    private void RecvNotifyShared(byte[] args)
    {
        throw new NotImplementedException();
    }

    private void RecvNotifyPrivate(byte[] args)
    {
        throw new NotImplementedException();
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
    public int Id { get; private set; }

}

