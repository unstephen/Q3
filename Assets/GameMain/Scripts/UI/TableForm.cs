using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class TableForm : UGuiForm
    {
        ProcedureMain main;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
        }

        public void OnClickExit(params object[] args)
        {
            Close();
            main.ChangeGame(GameMode.Lobby);
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
