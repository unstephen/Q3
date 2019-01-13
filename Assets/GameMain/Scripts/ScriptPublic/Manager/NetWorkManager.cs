using GamePlay;
using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using GameFramework;
using GameFramework.Network;
using ProtoBuf;
using UnityEngine;
using UnityGameFramework.Runtime;
using WebSocketSharp;
using ErrorEventArgs = WebSocketSharp.ErrorEventArgs;
using GameEntry = GamePlay.GameEntry;
using NetworkErrorEventArgs = GameFramework.Network.NetworkErrorEventArgs;

public class NetWorkManager : MonoSingleton<NetWorkManager>
{
    private LitJsonHelper jsonHelper = null;

    public override void Init()
    {
        base.Init();
//        connected = false;
//        wsConnected = true;
        //实例化jsonhelper
        jsonHelper = new LitJsonHelper();
    }

    /// <summary>
    /// Post Http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="postData">传输数据</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="contentType">媒体格式</param>
    /// <param name="encode">编码</param>
    /// <returns>泛型集合</returns>
    //public static List<T> PostAndRespList<T>(string url, string postData, int timeout = 1000, string contentType = "application/json;", string encode = "UTF-8")
    //{
    //    if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode) && !string.IsNullOrEmpty(contentType) && postData != null)
    //    {
    //        // webRequest.Headers.Add("Authorization", "Bearer " + "SportApiAuthData");
    //        HttpWebResponse webResponse = null;
    //        Stream responseStream = null;
    //        Stream requestStream = null;
    //        StreamReader streamReader = null;
    //        try
    //        {
    //            string respstr = GetStreamReader(url, responseStream, requestStream, streamReader, webResponse, timeout, encode, postData, contentType);
    //            //return JsonHelper.JsonDeserialize<List<T>>(respstr);
    //            return jsonHelper.ToObject<T>(respstr);

    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //        finally
    //        {
    //            if (responseStream != null) responseStream.Dispose();
    //            if (webResponse != null) webResponse.Dispose();
    //            if (requestStream != null) requestStream.Dispose();
    //            if (streamReader != null) streamReader.Dispose();
    //        }
    //    }
    //    return null;
    //}

