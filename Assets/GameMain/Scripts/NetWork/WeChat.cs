using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.UI;

public class LoginData : Singleton<LoginData> {
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
    public string refresh_token;
}

public class WeChat : MonoSingleton<WeChat> {
    public static event Action<WeChatAccessToken> WxLogin;
    [NonSerialized]
    public string APP_ID = "wxe8355f09eacfc7dd";
    [NonSerialized]
    public string APP_SE = "6f0a9ae684b5fd28d5427d1742173bcd";
    //public Text LogText;
#if UNITY_ANDROID    //111111
    AndroidJavaClass jc;
    AndroidJavaObject jo;
#elif UNITY_IOS
        [DllImport ("__Internal")]  
        private static extern void _WechatLogin();  
        [DllImport ("__Internal")]  
        private static extern void shareFriend(bool isInFriendCircle,string title, string webUrl, string des, string base64Data); 
           [DllImport("__Internal")]
    public static extern void inviteFriend(string title, string t2d);
    [DllImport("__Internal")]
    public static extern void iOSShareRecord(string img);//截图分享
     [DllImport("__Internal")]
    public static extern void getNetWorkState();//信号

    [DllImport("__Internal")]
    public static extern void getTime();//时间

    [DllImport("__Internal")]
    public static extern void getiOSBatteryLevel();//电池

    [DllImport("__Internal")]
    public static extern void _copyTextToClipboard(string text);//复制到ios剪切板
    [DllImport ("__Internal")]
     private static extern void wxPay(string mch_id, string prepay_Id, string nonce_str, string timeStamp,string sign);//微信支付
    [DllImport("__Internal")]
    private static extern void opUrl();
     [DllImport("__Internal")]
    private static extern void IOSInit(string UnityGo, string callBack);
#endif

    public override void Init() {
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
    public void Init(string UnityGo, string callBack) {
#if UNITY_ANDROID
        jo.Call( "Init", UnityGo, callBack );
#elif UNITY_IOS
    //    IOSInit(UnityGo, callBack);
#endif
    }

    public void WechatLogin() {
        Init( gameObject.name, "onWechatResp" );

#if UNITY_ANDROID
        jo.Call( "WechatLogin" );
#elif UNITY_IOS
        _WechatLogin();
#endif
    }

    public void WechatToken(string info) {
        // Debug.Log("token "+info);   
#if UNITY_ANDROID
        WeChatAccessToken accessToken = LitJson.JsonMapper.ToObject<WeChatAccessToken>( info );
        string[] str = new string[] { accessToken.access_token, accessToken.openid };
        jo.Call( "GetUserInfoReq", str );
#endif

    }

    public void WechatUserInfo(string info) {
        Debug.Log( "userinfo " + info );
        LoginData.Instance.wxUserInfo = LitJson.JsonMapper.ToObject<WechatUserInfo>( info );
        WxLogin(Token);
    }



    public void IOSWeChatResponse(string code) {
        string furl = ("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APP_ID + "&secret=" + APP_SE + "&code=" + code + "&grant_type=authorization_code");
        StartCoroutine("HttpGetUserInfo", furl);
    }
    public void onWechatResp(string code)//AS代码调用
    {
        Debug.Log( "onWechatResp():" + code );
        WeChatResponse(code);
    }
    public void WeChatResponse(string code) {

        string wxurl = "https://api.weixin.qq.com/sns/oauth2/access_token?"
                    + "appid="
                    + APP_ID
                    + "&secret="
                    + APP_SE
                    + "&code="
                    + code
                    + "&grant_type=authorization_code";
        StartCoroutine( HttpGetUserInfo( wxurl ) );
    }

    public WeChatAccessToken Token {
        get;private set;
    }
    IEnumerator HttpGetUserInfo(string url) {
        WWW data = new WWW( url );
        yield return data;
        if (data.error == null) {
            Debug.Log( "DATATEXT " + data.text );
            Token = LitJson.JsonMapper.ToObject<WeChatAccessToken>( data.text );
            string userUrl = "https://api.weixin.qq.com/sns/userinfo?access_token=" + Token.access_token + "&openid=" + Token.openid;
            StartCoroutine( "SetUserInfo", userUrl );
        }
        else {
            yield return null;
            Debug.LogError( "DATAERROR " + data.error );
        }
        StopCoroutine( "HttpGetUserInfo" );
    }

    IEnumerator SetUserInfo(string url) {
        WWW data = new WWW( url );
        yield return data;
        // LogText.text = data.text;
        if (data.error == null) {
            Debug.Log( "DATATEXT " + data.text );
            WechatUserInfo( data.text );
        }
        else {
            Debug.LogError( "DATAERROR " + data.error );
        }
        StopCoroutine( "HttpGetUserInfo" );
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
    /// <summary>
    /// 旋转图片90°
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public Texture2D TextureRotate(Texture2D texture) {
        Texture2D newTexture = new Texture2D( texture.height, texture.width );
        for (int i = 0; i < texture.width - 1; i++) {
            for (int j = 0; j < texture.height - 1; j++) {
                Color color = texture.GetPixel( i, j );
                newTexture.SetPixel( j, texture.width - 1 - i, color );
            }
        }
        newTexture.Apply();
        return newTexture;
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
        Init( transform.name, "OnShareSuccess" );
        jo.Call( "shareRecord", strbaser64 );
#elif UNITY_IPHONE
        iOSShareRecord( strbaser64 );
#endif
    }
    #endregion
    /// <summary>
    ///  复制到剪贴板  
    /// </summary>
    /// <param name="input"></param>
    public void CopyToClipboard(string input) {
#if UNITY_ANDROID
        // 对Android的调用   com.yunxuanleyou.hl.MainActivity
        AndroidJavaObject androidObject = new AndroidJavaObject( "com.yxwm.dz.MainActivity" );//是你Android项目的<类目录> + <类名>，这个不能写错，写错就无法或取出你要的Android类
        AndroidJavaObject activity = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ).GetStatic<AndroidJavaObject>( "currentActivity" );
        // 复制到剪贴板  
        androidObject.Call( "copyTextToClipboard", activity, input );
#elif UNITY_IPHONE
        //调用clipboard.h中的接口  
       _copyTextToClipboard(input);
        // Debug.LogError ("CopyToClipboard_______"+input);  
#endif
    }

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
        string strbaser64 = Convert.ToBase64String( bytesArr );
#if UNITY_ANDROID
        Init( transform.name, "OnShareResp" );
        jo.Call( "shareFriend", isInFriendCircle, title, webUrl, content, strbaser64 );
#elif UNITY_IPHONE
        IOSInit( transform.name, "OnShareResp");
        shareFriend(isInFriendCircle, title, webUrl, content, strbaser64);
#endif
    }

    /// <summary>
    /// 分享成功回调 通知服务器
    /// </summary>
    /// <param name="code"></param>
    public void OnShareResp(string code)//AS代码调用 
    {
        Debug.Log( "分享回调" + code );
    }

}