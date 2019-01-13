using System;
using GameFramework.Fsm;
using UniRx;
using UnityGameFramework.Runtime;
using PlayerOwner = GameFramework.Fsm.IFsm<GamePlay.Player>;

namespace GamePlay
{
    public class PlayerStatePlaying : PlayerStateBase
    {
        protected override void OnInit(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnInit(fsm);
            Log.Debug("PlayerStatePlaying OnInit name={0}",fsm.Owner.name);
        }

        protected override void OnEnter(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnEnter(fsm);
            Log.Debug("进入发牌表现");
            CardManager.Instance.DoDealCards();
        }

        protected override void OnUpdate(GameFramework.Fsm.IFsm<Player> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (CardManager.Instance.cardManagerState == CardManagerStates.Playing)
            {
                //先发牌再抢庄
                fsm.Owner.state = EPlayerState.Banker;
                ChangeState<PlayerStateBanker>(fsm);
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

