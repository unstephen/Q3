using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using UniRx;

namespace GamePlay
{
    public class TopBarForm : UGuiForm
    {
        private Text textName;
        private Text textId;
        private Text textToken;
        private Text textMoney;
        Image icon;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            textName = link.Get<Text>("TextName");
            textId = link.Get<Text>("TextID");
            textToken = link.Get<Text>("TextTicket");
            textMoney = link.Get<Text>("textMoney");
            icon = link.Get<Image>("headicon");
            //link.SetEvent("BtnSangong", UIEventType.Click, OnStartSangong);
        }

//         public void OnStartSangong(params object[] args)
//         {
//             var main = GameEntry.Procedure.CurrentProcedure as ProcedureMain;
//             main.ChangeGame(GameMode.Sangong);
//         }



#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            RoleData role = GameManager.Instance.GetRoleData();
            role.name.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textName).AddTo(disPosable);
            role.id.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textId).AddTo(disPosable);
            role.token.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textToken).AddTo(disPosable);

            string headUrl = "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0";
            string temp = NetWorkManager.Instance.GetResponseString(headUrl);

            WWW www = new WWW(headUrl);
            //yield return www;

            Texture2D tex2d = www.texture;
            //将图片保存至缓存路径
            byte[] pngData = tex2d.EncodeToPNG();
            //File.WriteAllBytes(path + url.GetHashCode(), pngData);

            Sprite m_sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0, 0));
            icon.sprite = m_sprite;
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
