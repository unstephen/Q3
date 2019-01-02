using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;

namespace GamePlay
{
    public class ShopForm : UGuiForm
    {
        GoodItem goodsItem;
        List<GoodItem> goodsList;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            goodsItem = link.Get<GoodItem>("GoodInfo");
            goodsItem.SetActive(false);
            goodsList = new List<GoodItem>();

            link.SetEvent("ButtonClose", UIEventType.Click, OnClickExit);

            RefreshGoods();
        }

        void RefreshGoods()
        {
            List<GoodsData> goods = GameManager.Instance.goodsList;

            //string jsonStr = File.ReadAllText("shopJson.txt");
            //Recv_Get_Shop shopData = JsonMapper.ToObject<Recv_Get_Shop>(jsonStr);
            //Debug.Log(jsonStr);
            if (goods != null)
            {
                int index = 0;
                foreach (var item in goods)
                {
                    GoodItem tempItem = goodsItem.Clone() as GoodItem;
                    if (tempItem)
                    {
                        tempItem.SetActive(true);
                        tempItem.OpenUI();
                        tempItem.SetItemInfo(item, index);

                        goodsList.Add(tempItem);
                    }

                    index++;
                }
            }
        }
        public void OnClickExit(params object[] args)
        {
            Close();
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
