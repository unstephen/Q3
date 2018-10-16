using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class TopBarForm : UGuiForm
    {

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
            //link.SetEvent("BtnSangong", UIEventType.Click, OnStartSangong);
        }

//         public void OnStartSangong(params object[] args)
//         {
//             var main = GameEntry.Procedure.CurrentProcedure as ProcedureMain;
//             main.ChangeGame(GameMode.Sangong);
//         }



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
