using System;
using GameFramework.Event;
using System.Collections.Generic;
using UniRx;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace GamePlay
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;

        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;
        private CreateRoomForm m_CreateRoomForm = null;
        public GameBase CurrentGame { get { return m_CurrentGame; } }
        private bool m_needSendLogin = true;
        private IDisposable loginDisposable;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        public GameMode CurGameMode()
        {
            return m_CurrentGame.GameMode;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            //初始化游戏模式
            m_Games.Add(GameMode.Lobby, new Lobby());
            m_Games.Add(GameMode.Majiang, new MajiangGame());
            m_Games.Add(GameMode.Chudadi, new ChudadiGame());
            m_Games.Add(GameMode.Sangong, new SangongGame());
        }

        public void ChangeGame(GameMode mode)
        {
            GameMode preMode = GameMode.Lobby;
            if (m_CurrentGame != null)
            {
                preMode = m_CurrentGame.GameMode;
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
                m_needSendLogin = true;
            }
            m_CurrentGame = m_Games[mode];
            m_CurrentGame.Initialize(preMode);

            
            if(mode>GameMode.Lobby)
            {

                GameEntry.Sound.PlayMusic(3);
                GameEntry.UI.OpenUIForm(UIFormId.CreateRoomForm, this);
            }
            else
            {
                GameEntry.Sound.PlayMusic(2);
            }
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);

            m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;

            m_CurrentGame = m_Games[GameMode.Lobby];
            m_CurrentGame.Initialize(GameMode.Lobby);
            GameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnConnected);
         
        }
        
        private void OnConnected(object sender, GameEventArgs e)
        {
            if (m_needSendLogin)
            {
              
                var role = GameManager.Instance.GetRoleData();
                NetWorkManager.Instance.Send(Protocal.Login, role.id.Value.ToString(), role.token.Value);
               // loginDisposable = GameManager.Instance.GetRoleData().pId.ObserveEveryValueChanged(p => p).Subscribe(s => StartGame());
                
                m_needSendLogin = false;
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }
      
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                return;
            }

            if (!m_GotoMenu)
            {
                m_GotoMenu = true;
                m_GotoMenuDelaySeconds = 0;
            }

            m_GotoMenuDelaySeconds += elapseSeconds;
            if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            {
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, GameEntry.Config.GetInt("Scene.Login"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_CreateRoomForm = (CreateRoomForm)ne.UIForm.Logic;
        }

    }
}
