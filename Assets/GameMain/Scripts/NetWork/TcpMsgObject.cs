using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using GameFramework.Network;

//public class BasePacket : GameFramework.Network.Packet
//{
//    public object[] args;
//
//    public override void Clear()
//    {
//       
//    }
//
//    public abstract int Id
//    {
//        get { return 0; }
//    }
//
//    public void SetArgs(params object[] argList)
//    {
//        args = argList;
//    }
//}



public abstract class BasePacket : GameFramework.Network.Packet
{
    public byte[] args;
    public void SetArgs(byte[] argList)
    {
        args = argList;
    }
}



/// <summary>
/// 消息结构
/// </summary>

public class Q3Packet : BasePacket
{
    public override void Clear()
    {
       
    }

    public override int Id
    {
        get { return 1;}
    }
}
