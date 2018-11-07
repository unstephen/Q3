using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;
#if false
public class WeChat : MonoBehaviour
{
    private static WeChat _instance;
    public static WeChat instance
    {
        get
        {
            return _instance;
        }
    }
   // public Text LogText;
#if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]  
        private static extern void WechatLogin();  
        [DllImport ("__Internal")]  
        private static extern void shareFriend(bool isInFriendCircle,string title, string webUrl, string des, string base64Data); 
        [DllImport ("__Internal")]
        private static extern void iOSShareRecord(string path);
        [DllImport ("__Internal")]
        private static extern void wxPay(string mch_id,string prepay_Id,string nonce_str,string time_Stamp,string sign);
   
#endif
    NetworkManager network;
     /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _instance=this;
    }
    public void CallSdkApi(string apiName, params object[] args)
    {
#if UNITY_ANDROID
        //Debug.Log("CallSdkApi "+apiName);
        //for(int i=0;i<args.Length;i++)
        //{
        //    Debug.Log("args "+args[i].ToString());
        //}
       AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
       AndroidJavaObject androidJavaObj = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
       // androidJavaClass.Call(apiName,args);
        androidJavaObj.Call(apiName,args);
#endif
    }

    public void CallStaticSdkApi(string apiName, params object[] args)
    {
        #if UNITY_ANDROID
        Debug.Log("CallStaticSdkApi "+apiName);    //111
        AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject androidJavaObj = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
        androidJavaObj.CallStatic(apiName,args);
        #endif
    }
  
    public void WeChatLogin()
    {
       
#if UNITY_ANDROID
        CallSdkApi("WechatLogin");
#elif UNITY_IOS&&!UNITY_EDITOR
        WechatLogin();
#endif

    }

    public void WechatToken(string info)
    {
        // Debug.Log("token "+info);   
        WeChatAccessToken accessToken=LitJson.JsonMapper.ToObject<WeChatAccessToken>(info);
        string[] str=new string[]{accessToken.access_token,accessToken.openid};
        CallSdkApi("GetUserInfoReq",str);

    }

    public void WechatUserInfo(string info)
    {
      //  Debug.Log("userinfo "+info);
       GameData.Inst.wxUserInfo=LitJson.JsonMapper.ToObject<WechatUserInfo>(info);
       Login(GameData.Inst.wxUserInfo);
    }

    public void Login(WechatUserInfo user)
    {
        if(network==null)
            network = NetworkManager.Instance;
        C2S_LoginMsg msg = new C2S_LoginMsg();
		msg.openId = user.openid;
        msg.nickName = user.nickname;
        Packet packet = new Packet(Protocal.LOGIN, msg);
        if(network!=null){
            network.EmitPacket(packet);
        }else
        {
            Debug.LogError("network is null");
        }    
    }

    public void ShareFriend(bool moments,string title, string webUrl, string desc, string base64Data) 
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string[] str=new string[]{title,webUrl,desc,base64Data};
        CallSdkApi("shareFriend",moments,title,webUrl,desc,base64Data);
#elif UNITY_IOS && !UNITY_EDITOR
 //shareFriend(bool isInFriendCircle, const char *title, const char *webUrl, const char *des, const char *base64Data); 

        shareFriend(moments,title,webUrl,desc,base64Data);
#endif
    }

    public void ShareRecord(string recordStr,bool friend)
    {
          #if UNITY_ANDROID && !UNITY_EDITOR
        CallSdkApi("ShareRecord",recordStr,friend);
        # elif UNITY_IOS && !UNITY_EDITOR
        iOSShareRecord(recordStr);
            #endif
    }

    public void WxPay(string mch_id,string prepay_Id,string nonce_str,string timeStamp,string sign)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
       string[] str = new string[] { mch_id, prepay_Id, nonce_str, timeStamp,sign };
        CallSdkApi("wxPay", str);
#elif UNITY_IOS && !UNITY_EDITOR
        wxPay(mch_id,prepay_Id,nonce_str,timeStamp,sign);
#endif
    }
    string APP_ID;
    string APP_SE;
    public void IOSWeChatResponse(string code)
    {
     string furl= ("https://api.weixin.qq.com/sns/oauth2/access_token?appid="+ APP_ID + "&secret="+ APP_SE +"&code=" +code + "&grant_type=authorization_code");
       StartCoroutine("HttpGetUserInfo",furl);
    }
    public void onWechatResp(string code)//AS代码调用
    {
        WeChatResponse(code);
    }
    public void WeChatResponse(string code)
    {
     //   LogText.text = "android==>unity";
        string wxurl = "https://api.weixin.qq.com/sns/oauth2/access_token?"
                    + "appid="
                    // +"wx26324987ce00ca0a"
                    + APP_ID
                    + "&secret="
                    // + "34dbd9a49057c386f36dda0d8d6100fd"
                    + APP_SE
                    + "&code="
                    + code
                    + "&grant_type=authorization_code";
        StartCoroutine(HttpGetUserInfo(wxurl));
    }


    IEnumerator HttpGetUserInfo(string url)
    {
       WWW data=new WWW(url);
       yield return data;
       if(data.error==null)
       {
           Debug.Log("DATATEXT "+data.text);
            // WechatUserInfo(data.text);
            // Hashtable info = data.text as Hashtable;
            WeChatAccessToken accessToken = LitJson.JsonMapper.ToObject<WeChatAccessToken>(data.text);
            string userUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" +accessToken.access_token + "&openid=" + accessToken.openid;
            StartCoroutine("SetUserInfo", userUrl);
        }
        else
       {
            yield return null;
           Debug.LogError("DATAERROR "+data.error);
       }
       StopCoroutine("HttpGetUserInfo");
    }

    IEnumerator SetUserInfo(string url)
    {
        WWW data = new WWW(url);
        yield return data;
       // LogText.text = data.text;
        if (data.error == null)
        {
            LOG.Log("DATATEXT " + data.text);
            WechatUserInfo(data.text);
        }
        else
        {
            Debug.LogError("DATAERROR " + data.error);
        }
        StopCoroutine("HttpGetUserInfo");
    }

    public void SMSLogin()
    {
        if (network == null)
            network = NetworkManager.Instance;
        C2S_LoginMsg msg = new C2S_LoginMsg();
        msg.openId = GameData.Inst.SmsInfo.country + GameData.Inst.SmsInfo.phone;
        msg.nickName = "玩家" + GameData.Inst.SmsInfo.phone.Remove(0, 7);
        if (GameData.Inst.wxUserInfo!=null)
            msg.sex = GameData.Inst.wxUserInfo.sex;
        Packet packet = new Packet(Protocal.LOGIN, msg);
        if (network != null)
            network.EmitPacket(packet);
    }

}
#endif