using GamePlay;
using UnityEngine.UI;

public class subPanelBaseClubInfo : UGuiComponent
{
    Text text_id;
    Text text_name;
    Text text_info;
    Text text_vip;
    Text text_count;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = GetComponent<GUILink>();
        text_id = link.Get<Text>("id_Text");
        text_name = link.Get<Text>("name_Text");
        text_info = link.Get<Text>("info_Text");
        text_vip = link.Get<Text>("vip_Text");
        text_count = link.Get<Text>("count_Text");
    }

    protected override void OnOpen(object userData)
    {
        base.OnOpen(userData);

        ShowBaseInfo((int)userData);
    }

    public void ShowBaseInfo(int index)
    {
        RoleData role = GameManager.Instance.GetRoleData();

        ClubData temp = role.myClubList[index];
        text_id.text = temp.club_id;
        text_name.text = temp.club_name;
        text_info.text = temp.ongoing_games.ToString();
        text_vip.text = temp.vip_level.ToString();
        text_count.text = temp.member_number.ToString();
    }
}