using System;
using GameFramework.Fsm;
using UniRx;
using UnityGameFramework.Runtime;
using PlayerOwner = GameFramework.Fsm.IFsm<GamePlay.Player>;

namespace GamePlay
{
    public class PlayerStateEnd : PlayerStateBase
    {
        protected override void OnInit(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnEnter(fsm);
            fsm.Owner.OnEnd();
            Log.Debug("进入结束");
        }

        protected override void OnUpdate(GameFramework.Fsm.IFsm<Player> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            //如果手牌数据有3个有效的（不为0）则翻牌算牌型
            if (fsm.Owner.handCardsData.Count > 0)
            {
                int validNum = 0;
                for (int i = 0; i < fsm.Owner.handCardsData.Count; i++)
                {
                    if (fsm.Owner.handCardsData[i] > 0)
                        validNum++;
                }

                if (validNum == fsm.Owner.handCardsData.Count)
                {
                    fsm.Owner.OnMakeHandCard();
                    ChangeState<PlayerStateCardStyle>(fsm);
                }
            }
          
        }

        protected override void OnLeave(GameFramework.Fsm.IFsm<Player> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnDestroy(fsm);
        }
    } 
}

