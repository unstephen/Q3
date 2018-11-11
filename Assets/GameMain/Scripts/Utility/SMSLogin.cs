using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using LitJson;
#if false
public class SMSLogin : PanelBase, SMSSDKHandler
{

    // Use this for initialization
    // public GUISkin demoSkin;
    public SMSSDK smssdk;
    public UserInfo userInfo;
    //please add your phone number
    private string phone = "";
    private string zone = "86";
    private string tempCode = "9773719";
    private string code = "";
    private string result = null;
    public InputField num;
    public Text TimerText;
    //public InputField Zone;
    public InputField SMScode;
    public GameObject getCode;
    //public Text Log;
    public static SMSLogin Instance { get; private set; }
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        Instance = this;
        Debug.Log("[SMSSDK]Demo  ===>>>  Start");
        smssdk = gameObject.GetComponent<SMSSDK>();
        smssdk.init("2572dfd2bcfc8", "443ac92c0138902f99eb3650a99901dd", true);
        userInfo = new UserInfo();
        smssdk.setHandler(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GetSMSCode()
    {
        phone = num.text;
        //  zone = Zone.text;
        if (phone.Length == 11)
        {
            smssdk.getCode(CodeType.TextCode, phone, zone, tempCode);
        }
        else
        {
            UIManager.Instance.Create<PopupReminder>( ShowReminder );
        }
    }

    public void CommitCode()
    {
        code = SMScode.text;
        phone = num.text;
        // zone = Zone.text;
        if (code.Length == 4 && phone.Length == 11)
        {
            smssdk.commitCode(phone, zone, code);
        }
        else
        {
            UIManager.Instance.Create<PopupReminder>( ShowReminder );
        }
    }

    private void ShowReminder()
    {
        PopupReminder reminder = UIManager.Instance.Get<PopupReminder>( );
        reminder.Show("输入错误，请重新输入");
    }

    public void CommitUserInfo()
    {
        code = SMScode.text;
        phone = num.text;
        // zone = Zone.text;

    }


    public void onComplete(int action, object resp)
    {
        // Log.text = resp.ToString()+action;
        print("***********resp.ToString*********" + resp.ToString());
        ActionType act = (ActionType)action;
        if (resp != null)
        {
            result = resp.ToString();
        }
        if (act == ActionType.GetCode)
        {
            //getCode.SetActive(false);
            Util.SetButtonState(false, getCode.GetComponent<UIButton>());
            StartCoroutine(TimerClock());
            string responseString = (string)resp;
            Debug.Log("isSmart :" + responseString);
        }
        else if (act == ActionType.GetVersion)
        {
            string version = (string)resp;
            Debug.Log("version :" + version);
            Debug.Log("Demo*version*********" + version);

        }
        else if (act == ActionType.GetSupportedCountries)
        {

            string responseString = (string)resp;
            Debug.Log("zoneString :" + responseString);

        }
        else if (act == ActionType.GetFriends)
        {
            string responseString = (string)resp;
            Debug.Log("friendsString :" + responseString);

        }
        else if (act == ActionType.CommitCode)
        {
            string responseString = (string)resp;
            GameData.Inst.SmsInfo = new SMSResp();
#if UNITY_ANDROID
            GameData.Inst.SmsInfo = JsonMapper.ToObject<SMSResp>(responseString);
#elif UNITY_IOS
               GameData.Inst.SmsInfo.country = zone;
            GameData.Inst.SmsInfo.phone = phone;
#endif
            WeChat.instance.SMSLogin();
            Debug.Log("commitCodeString :" + responseString);
        }
        else if (act == ActionType.SubmitUserInfo)
        {

            string responseString = (string)resp;
            Debug.Log("submitString :" + responseString);

        }
        else if (act == ActionType.ShowRegisterView)
        {

            string responseString = (string)resp;
            Debug.Log("showRegisterView :" + responseString);

        }
        else if (act == ActionType.ShowContractFriendsView)
        {

            string responseString = (string)resp;
            Debug.Log("showContractFriendsView :" + responseString);
        }
    }

    public void onError(int action, object resp)
    {
        Debug.Log("Error :" + resp);
        result = resp.ToString();
        print("OnError ******resp" + resp);
    }
    public IEnumerator TimerClock()
    {
        TimerText.gameObject.SetActive(true);
        int timer;
        for (int i = 0; i <61; i++)
        {
             timer= 60 - i;
            TimerText.text = timer + "秒后重新获取";
            yield return new WaitForSeconds(1);
            if (timer <= 0)
            {
                Util.SetButtonState(true, getCode.GetComponent<UIButton>());
                TimerText.gameObject.SetActive(false);
            }
        }
      
    }
   
}

#endif