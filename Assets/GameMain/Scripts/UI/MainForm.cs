using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using UniRx;

namespace GamePlay
{
    public class MainForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
            link.SetEvent("BtnSangong", UIEventType.Click, OnStartSangong);
            link.SetEvent("BtnShop", UIEventType.Click, OnShopClick);

            RoleData role = GameManager.Instance.GetRoleData();
            //string id = "user_id=" + role.id.Value;
            //string token = "access_token=" + role.token.Value;
            //long time = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            //string timeStr = "timestamp=" + time.ToString();

            Recv_Get_MainPage mainPage = NetWorkManager.Instance.CreateGetMsg<Recv_Get_MainPage>(GameConst._mainPage, 
                GameManager.Instance.GetSendInfoStringList<Send_Get_MainPage>(role.id.Value, role.token.Value));
            if (mainPage != null && mainPage.code == 0)
            {
                role.SetRoleProperty(mainPage.data);
            }
        }

        public void OnStartSangong(params object[] args)
        {
            var main = GameEntry.Procedure.CurrentProcedure as ProcedureMain;
            main.ChangeGame(GameMode.Sangong);
        }

        public void OnShopClick(params object[] args)
        {
            RoleData role = GameManager.Instance.GetRoleData();

            //商店测试
            //Recv_Get_Shop shopPage = NetWorkManager.Instance.CreateGetMsg<Recv_Get_Shop>(GameConst._shop,
            //    GameManager.Instance.GetSendInfoStringList<Send_Get_Shop>(role.id.Value, role.token.Value));
            //if (shopPage != null && shopPage.code == 0)
            //{
            //    Debug.Log("show skop page");
            //}

            //订单测试
            Recv_Post_Order shopPage = NetWorkManager.Instance.CreatePostMsg<Recv_Post_Order>(GameConst._order,
                GameManager.Instance.GetSendInfoStringList<Send_Post_Order>(role.id.Value, role.token.Value, "0"));
            if (shopPage != null && shopPage.code == 0)
            {
                Debug.Log("show skop page");
            }
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