    /// <summary>
    /// Post Http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="postData">传输数据</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="contentType">媒体格式</param>
    /// <param name="encode">编码</param>
    /// <returns>泛型集合</returns>
    public T PostAndRespSignle<T>(string url, int timeout = 1000, string postData = "", string contentType = "application/json;", string encode = "UTF-8")
    {
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode) && !string.IsNullOrEmpty(contentType) && postData != null)
        {
            // webRequest.Headers.Add("Authorization", "Bearer " + "SportApiAuthData");
            //HttpWebResponse webResponse = null;
            Stream responseStream = null;
            Stream requestStream = null;
            StreamReader streamReader = null;
            try
            {
                string respstr = GetStreamReader(url, responseStream, requestStream, streamReader, timeout, encode, postData, contentType);
                return JsonMapper.ToObject<T>(respstr);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                //if (webResponse != null) webResponse.Dispose();
                if (requestStream != null) requestStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
            }
        }
        return default(T);
    }

    /// <summary>
    /// Post Http请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="postData"></param>
    /// <param name="timeout"></param>
    /// <param name="contentType"></param>
    /// <param name="encode"></param>
    /// <returns>响应流字符串</returns>
    public string PostAndRespStr(string url, int timeout = 5000, string postData = "", string contentType = "application/json;", string encode = "UTF-8")
    {
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode) && !string.IsNullOrEmpty(contentType) && postData != null)
        {
            //HttpWebResponse webResponse = null;
            Stream responseStream = null;
            Stream requestStream = null;
            StreamReader streamReader = null;
            try
            {

                return GetStreamReader(url, responseStream, requestStream, streamReader, timeout, encode, postData, contentType);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                //if (webResponse != null) webResponse.Dispose();
                if (requestStream != null) requestStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
            }
        }
        return null;
    }

    private string GetStreamReader(string url, Stream responseStream, Stream requestStream, StreamReader streamReader , int timeout, string encode, string postData, string contentType)
    {
        byte[] data = Encoding.GetEncoding(encode).GetBytes(postData);
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "POST";
        webRequest.ContentType = contentType + ";" + encode;
        webRequest.ContentLength = data.Length;
        webRequest.Timeout = timeout;
        requestStream = webRequest.GetRequestStream();
        requestStream.Write(data, 0, data.Length);
        responseStream = webRequest.GetResponse().GetResponseStream();
        if (responseStream == null) { return ""; }
        streamReader = new StreamReader(responseStream, Encoding.GetEncoding(encode));
        return streamReader.ReadToEnd();
    }

    /// <summary>
    /// Post文件流给指定Url
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="filePath">文件路径</param>
    /// <returns>响应流字符串</returns>
    public string PostFile(string url, string filePath, string contentType = "application/octet-stream", string encode = "UTF-8")
    {
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode) && !string.IsNullOrEmpty(contentType) && !string.IsNullOrEmpty(filePath))
        {

            Stream requestStream = null;
            Stream responseStream = null;
            StreamReader streamReader = null;
            FileStream fileStream = null;
            try
            {
                // 设置参数
                HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
                webRequest.AllowAutoRedirect = true;
                webRequest.Method = "POST";
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                webRequest.ContentType = "multipart/form-data;charset=" + encode + ";boundary=" + boundary;
                byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");//消息开始
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");//消息结尾
                var fileName = filePath.Substring(filePath.LastIndexOf("/") + 1);
                //请求头部信息
                string postHeader = string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:{1}\r\n\r\n", fileName, contentType);
                byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileByteArr = new byte[fileStream.Length];
                fileStream.Read(fileByteArr, 0, fileByteArr.Length);
                fileStream.Close();
                requestStream = webRequest.GetRequestStream();
                requestStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                requestStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                requestStream.Write(fileByteArr, 0, fileByteArr.Length);
                requestStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                requestStream.Close();
                responseStream = webRequest.GetResponse().GetResponseStream();//发送请求，得到响应流
                if (responseStream == null) return string.Empty;
                streamReader = new StreamReader(responseStream, Encoding.UTF8);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
        return null;

    }

    /// <summary>
    /// Get http请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="timeout">超时时间</param>
    /// <param name="encode">编码</param>
    /// <returns>返回单个实体</returns>
    public T GetSingle<T>(string url, int timeout = 5000, string encode = "UTF-8") where T : Http_MsgBase,new()
    {
        //HttpWebRequest对象
        //HttpClient->dudu 调用预热处理
        //Stream—>Model

        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode))
        {
            Stream responseStream = null;
            StreamReader streamReader = null;
            //WebResponse webResponse = null;
            try
            {
                string respStr = GetRespStr(url, responseStream, streamReader, timeout, encode);
               // string respStr = "{\"code\":0,\"errMsg\":\"\",\"data\":{\"user_id\":\"123\",\"access_token\":\"test_token\"}}";
//                var respJson =  JsonMapper.ToObject(respStr);
//                var test = respJson["data"].ToJson();
//                var ret = JsonMapper.ToObject<T>(respJson["data"].ToJson());
//                ret.code = Convert.ToInt32(respJson["code"]);
//                ret.errMsg = respJson["errMsg"].ToString();
//                return ret;
                if (respStr.StartsWith("<br"))
                {
                    throw new ArgumentNullException (respStr);;
                }
                return JsonMapper.ToObject<T>(respStr);
            }
            catch (Exception ex)
            {
                Log.Error("http返回錯誤："+ex);
            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
                //if (webResponse != null) webResponse.Dispose();
            }
        }
        return default(T);
    }

    /// <summary>
    ///  Get http请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="timeout"></param>
    /// <param name="encode"></param>
    /// <returns>响应流字符串</returns>
    public string GetResponseString(string url, int timeout = 5000, string encode = "UTF-8")
    {
        if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(encode))
        {
            Stream responseStream = null;
            StreamReader streamReader = null;
            //WebResponse webResponse = null;
            try
            {
                return GetRespStr(url, responseStream, streamReader, timeout, encode);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (responseStream != null) responseStream.Dispose();
                if (streamReader != null) streamReader.Dispose();
                //if (webResponse != null) webResponse.Dispose();
            }
        }
        return null;
    }

    private string GetRespStr(string url, Stream responseStream, StreamReader streamReader, int timeout, string encode)
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        webRequest.Method = "GET";
        webRequest.Timeout = timeout;
        webRequest.ContentType = "application/json;charset=utf-8";
        //这里是最开始的序列化代码
        //responseStream = webRequest.GetResponse().GetResponseStream();
        //if (responseStream == null) { return ""; }
        //streamReader = new StreamReader(responseStream, Encoding.GetEncoding(encode));

        //return streamReader.ReadToEnd();

        string Result = string.Empty;
        using (responseStream = webRequest.GetResponse().GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                Result = reader.ReadToEnd().ToString();
            }
        }

        //这里我本想处理一下反转义，但没有成功
        string tempStr = System.Text.RegularExpressions.Regex.Unescape(Result);
        ////第二种直接强行删除字符串中的“\”,但也没有成功
        //string tempStr = Result.Replace("\\", string.Empty);
        ////第三种方法就是先将\“一起替换成其他字符（例如￥、#等），然后在强制转换回来
        //string tempStr1 = Result.Replace("\*"， "#");
        //string tempStr = tempStr1.Replace("#", "\*");

        return Result;
    }

    public string CreateGetUrl(string url, List<string> paramList)
    {
        string temp = url;

        for (int i = 0; i < paramList.Count; ++i)
        {
            if (i == paramList.Count - 1)
            {
                temp += string.Format("{0}", paramList[i]);
            }
            else
            {
                temp += string.Format("{0}&", paramList[i]);
            }
        }
        Debug.Log(temp);
        return temp;
    }

    public void SetHttpDataByType(string virtualPath)
    {

    }

    public T CreateGetMsg<T>(string vritualPath, List<string> getStrs) where T : Http_MsgBase,new()
    {
        string tempUrl = GameConst.httpUrl + vritualPath + "?";

        return GetSingle<T>(CreateGetUrl(tempUrl, getStrs), 1000);
    }

    public string CreatePostString(List<string> postData)
    {
        StringBuilder buffer = new StringBuilder();

        int i = 0;
        foreach(string item in postData)
        {
            if (i > 0)
            {
                buffer.AppendFormat("&{0}", item);
            }
            else
            {
                buffer.AppendFormat("{0}", item);
            }
            i++;
        }

        return buffer.ToString();
    }

    public T CreatePostMsg<T>(string vritualPath, List<string> getStrs) where T : Http_MsgBase
    {
        string tempUrl = GameConst.httpUrl + vritualPath + "?";

        return PostAndRespSignle<T>(CreateGetUrl(tempUrl, getStrs), 1000);
    }
