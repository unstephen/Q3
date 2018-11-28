using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace GamePlay
{
    public class PlayerStateController
    {
        private IFsmManager m_FsmManager;
        public IFsm<Player> fsm { private set; get; }

        public void Init(Player player,IFsmManager FSM,params PlayerStateBase[] states)
        {
            if (FSM == null)

            {

                throw new GameFrameworkException("FSM manager is invalid.");

            }



            m_FsmManager = FSM;

            fsm = m_FsmManager.CreateFsm(player.name.Value,player, states);
        
        }

        public void Start<T>() where T : PlayerStateBase
        {
            fsm.Start<T>();
        }
    }
}
