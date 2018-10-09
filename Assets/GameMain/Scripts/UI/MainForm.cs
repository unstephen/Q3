using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class MainForm : UGuiForm
    {

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
           // link.SetEvent("Start", UIEventType.Click, OnStartButtonClick);
           // link.SetEvent("Quit", UIEventType.Click, OnQuitButtonClick);
        }

        public void OnStartButtonClick(params object[] args)
        {
           // m_ProcedureMenu.StartGame();
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


        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {

            base.OnClose(userData);
        }
    }
}
