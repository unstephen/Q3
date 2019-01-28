using GameFramework;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityGameFramework.Runtime;
using System.IO;
using UnityEngine.UI;

namespace GamePlay
{
    public struct HistoryText
    {
        public Text count;
        public Text score;
    }

    public class HistoryForm : UGuiForm
    {
        GoodItem goodsItem;
        List<GoodItem> goodsList;
        List<HistoryText> historyTextList;

        HistoryItem historyItem;

        int curIndex;
        int pageTotle;
        List<HistoryItem> itemList;

        bool isMax;
        Dictionary<int, List<HistorySingleBaseData>> pageHistoryList;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            link.SetEvent("ButtonClose", UIEventType.Click, OnClickExit);

            historyTextList = new List<HistoryText>();

            Text delta = link.Get<Text>("Text1");

            for (int i = 0; i < 6; i += 2)
            {
                HistoryText temp = new HistoryText();
                temp.count = link.Get<Text>("Text" + (i + 1));
                temp.score = link.Get<Text>("Text" + (i + 2));
                historyTextList.Add(temp);
            }

            historyItem = link.Get<HistoryItem>("HistoryItem");
            historyItem.SetActive(false);
            itemList = new List<HistoryItem>();

            link.SetEvent("ButtonLeft", UIEventType.Click, _ => ChangeHistoryPage(-1));
            link.SetEvent("ButtonRight", UIEventType.Click, _ => ChangeHistoryPage(1));

            pageHistoryList = new Dictionary<int, List<HistorySingleBaseData>>();
        }

        public void OnClickExit(params object[] args)
        {
            Close();
        }

        void RefreshHistorySingle(int page)
        {
            RoleData role = GameManager.Instance.GetRoleData();
            Recv_Get_SearchHistory historyData = NetWorkManager.Instance.CreateGetMsg<Recv_Get_SearchHistory>(GameConst._mainPage,
            GameManager.Instance.GetSendInfoStringList<Send_SearchHistory>(role.id.Value, role.token.Value, page.ToString(), GameConst.pageSize.ToString()));

            //string jsonStr = File.ReadAllText("JsonTest/history_2.txt");
            //Recv_Get_SearchHistory historyData = LitJson.JsonMapper.ToObject<Recv_Get_SearchHistory>(jsonStr);
            //Debug.Log(jsonStr);

            if (historyData != null)
            {
                if (historyData.data.message == "1" || historyData.data.message == "2")
                {
                    isMax = true;
                }

                ShowItemList(page, historyData.data.list);

                if (!pageHistoryList.ContainsKey(page))
                {
                    List<HistorySingleBaseData> tempList = new List<HistorySingleBaseData>();
                    if (historyData.data.list == null)
                    {
                        return;
                    }
                    foreach (var item in historyData.data.list)
                    {
                        tempList.Add(item);
                    }
                    
                    pageHistoryList.Add(page, tempList);
                }
            }
        }

        void RefreshHistoryAll()
        {
            RoleData role = GameManager.Instance.GetRoleData();
            Recv_Get_History historyData = NetWorkManager.Instance.CreateGetMsg<Recv_Get_History>(GameConst._mainPage,
            GameManager.Instance.GetSendInfoStringList<Send_SearchHistoryAll>(role.id.Value, role.token.Value));

            //string jsonStr = File.ReadAllText("JsonTest/history_1.txt");
            //Recv_Get_History historyData = LitJson.JsonMapper.ToObject<Recv_Get_History>(jsonStr);
            //Debug.Log(jsonStr);

            if (historyData != null)
            {
                int curIndex = 0;
                if (historyData.data.list != null)
                {
                    foreach (var item in historyData.data.list)
                    {
                        HistoryText temp = historyTextList[curIndex];
                        temp.count.text = item.games_count.ToString();
                        temp.score.text = item.profit.ToString();
                        historyTextList[curIndex] = temp;

                        curIndex++;
                    }
                }
                else
                {
                    foreach (var textitem in historyTextList)
                    {
                        textitem.count.text = "0";
                        textitem.score.text = "0";
                    }
                }
            }
        }

        private void ChangeHistoryPage(int delta)
        {
            int curPage = curIndex + delta;
            if (curPage < 0 || isMax)
            {
                return;
            }

            if (pageHistoryList.ContainsKey(curPage))
            {
                ShowItemList(curPage, pageHistoryList[curPage]);
            }
            else
            {
                RefreshHistorySingle(curPage);
            }
        }

        private void ShowItemList(int page, List<HistorySingleBaseData> list)
        {
            if (list == null)
            {
                return;
            }

            int curIndex = 0;
            foreach (var item in list)
            {
                if (curIndex < itemList.Count)
                {
                    if (!itemList[curIndex].isActiveAndEnabled)
                    {
                        itemList[curIndex].SetActive(true);
                    }

                    itemList[curIndex].SetItemInfo(item, curIndex);
                }
                else
                {
                    HistoryItem tempItem = historyItem.Clone() as HistoryItem;
                    if (tempItem)
                    {
                        tempItem.SetActive(true);
                        tempItem.OpenUI();
                        tempItem.SetItemInfo(item, curIndex);

                        itemList.Add(tempItem);
                    }
                }

                curIndex++;
            }

            if (curIndex < itemList.Count)
            {
                for (int i = curIndex; i < itemList.Count; ++i)
                {
                    itemList[i].SetActive(false);
                }
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            curIndex = 0;
            isMax = false;
            RefreshHistorySingle(curIndex);
            RefreshHistoryAll();
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
