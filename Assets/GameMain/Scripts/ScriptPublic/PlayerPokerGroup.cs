using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PokerData
{
    public int num; //1-k 1-13
    public int realNum; //1-10 J、Q、K 0点
    public int color; // 3 黑桃; 2 红桃; 1 梅花; 0 方块

    public PokerData(int _num, int _color)
    {
        num = _num;
        color = _color;
        realNum = num > 10 ? 0 : num;
    }
}

public class PlayerPokerGroup
{
    private PokerData[] playerPokers = new PokerData[GameConst.cardMaxNum];

    private int index = 0;

    public void AddPLayerPoker(int num, int color)
    {
        if (index < GameConst.cardMaxNum)
        {
            playerPokers[index++] = new PokerData(num, color);
        }
        else
        {
            Debug.Log("add index out of the range!");
        }
    }
}
