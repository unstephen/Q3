using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;
using UnityEngine.UI;
using UniRx;
using System.Collections;

namespace GamePlay
{
    public class TopBarForm : UGuiForm
    {
        private Text textName;
        private Text textId;
        private Text textToken;
        private Text textMoney;
        Image icon;
        string headUrl;

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

        IEnumerator getimage()
        {
            WWW www = new WWW(headUrl);//用WWW加载网络图片
            yield return www;
            //icon = transform.GetComponent<Image>();
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                //获取Texture
                Texture2D texture = www.texture;
                //因为我们定义的是Image，所以这里需要把Texture2D转化为Sprite
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                icon.sprite = sprite;
            }
        }


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
            //role.token.ObserveEveryValueChanged(x => x.Value).SubscribeToText(textToken).AddTo(disPosable);

            //headUrl = "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0";
            if (role != null && !string.IsNullOrEmpty(role.headUrl))
            {
                headUrl = role.headUrl;
                StartCoroutine("getimage");
            }
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
