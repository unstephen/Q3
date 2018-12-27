using System;
using GameFramework.Fsm;
using UniRx;
using UnityGameFramework.Runtime;
using PlayerOwner = GameFramework.Fsm.IFsm<GamePlay.Player>;

namespace GamePlay
{
    public class PlayerStateGameReady : PlayerStateBase
    {
        protected override void OnInit(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnInit(fsm);
        }

        protected override void OnEnter(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnEnter(fsm);
            fsm.Owner.OnGameReady();
            Log.Debug("进入准备");
            NetWorkManager.Instance.Send(Protocal.READY,RoomManager.Instance.rData.gId.Value);
        }

        protected override void OnUpdate(GameFramework.Fsm.IFsm<Player> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
            if (fsm.Owner.state == EPlayerState.Deal)
            {
                ChangeState<PlayerStateDeal>(fsm);
            }
            else if (fsm.Owner.state == EPlayerState.Seat)
            {
                ChangeState<PlayerStateSeat>(fsm);
            }
        }

        protected override void OnLeave(GameFramework.Fsm.IFsm<Player> fsm, bool isShutdown)
        {
            base.OnLeave(fsm, isShutdown);
            NetWorkManager.Instance.Send(Protocal.READY_CANCEL,RoomManager.Instance.rData.gId.Value);
        }

        protected override void OnDestroy(GameFramework.Fsm.IFsm<Player> fsm)
        {
            base.OnDestroy(fsm);
        }
    } 
}

