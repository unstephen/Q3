using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class CreateRoomForm : UGuiForm
    {
        ProcedureMain main;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            GameEntry.UI.SetUIFormInstancePriority(this.gameObject.GetComponent<UIForm>(), 121);
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnCreate", UIEventType.Click, OnCreateRoom);
        }

         public void OnClickExit(params object[] args)
         {
             Close();
             //main.ChangeGame(GameMode.Lobby);
         }

        public void OnCreateRoom(params object[] args)
        {
            var mode = main.CurGameMode();
            switch (main.CurGameMode())
            {
                case GameMode.Majiang:
                    break;
                case GameMode.Chudadi:
                    break;
                case GameMode.Sangong:
                    GameEntry.UI.OpenUIForm(UIFormId.TableForm, main);
                    break;
            }
         //   main.ChangeGame(mode);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);


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
