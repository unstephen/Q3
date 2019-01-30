using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using UniRx;

namespace GamePlay
{
    public class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu = null;
        Text Textrecv;
        Text Textsend;
        private bool isClicked = false;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
            link.SetEvent("Start", UIEventType.Click, OnStartButtonClick);
            link.SetEvent("Quit", UIEventType.Click, OnQuitButtonClick);

            Textrecv = link.Get<Text>("Textrecv");
            Textsend = link.Get<Text>("Textsend");

        }

        private void WechatLogin(WeChatAccessToken token) {//第一次登录  
            //PlayerPrefs.SetString( CODE, code );
            WeChat.WxLogin -= WechatLogin;
            Login( token.access_token, token.openid );
        }

        private void Login(string token, string openid) {
            Log.Info( "Login...token=" + token + "........openid=" + openid );
            Recv_Login login = NetWorkManager.Instance.CreateGetMsg<Recv_Login>( GameConst._login, new List<string> { "login_type=weixin", token, openid } );

            if (login != null && login.code == 0) {
                GameManager.Instance.InitRoleData( login.data.user_id, login.data.access_token );
            }
            if (login != null) {
                m_ProcedureMenu.StartGame();
            }
        }

        private const string CODE = "code";
        public void OnStartButtonClick(params object[] args)
        {
            if (isClicked)
                return;
            isClicked = true;
            //var code = PlayerPrefs.GetString( CODE );
            //if (string.IsNullOrEmpty( code )) {
                WeChat.WxLogin += WechatLogin;
                WeChat.Instance.WechatLogin();
            //}
            //else {//第二次登录
            //    Log.Info( "code:" + code );                
            //}
           
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

            RoleData role = GameManager.Instance.GetRoleData();
            if (role != null)
            {
                role.token.SubscribeToText(Textrecv);
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
