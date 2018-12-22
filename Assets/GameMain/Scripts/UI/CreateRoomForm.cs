using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class CreateRoomForm : UGuiForm
    {
        ProcedureMain main;
        private InputField InputRoomName;
        private InputField InputRoomPsd;
        private Toggle[] ToggleScore = new Toggle[4];
        private Toggle[] ToggleType = new Toggle[2];
        private Toggle[] TogglePlayer = new Toggle[2];
        private Toggle[] ToggleTime = new Toggle[2];
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            GameEntry.UI.SetUIFormInstancePriority(this.gameObject.GetComponent<UIForm>(), 121);
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnCreate", UIEventType.Click, OnCreateRoom);
            InputRoomName = link.Get<InputField>("InputRoomName");
            InputRoomPsd = link.Get<InputField>("InputRoomPsd");
            for (int i = 0; i < ToggleScore.Length; i++)
            {
                ToggleScore[i] = link.Get<Toggle>("ToggleScore"+(i+1));
            }
            for (int i = 0; i < ToggleType.Length; i++)
            {
                ToggleType[i] = link.Get<Toggle>("ToggleType"+(i+1));
            }
            for (int i = 0; i < TogglePlayer.Length; i++)
            {
                TogglePlayer[i] = link.Get<Toggle>("TogglePlayer"+(i+1));
            }
            for (int i = 0; i < ToggleTime.Length; i++)
            {
                ToggleTime[i] = link.Get<Toggle>("ToggleTime"+(i+1));
            }
            
        }

        private void OnCreateRoom(object[] args)
        {
            //TODO 设置俱乐部id
            int clubId = 0;
            //底分
            string score = "1";
            if (ToggleScore[0].isOn)
            {
                score = "5";
            }
            else if(ToggleScore[1].isOn)
            {
                score = "10";
            }
            else if(ToggleScore[2].isOn)
            {
                score = "15";
            }
            else
            {
                score = "20";
            }
            //游戏类型
            string gemeType = "1";
            if (ToggleType[0].isOn)
            {
                gemeType = "1";
            }
            else if(ToggleType[1].isOn)
            {
                gemeType = "2";
            }
            //游戏人数
            string playerCount = "6";
            if (TogglePlayer[0].isOn)
            {
                gemeType = "6";
            }
            else if(TogglePlayer[1].isOn)
            {
                gemeType = "10";
            }
            //游戏时长
            string time = "6";
            if (ToggleTime[0].isOn)
            {
                time = "1200";
            }
            else if(ToggleTime[1].isOn)
            {
                time = "2400";
            }

            var userId = GameManager.Instance.GetRoleData().id.Value.ToString();
            var token = GameManager.Instance.GetRoleData().token.Value;
            Recv_CreateRoom createRoom = NetWorkManager.Instance.CreateGetMsg<Recv_CreateRoom>(GameConst._createRoom, 
                GameManager.Instance.GetSendInfoStringList<Send_Create_Room>(userId,token,clubId.ToString(),InputRoomName.text,InputRoomPsd.text,
                    score,gemeType,playerCount,time));
            if (createRoom != null && createRoom.code == 0)
            {
                DoCreateRoom(createRoom.data.GID,createRoom.data.room_id);
            }
        }

        public void OnClickExit(params object[] args)
         {
             Close();
         }

        public void DoCreateRoom(int GID,int room_id)
        {
            var mode = main.CurGameMode();
            //初始化房间
            RoomManager.Instance.Init(GID,room_id,InputRoomName.text,0);
            //初始化扑克管理器
            var cardManager = CardManager.Instance;
            switch (main.CurGameMode())
            {
                case GameMode.Majiang:
                    break;
                case GameMode.Chudadi:
                    break;
                case GameMode.Sangong:
                    GameEntry.UI.OpenUIForm(UIFormId.TableForm, main);
                    Close(true);
                    break;
            }
           
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
