using GamePlay;
using System.Collections.Generic;

public class SubChildPanelGames : UGuiComponent
{
    ClubGameItem gameItem;
    List<ClubGameItem> gameItemList;

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        if (userData == null)
        {
            return;
        }

        GUILink link = GetComponent<GUILink>();
        gameItem = link.GetComponent<ClubGameItem>("running");
        gameItem.SetActive(false);
        if (gameItemList == null)
        {
            gameItemList = new List<ClubGameItem>();
        }

        InitLocal((List<RunGameData>)userData);
    }

    public void InitLocal(List<RunGameData> list)
    {
        if (list == null || list.Count == 0)
        {
            return;
        }

        int curIndex = 0;
        foreach (var item in list)
        {
            if (curIndex < gameItemList.Count)
            {
                if (!gameItemList[curIndex].isActiveAndEnabled)
                {
                    gameItemList[curIndex].SetActive(true);
                }

                gameItemList[curIndex].SetItemInfo(item, curIndex);
            }
            else
            {
                ClubGameItem tempItem = gameItem.Clone() as ClubGameItem;
                if (tempItem)
                {
                    tempItem.SetActive(true);
                    tempItem.OpenUI();
                    tempItem.SetItemInfo(item, curIndex);

                    gameItemList.Add(tempItem);
                }
            }

            curIndex++;
        }

        if (curIndex < gameItemList.Count)
        {
            for (int i = curIndex; i < gameItemList.Count; ++i)
            {
                gameItemList[i].SetActive(false);
            }
        }
    }
}