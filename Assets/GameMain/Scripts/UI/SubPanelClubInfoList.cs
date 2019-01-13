using GamePlay;
using System.Collections.Generic;
using UnityEngine.UI;

public class SubPanelClubInfoList : UGuiComponent
{
    string[] tabStr = new string[3] { "ToggleMembers", "ToggleGames", "ToggleRequest" };
    List<Toggle> toggleList = new List<Toggle>();

    private int curIndex;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();

        for (int i = 0; i < tabStr.Length; i++)
        {
            Toggle temp = link.Get<Toggle>(tabStr[i]);
            toggleList.Add(temp);

            int index = i;
            link.SetEvent(tabStr[i], UIEventType.Click, _ => OnClickChange(index));
        }

        curIndex = 0;
    }

    public void OnClickChange(int index)
    {
        if (curIndex == index)
        {
            return;
        }

        curIndex = index;
        switch (index)
        {

        }
    }
}