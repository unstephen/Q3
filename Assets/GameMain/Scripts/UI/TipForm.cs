using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using UnityEngine.UI;

namespace GamePlay
{
    public class TipForm : UGuiForm
    {
        bool isfill = false;
        Text content;
        RectTransform container;

        GameObject panelWeixin;
        GameObject panelRule;
        Text titleName;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            panelWeixin = link.Get("PanelSurface");
            panelRule = link.Get("PanelRule");
            titleName = link.Get<Text>("TextName");

            content = link.Get<Text>("TextDesc");
            container = link.Get<RectTransform>("Content");

            link.SetEvent("ButtonClose", UIEventType.Click, OnClickExit);
        }

        public void OnClickExit(params object[] args)
        {
            Close();
        }

        private void LateUpdate()
        {
            if (isfill)
            {
                var rt = content.GetComponent<RectTransform>();
                var oldSize = container.sizeDelta;
                oldSize.y = rt.sizeDelta.y - rt.anchoredPosition.y;
                container.sizeDelta = oldSize;

                isfill = false;
            }
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

            int index = (int)userData;
            RefreshPanelInfo(index);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);
        }

        /// <summary>
        /// 显示对应界面
        /// </summary>
        /// <param name="index">0：玩法 1：客服</param>
        void RefreshPanelInfo(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        panelWeixin.SetActive(false);
                        panelRule.SetActive(true);

                        titleName.text = "玩法说明";
                        isfill = true;
                    }
                    break;
                case 1:
                    {
                        panelWeixin.SetActive(true);
                        panelRule.SetActive(false);

                        titleName.text = "客服";
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
