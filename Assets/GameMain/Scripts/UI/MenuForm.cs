using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
            link.SetEvent("Start", UIEventType.Click, OnStartButtonClick);
            link.SetEvent("Quit", UIEventType.Click, OnQuitButtonClick);
        }

        public void OnStartButtonClick(params object[] args)
        {    
            string type = "login_type=weixin";
            string token = "access_token=test_token";
            string openId = "openid=123";
                
            Recv_Login login = NetWorkManager.Instance.CreateGetMsg<Recv_Login>(GameConst._login, new List<string> { type, token, openId });

            if (login != null && login.code == 0)
            {
                GameManager.Instance.InitRoleData(login.data.user_id, login.data.access_token);

           
                
               
            }
            //NetWorkManager.Instance.CreateGameSocket( GameConst.ipadress, OnSocketConnect );
            if (login != null)
            {
                NetWorkManager.Instance.CreateChanel();
            }
        }
        
 
        public void OnQuitButtonClick(params object[] args)
        {
            Log.Debug("quit{0}",111111);
        }


#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureMenu = (ProcedureMenu)userData;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }
//            PlayerStateInit s = new PlayerStateInit();
//            s.star
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            m_ProcedureMenu = null;

            base.OnClose(userData);
        }
    }
}
