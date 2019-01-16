using GamePlay;
using System.Collections.Generic;

public class SubChildPanelRequest : UGuiComponent
{
    ClubRequestItem requestItem;
    List<ClubRequestItem> requestItemList;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        if (userData == null)
        {
            return;
        }

        GUILink link = GetComponent<GUILink>();
        requestItem = link.GetComponent<ClubRequestItem>("Background");
        requestItem.SetActive(false);
        if (requestItemList == null)
        {
            requestItemList = new List<ClubRequestItem>();
        }

        InitLocal((List<ApplicantsData>)userData);
    }

    public void InitLocal(List<ApplicantsData> list)
    {
        if (list == null || list.Count == 0)
        {
            return;
        }

        int curIndex = 0;
        foreach (var item in list)
        {
            if (curIndex < requestItemList.Count)
            {
                if (!requestItemList[curIndex].isActiveAndEnabled)
                {
                    requestItemList[curIndex].SetActive(true);
                }

                requestItemList[curIndex].SetItemInfo(item, curIndex);
            }
            else
            {
                ClubRequestItem tempItem = requestItem.Clone() as ClubRequestItem;
                if (tempItem)
                {
                    tempItem.SetActive(true);
                    tempItem.OpenUI();
                    tempItem.SetItemInfo(item, curIndex);

                    requestItemList.Add(tempItem);
                }
            }

            curIndex++;
        }

        if (curIndex < requestItemList.Count)
        {
            for (int i = curIndex; i < requestItemList.Count; ++i)
            {
                requestItemList[i].SetActive(false);
            }
        }
    }
}