using GameFramework;
using System;
using System.Collections.Generic;
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
            link.SetEvent("BtnSangong", UIEventType.Click, OnStartSangong);

            string userId = "user_id=0";
            string token = "access_token=test_token";
            long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            string timeStr = "timestamp=" + time.ToString();

            Recv_Get_MainPage mainPage = NetWorkManager.Instance.CreateGetMsg<Recv_Get_MainPage>(GameConst._mainPage, new List<string> { userId, token, timeStr });
            if (mainPage != null && mainPage.code == 0)
            {

            }
        }

        public void OnStartSangong(params object[] args)
        {
            var main = GameEntry.Procedure.CurrentProcedure as ProcedureMain;
            main.ChangeGame(GameMode.Sangong);
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
