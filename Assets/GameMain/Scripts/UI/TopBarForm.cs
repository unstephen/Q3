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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();

            textName = link.Get<Text>("TextName");
            textId = link.Get<Text>("TextID");
            textToken = link.Get<Text>("TextTicket");
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