#region WebSocket
//    public int reconnectDelay = 5;
//    private volatile bool connected;
//    //游戏服socket
//    public WebSocket gamews;
//    private object eventQueueLock;
//    private Queue<Response> eventQueue;
//    
//    //游戏服
//    public volatile bool gwsConncted;
//    private volatile bool wsConnected;
//    
//    private Thread socketThread;
//    private Thread pingThread;
//    private Thread gameSocketThread;
//    private WebSocket ws;
//    //重连
//    private Action reconnectHandler;
//    public Action mainReconnectHandler;
//    
//    public void CreateGameSocket(string uri,Action recall=null)
//    {
//        if (gamews!=null)
//            gamews.Close( );    
//        gamews = new WebSocket(uri, "default-protocol");
//        reconnectHandler=recall;
//        gamews.OnOpen += OnGameOpen;
//        gamews.OnMessage += HandleMessage;
//        gamews.OnError += OnError;
//        gamews.OnClose += OnGameClose;
//        GameConnect();
//           
//    }
//
//    private void GameConnect()
//    {           
//        gwsConncted = true;
//        if( gameSocketThread != null )
//            gameSocketThread.Abort( );
//        gameSocketThread = new Thread(RunGameSocketThread);
//        gameSocketThread.Start(gamews);
//    }
//    
//    private void RunGameSocketThread(object obj)
//    {
//        WebSocket webSocket = (WebSocket)obj;
//        while (gwsConncted)
//        {
//            if (webSocket.IsConnected)
//            {
//                Thread.Sleep(reconnectDelay);
//            }
//            else
//            {
//                webSocket.Connect();
//            }
//        }
//           
//        webSocket.Close();
//    }
//    
//    
//    private void HandleMessage(object sender, MessageEventArgs message)
//    {
//        Log.Info( "Recieve " + message.Data + "---------------" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") );
//        Response resp = new Response(message.Data);
//
//        try
//        {
//            GameManager.Instance.ParseMessage(resp.code, resp.args);
//            lock (eventQueueLock)
//            {
//                eventQueue.Enqueue(resp);   
//            }
//        }
//        catch (Exception exce)
//        {
//            Debug.LogError(exce.ToString());
//
//        }
//    }
//    
//    private bool IsGameSocketOpened;
//    private void OnGameOpen(object sender, EventArgs e)
//    {
//        Log.Info( "---------OnGameSocketConnected" );
//        IsGameSocketOpened = true;
//    }
//    
//    private void OnError(object sender, ErrorEventArgs e)
//    {
//        Debug.LogError(e.Message.ToString());
//    }
//    
//    private void OnGameClose(object sender, CloseEventArgs e)
//    {
//        Debug.Log("Socket closed code "+e.Code+" "+e.Reason);  
//    }
//    public void OnDestroy()
//    {
//        Unload( );            
//    }
//
//    public void OnApplicationQuit()
//    {
//        Close();
//    }
//    
//    public void Unload()
//    {
//        CloseGameSocket();
//        Close();
//        if( gameSocketThread != null )
//            gameSocketThread.Abort( );
//        if( socketThread != null )
//            socketThread.Abort( );
//        if( pingThread != null )
//            pingThread.Abort( );
//    }
//    
//    public void CloseGameSocket()
//    {
//        Log.Info( "------------------CloseGameSocket" );
//        gwsConncted=false;
//        IsGameSocketOpened = false;
//    }
//    
//    public void Close()
//    {
//        Log.Info( "------------------Close" );
//        connected = false;
//        //IsMainReconnected = false;
//    }

    #endregion
    public INetworkChannel channel;

    public void CreateChanel()
    {
        if (channel == null)
        {
            var helper = new Q3NetworkHelper();
            helper.PacketHeaderLength = 2;
            channel = GameEntry.Network.CreateNetworkChannel("q3", helper);
         
        }
        IPAddress ipAddress = IPAddress.Parse(GameConst.ipadress); 
        channel.Connect(ipAddress,GameConst.tcp_port);
        channel.ReceiveBufferSize = 1024;
        channel.SendBufferSize = 1024;
        channel.HeartBeatInterval = 3600 * 24;
    }

    public void CloseChanel()
    {
        if (channel != null)
        {
            channel.Close();
            GameEntry.Network.DestroyNetworkChannel("q3");
            channel = null;
        }
    }
    /// <summary>
    /// Q3 TCP的发送消息函数
    /// </summary>
    /// <param name="id">协议id,参看Protocal.cs</param>
    /// <param name="args">若干参数，参看MsgParse来Push参数</param>
    public void Send(byte id,params object[] args)
    {
        Q3Packet msg = new Q3Packet();
        byte[] buffer = new byte[0];
        MsgParse.PushByte(id,ref buffer);
        foreach (var arg in args)
        {
            if (arg is byte)
            {
                MsgParse.PushByte((byte)arg,ref buffer);
            }
            else if (arg is string)
            {
                MsgParse.PushString((string)arg,ref buffer);
            }
            else if (arg is int)
            {
                MsgParse.PushInt((int)arg,ref buffer);
            }
        }
       
        msg.SetArgs(buffer);
        channel.Send(msg);
        Log.Debug("->>>>>>>>>>>>{0}",id);
    }

    public class Q3NetworkHelper : INetworkChannelHelper
    {
        public void Initialize(INetworkChannel networkChannel)
        {
            networkChannel.RegisterHandler(new RecvHandler());
        }

        public void Shutdown()
        {
            Log.Info("Q3NetworkHelper Shutdown");
        }

        public bool SendHeartBeat()
        {
            return false;
        }

        public bool Serialize<T>(T packet, Stream destination) where T : GameFramework.Network.Packet
        {
           
            var c2SPacket = (object) packet as Q3Packet;
            var dataArray = c2SPacket.args;
            byte[] sendArray = new byte[2+dataArray.Length];
            var sizeArray = BitConverter.GetBytes((short)dataArray.Length);
            MsgParse.ReverseBytes(ref sizeArray);
            sendArray[0] = sizeArray[0];
            sendArray[1] = sizeArray[1];
            
            Buffer.BlockCopy(dataArray,0,sendArray,2,dataArray.Length);
            destination.Write(sendArray, 0, sendArray.Length);
            return true;
        }

        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            Q3PacketHeader header = new Q3PacketHeader();
            try
            {
                byte[] buffer = new byte[2];
                source.Read(buffer, 0, 2);
                MsgParse.ReverseBytes(ref buffer);
                header.PacketLength = BitConverter.ToInt16(buffer, 0);
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            customErrorData = null;
            return header;
        }

        public GameFramework.Network.Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            var ret = new Q3Packet();
            try
            {
                byte[] bytes = new byte[packetHeader.PacketLength];
                source.Read(bytes, 0, packetHeader.PacketLength);


                ret.SetArgs(bytes);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            customErrorData = null;
            return ret;
        }

        public int PacketHeaderLength { get; set; }
    }

    public class Q3PacketHeader : IPacketHeader, IReference
    {
        public int PacketLength { get; set; }
        public void Clear()
        {
            PacketLength = 0;
        }
    }

 
 
}
