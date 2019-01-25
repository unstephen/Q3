using GameFramework;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class CreateRoomForm : UGuiForm
    {
        ProcedureMain main;
        private SubCreatRoomKeyBoard KeyBoard;
        private Toggle[] ToggleScore = new Toggle[3];
        private Toggle[] ToggleMaxScore = new Toggle[3];
        private Toggle[] ToggleType = new Toggle[4];
        private Toggle[] TogglePlayer = new Toggle[2];
        private Toggle[] ToggleTime = new Toggle[2];
        private Toggle[] ToggleFeature = new Toggle[2];
        public Text TextPsd;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            main = userData as ProcedureMain;
            GUILink link = GetComponent<GUILink>();
            TextPsd = link.Get<Text>("TextPsd");
            KeyBoard = link.AddComponent<SubCreatRoomKeyBoard>("PanelSetPsd");
            GameEntry.UI.SetUIFormInstancePriority(this.gameObject.GetComponent<UIForm>(), 121);
            link.SetEvent("Quit", UIEventType.Click, OnClickExit);
            link.SetEvent("BtnCreate", UIEventType.Click, OnCreateRoom);
            link.SetEvent("ButtonSetPsd",UIEventType.Click, OnSetPsd);
            
            for (int i = 0; i < ToggleScore.Length; i++)
            {
                ToggleScore[i] = link.Get<Toggle>("ToggleScore"+(i+1));
            }
            for (int i = 0; i < ToggleMaxScore.Length; i++)
            {
                ToggleMaxScore[i] = link.Get<Toggle>("ToggleMaxScore"+(i+1));
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
            for (int i = 0; i < ToggleFeature.Length; i++)
            {
                ToggleFeature[i] = link.Get<Toggle>("ToggleFeature"+(i+1));
            }
        }

        private void OnSetPsd(object[] args)
        {
            KeyBoard.OpenUI();
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
            else if(ToggleType[2].isOn)
            {
                gemeType = "3";
            }
            else if(ToggleType[3].isOn)
            {
                gemeType = "4";
            }
            //游戏人数
            string playerCount = "6";
            if (TogglePlayer[0].isOn)
            {
                playerCount = "6";
            }
            else if(TogglePlayer[1].isOn)
            {
                playerCount = "10";
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
            
            //下注上限
            string max_score = "100";
            if (ToggleMaxScore[0].isOn)
            {
                max_score = "100";
            }
            else if(ToggleMaxScore[1].isOn)
            {
                max_score = "500";
            }
            else if(ToggleMaxScore[2].isOn)
            {
                max_score = "1000";
            }
            
            //个性选择,能否搓牌
            bool canRubbing = ToggleFeature[0].isOn;
            
            //发送消息创建房间

            var userId = GameManager.Instance.GetRoleData().id.Value.ToString();
            var token = GameManager.Instance.GetRoleData().token.Value;
            string psd = string.IsNullOrEmpty(TextPsd.text) ? "1" : TextPsd.text;
            Recv_CreateRoom createRoom = NetWorkManager.Instance.CreateGetMsg<Recv_CreateRoom>(GameConst._createRoom, 
                GameManager.Instance.GetSendInfoStringList<Send_Create_Room>(userId,token,clubId.ToString(),"xxxx",psd,
                    score,gemeType,playerCount,time));
            if (createRoom != null && createRoom.code == 0)
            { 
                NetWorkManager.Instance.CreateChanel();
                DoCreateRoom(createRoom.data.GID,createRoom.data.room_id,createRoom.data.max_score,createRoom.data.base_score);
                RoomManager.Instance.rData.canRubbing = canRubbing;
            }
        }

        public void OnClickExit(params object[] args)
         {
             Close();
         }

        public void DoCreateRoom(int GID,int room_id,int max_score,int min_score)
        {
            var mode = main.CurGameMode();
            //初始化房间
            RoomManager.Instance.Init(GID,room_id,"xxxx",0,max_score,min_score);
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
    
     public class SubCreatRoomKeyBoard : UGuiComponent
    {
        private Text TextContent;
        private ReactiveProperty<int> inputText = new ReactiveProperty<int>(-1);
        private int roomId;
        private string room_name;
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            GUILink link = GetComponent<GUILink>();
            TextContent = link.Get<Text>("TextContent");
            for (int i = 0; i < 10; i++)
            {
                int index = i;
                link.SetEvent("Button" + i, UIEventType.Click, x =>
                {
                    if (inputText.Value == -1)
                    {
                        inputText.Value = index;
                    }
                    else if (inputText.Value < 1000)
                    {
                        inputText.Value = inputText.Value * 10 + index;
                    }
                });
            }

            link.SetEvent("ButtonOK", UIEventType.Click, ClickOk);
            link.SetEvent("ButtonDel", UIEventType.Click, ClickDel);
            link.SetEvent("ButtonClose", UIEventType.Click, _ => {OnClose(null); });
        }

        private void ClickDel(object[] args)
        {
            if (inputText.Value <10 && inputText.Value >=0)
            {
                inputText.Value = -1;
            }
            else if (inputText.Value>=10 && inputText.Value < 10000)
            {
                inputText.Value = inputText.Value /10;
            }
        }

        private void ClickOk(object[] args)
        {
            var rootUI = GameEntry.UI.GetUIForm(UIFormId.CreateRoomForm) as CreateRoomForm;
            rootUI.TextPsd.text = inputText.Value.ToString();
            OnClose(null);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            inputText.Value = -1;
            roomId = -1;
            inputText.Subscribe(x=>
            {
                TextContent.text = x == -1? "" : x.ToString();
                
            }).AddTo(disPosable);
        }
    }
}
