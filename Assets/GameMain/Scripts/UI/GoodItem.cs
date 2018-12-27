using GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class GoodItem : UGuiComponentClone
{
    Text idstr;
    Text price;
    Image icon;
    Text name;
    public int index;

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        GUILink link = this.gameObject.GetComponent<GUILink>();

        idstr = link.Get<Text>("Name");
        name = link.Get<Text>("Num");
        icon = link.Get<Image>("Image");
        price = link.Get<Text>("Price");

        link.SetEvent("goodButton", UIEventType.Click, _ => OnClick());
    }

    public void SetItemInfo(GoodsData data, int index)
    {
        this.index = index;

        idstr.text = data.goods_id;
        name.text = data.goods_name.ToString();
        price.text = data.price.ToString();
    }

    public void OnClick()
    {
        GameEntry.UI.OpenUIForm(UIFormId.DialogForm, new DialogParams
        {
            Mode = 2,
            Title = "购买商品",
            Message = "是否花费" + price.text + "够买" + idstr.text + "?",
            ConfirmText = "确定",
            OnClickConfirm = delegate (object userData) { DoMessegeBox(true); },
            CancelText = "取消",
            OnClickCancel = delegate (object userData) { DoMessegeBox(false); }
        });
    }

    public void DoMessegeBox(bool sure)
    {
        Debug.Log("********************" + sure);
    }
}