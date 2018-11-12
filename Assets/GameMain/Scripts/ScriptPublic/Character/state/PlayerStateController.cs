using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;

public class PlayerStateController
{
    private IFsmManager m_FsmManager;
    private IFsm<Player> m_PlayerFsm;

    public void Init(Player player,IFsmManager FSM,params PlayerStateBase[] states)
    {
        if (FSM == null)

        {

            throw new GameFrameworkException("FSM manager is invalid.");

        }



        m_FsmManager = FSM;

        m_PlayerFsm = m_FsmManager.CreateFsm(player.name.Value,player, states);
        
    }

    public void Start<T>() where T : PlayerStateBase
    {
        m_PlayerFsm.Start<T>();
    }

    public void FireEvent()
    {
        
    }
}