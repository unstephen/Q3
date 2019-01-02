using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;

namespace GamePlay
{
    public class GameSettingForm : UGuiForm
    {
        GoodItem goodsItem;
        List<GoodItem> goodsList;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            link.SetEvent("ButtonClose", UIEventType.Click, OnClickExit); 
            link.SetEvent("ButtonCreate", UIEventType.Click, OnCreateRoom);
        }

        public void OnClickExit(params object[] args)
        {
            Close();
        }

        public void OnCreateRoom(params object[] args)
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
            //            PlayerStateInit s = new PlayerStateInit();
            //            s.star
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
