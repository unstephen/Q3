using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using GameFramework.Resource;
using GamePlay;
using UniRx;
using UnityGameFramework.Runtime;
using GameEntry = GamePlay.GameEntry;

public class CardManager : MonoSingleton<CardManager> 
{
    private int[] cardNames;  //所有牌集合
    private Transform _heapPos;
    public Transform heapPos
    {
        get
        {
            if (_heapPos == null)
            {
                var temp = GameObject.Find("heapPos");
                if (temp != null)
                    _heapPos = temp.transform;
            }
            return _heapPos;
        }
    } //牌堆位置

    public Vector3[] playerHeapPos;    //玩家牌堆位置
    public CardManagerStates cardManagerState;  //卡牌回合状态
    private int termCurrentIndex;  //回合当前玩家索引
    private int termStartIndex;  //回合开始玩家索引
    //public GameObject coverPrefab;      //背面排预制件
    public float dealCardSpeed = 20;  //发牌速度
    public override void Init()
    {
         cardNames = GetCardNames();
//        GameEntry.Resource.LoadAsset(AssetUtility.GetCardAsset("0"), new LoadAssetCallbacks(
//            (assetName, asset, duration, userData) =>
//            {
//                coverPrefab = GameObject.Instantiate(asset as GameObject);
//                coverPrefab.transform.localScale = Vector3.one;
//                coverPrefab
//            },
//
//            (assetName, status, errorMessage, userData) =>
//            {
//                Log.Error("Can not cover card '{0}' from '{1}' with error message '{2}'.", 0, assetName, errorMessage);
//            }));

    }

    public List<Player> Players
    {
        get { return RoomManager.Instance.rData.allPlayers; }
    }

    #region 洗牌、发牌
    /// <summary>
    /// 洗牌
    /// </summary>
    public void ShuffleCards()
    {
        //进入洗牌阶段
        cardManagerState = CardManagerStates.ShuffleCards;
        cardNames = cardNames.OrderBy(c => Guid.NewGuid()).ToArray();
    }
    /// <summary>
    /// 初始化牌堆
    /// </summary>
    public void InitCardPos(List<Vector3> transLst)
    {
        playerHeapPos = transLst.ToArray();
    }
    /// <summary>
    /// 开始发牌
    /// </summary>
    public void DoDealCards()
    {
        StartCoroutine(DealCards());
        RoomManager.Instance.Self.Value.state = EPlayerState.Deal;
        foreach (var other in RoomManager.Instance.rData.roomPlayers)
        {
            other.state = EPlayerState.Deal;
        }
    }
   
    IEnumerator DealCards()
    {
        //进入发牌阶段
        cardManagerState = CardManagerStates.DealCards;
        termCurrentIndex = termStartIndex;

        yield return DealHeapCards();
    }
    /// <summary>
    /// 发牌堆上的牌（如果现在不是抢地主阶段，发普通牌，如果是，发地主牌）
    /// </summary>
    /// <returns></returns>
    private IEnumerator DealHeapCards()
    {
        //显示牌堆
        heapPos.gameObject.SetActive(true);

        //按玩家数量，每人3张牌来取牌
        var cardNamesNeeded = cardNames.Take(Players.Count * 3);       

        //计算每张地主牌的位置
        int cardIndex = 0;
        foreach (var cardName in cardNamesNeeded)
        {
            //给当前玩家发一张牌
            var nowCardNum = Players[termCurrentIndex].handCards.Count;
            Players[termCurrentIndex].GetCard(cardName);
            var tempindex = termCurrentIndex;
            var cover = Instantiate(heapPos.gameObject);
          //  cover.GetComponent<RectTransform>().localScale = Vector3.one;
            //移动动画，动画结束后自动销毁
            var tween = cover.transform.DOMove(playerHeapPos[tempindex], 0.3f);
            tween.OnComplete(() =>
            {
                Players[tempindex].ShowLastCard(nowCardNum);
                Destroy(cover);
            });

            yield return new WaitForSeconds(1 / dealCardSpeed);

         
            //下一个需要发牌者
          //
           SetNextPlayer();

            cardIndex++;
        }

    
        //隐藏牌堆
        heapPos.gameObject.SetActive(false);
       // playerHeapPos[0].gameObject.SetActive(false);

        //发普通牌
        //显示玩家手牌
       // ShowPlayerSelfCards();
        cardManagerState = CardManagerStates.Playing;
    }
    public void SetNextPlayer()
    {
        termCurrentIndex = (termCurrentIndex + 1) % Players.Count;
    }

//    /// <summary>
//    /// 显示玩家手牌
//    /// </summary>
//    private void ShowPlayerSelfCards()
//    {
//        //销毁玩家手牌
//        DestroyPlayerSelfCards();
//
//        Players.ToList().ForEach(s =>
//        {
//            var player0 = s as PlayerSelf;
//            if (player0 != null)
//            {
//                player0.GenerateAllCards();
//            }
//        });
//    }
//    /// <summary>
//    /// 清空牌局
//    /// </summary>
//    public void ClearCards()
//    {
//        //清空所有玩家卡牌
//        Players.ToList().ForEach(s => s.DropCards());
//
//        //销毁玩家手牌
//        DestroyPlayerSelfCards();
//    }
//    /// <summary>
//    /// 
//    /// </summary>
//    private void DestroyPlayerSelfCards()
//    {
//        //销毁玩家手牌
//        Players.ToList().ForEach(s =>
//        {
//            var player0 = s as PlayerSelf;
//            if (player0 != null)
//            {
//                player0.DestroyAllCards();
//            }
//        });
//    }
    /// <summary>
    /// 加载所有卡牌名
    /// </summary>
    /// <returns></returns>
    private int[] GetCardNames()
    {
        int[] ret = new int[GameConst.pokerCount]; 
        for (int i = 0; i < GameConst.pokerCount; i++)
        {
            ret[i] = i + 1;
        }

        return ret;
    }
    #endregion
}
