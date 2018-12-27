using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LoginData{
    static private LoginData _instance;
    static public LoginData Instance {
        get;private set;
    }

    public LoginData() {
        _instance = this;
    }

    public WechatUserInfo wxUserInfo;
}
/// <summary>
/// 微信
/// </summary>
public class WeChatAccessToken {
    public string access_token;
    public int expires_in;
    public string refresh_token;
    public string openid;
    public string scope;
}
/// <summary>
/// 微信
/// </summary>
public class WechatUserInfo {
    public string openid;
    public string nickname;
    public int sex;
    public string province;
    public string city;
    public string country;
    public string headimgurl;
    public List<string> privilege;
    public string unionid;
}
public class WeChat:MonoBehaviour
{
      public static event Action<WechatUserInfo> WxLogin;
    [NonSerialized]
    public string APP_ID= "wxe8355f09eacfc7dd";
    [NonSerialized]
    public string APP_SE= "6f0a9ae684b5fd28d5427d1742173bcd";
    static private WeChat _instance;
    static public WeChat Instance {
        get;private set;
    }
    public Text text;
#if UNITY_ANDROID  
    AndroidJavaClass jc;
    AndroidJavaObject jo;
#endif


    void Awake() { 
#if UNITY_ANDROID && !UNITY_EDITOR  //111111
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
#endif

    }

    /// <summary>
    /// 安卓回掉参数
    /// </summary>
    /// <param name="UnityGO"></param>
    /// <param name="args"></param>
    public void Init(string UnityGo, string callBack)
    {
#if UNITY_ANDROID
        jo.Call("Init", UnityGo, callBack);
#endif
    }

    public void WechatLogin()
    {
        Init(gameObject.name, "onWechatResp");
     
#if UNITY_ANDROID
            jo.Call("WechatLogin");
#endif
    }

    public void WechatToken(string info)
    {
        // Debug.Log("token "+info);   
#if UNITY_ANDROID
        WeChatAccessToken accessToken = LitJson.JsonMapper.ToObject<WeChatAccessToken>(info);
        string[] str = new string[] { accessToken.access_token, accessToken.openid };
        jo.Call("GetUserInfoReq", str);
#endif

    }

    public void WechatUserInfo(string info)
    {
        Debug.Log("userinfo " + info);
        LoginData.Instance.wxUserInfo = LitJson.JsonMapper.ToObject<WechatUserInfo>(info);
        WxLogin(LoginData.Instance.wxUserInfo);
    }

    public void onWechatResp(string code)//AS代码调用
    {
        Debug.Log(code);
        WeChatResponse(code);
    }
    public void WeChatResponse(string code)
    {

        string wxurl = "https://api.weixin.qq.com/sns/oauth2/access_token?"
                    + "appid="
                    + APP_ID
                    + "&secret="
                    + APP_SE
                    + "&code="
                    + code
                    + "&grant_type=authorization_code";
        StartCoroutine(HttpGetUserInfo(wxurl));
    }
    IEnumerator HttpGetUserInfo(string url)
    {
        WWW data = new WWW(url);
        yield return data;
        if (data.error == null)
        {
            text.text = "DATATEXT " + data.text;
            // WechatUserInfo(data.text);
            // Hashtable info = data.text as Hashtable;
            WeChatAccessToken accessToken = LitJson.JsonMapper.ToObject<WeChatAccessToken>(data.text);
            string userUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken.access_token + "&openid=" + accessToken.openid;
            StartCoroutine("SetUserInfo", userUrl);

            RoleData role = GameManager.Instance.GetRoleData();
            role.token.SetValueAndForceNotify(accessToken.access_token);
        }
        else
        {
            yield return null;
            text.text = "DATAERROR " + data.error;
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
            text.text= "DATATEXT " + data.text;
            WechatUserInfo(data.text);
        }
        else
        {
            text.text =  "DATAERROR " + data.error;
        }
        StopCoroutine("HttpGetUserInfo");
    }




    #region 截图
    /// <summary>
    /// 相机截图
    /// </summary>
    /// <param name="camera">"被截图的相机"</param>
    public void CaptureCamera() {

        Camera camera = Camera.main;
        Rect rect = new Rect( 0, 0, Screen.width, Screen.height );
        RenderTexture old = RenderTexture.active;
        RenderTexture rt = new RenderTexture( ( int )rect.width, ( int )rect.height, 0 );
        camera.targetTexture = rt;
        camera.Render();
        // 激活这个rt, 并从中中读取像素  
        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D( ( int )rect.width, ( int )rect.height, TextureFormat.RGB24, false );
        screenShot.ReadPixels( rect, 0, 0 );
        screenShot.Apply();
        camera.targetTexture = null;
        RenderTexture.active = old; // JC: added to avoid errors  
        GameObject.Destroy( rt );

        byte[] bytes = screenShot.EncodeToPNG();
        screenShot.Compress( false );

        ShareScreenShot( Convert.ToBase64String( bytes ) );
    }

    public void ScreenshotEx() {
        StopCoroutine( "DoShareRecord" );
        StartCoroutine( "DoShareRecord" );
    }
    IEnumerator DoShareRecord() {
        yield return new WaitForEndOfFrame();
        Rect rect = new Rect( 0, 0, Screen.width, Screen.height );
        Texture2D screenShot = new Texture2D( ( int )rect.width, ( int )rect.height, TextureFormat.RGB24, false );
        screenShot.ReadPixels( rect, 0, 0 );
        screenShot.Apply();
        screenShot.Compress( false );
        byte[] bytes = screenShot.EncodeToPNG();
        ShareScreenShot( Convert.ToBase64String( bytes ) );
    }
    //截图
    public void ShareScreenShot(string strbaser64) {
#if UNITY_ANDROID
        Init(transform.name, "OnShareSuccess");
        jo.Call("shareRecord", strbaser64);
#endif
    } 
    #endregion


    public Texture2D Icon;
    /// <summary>
    /// 分享
    /// </summary>
    /// <param name="isInFriendCircle"></param>
    /// <param name="title"></param>
    /// <param name="webUrl"></param>
    /// <param name="str"></param>
    /// <param name="t2d"></param>
    public void ShareFriend(bool isInFriendCircle, string title, string webUrl, string content, Texture2D t2d)//true=朋友圈
    {
        if (t2d == null) {
            t2d = Icon;
        }
        byte[] bytesArr = null;
        t2d.Compress( false );
        bytesArr = t2d.EncodeToPNG();// JPG
        string strbaser64 = Convert.ToBase64String(bytesArr);
#if UNITY_ANDROID
        Init(transform.name, "OnShareResp");
        jo.Call("shareFriend", isInFriendCircle, title, webUrl, content, strbaser64);
#endif
    }

}