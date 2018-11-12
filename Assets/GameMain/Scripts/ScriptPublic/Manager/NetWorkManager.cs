using LitJson;
using System;
using System.IO;
using System.Net;
using System.Text;

public class NetWorkManager : MonoSingleton<NetWorkManager>
{

    // 构建http连接
    /// <summary>
    /// Get数据接口
    /// </summary>
    /// <param name="getUrl">接口地址</param>
    /// <returns></returns>
    private static string GetWebRequest()
    {
        string responseContent = "";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(GameConst.httpUrl);
        request.ContentType = "application/json";
        request.Method = "GET";

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //在这里对接收到的页面内容进行处理
        using (Stream resStream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
            {
                responseContent = reader.ReadToEnd().ToString();
            }
        }
        return responseContent;
    }
    /// <summary>
    /// Post数据接口
    /// </summary>
    /// <param name="postUrl">接口地址</param>
    /// <param name="paramData">提交json数据</param>
    /// <param name="dataEncode">编码方式(Encoding.UTF8)</param>
    /// <returns></returns>
    private static string PostWebRequest(string paramData, Encoding dataEncode)
    {
        string responseContent = string.Empty;
        try
        {
            byte[] byteArray = dataEncode.GetBytes(paramData); //转化
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(GameConst.httpUrl));
            webReq.Method = "POST";
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = byteArray.Length;
            using (Stream reqStream = webReq.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);//写入参数
                                                                //reqStream.Close();
            }
            using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
            {
                //在这里对接收到的页面内容进行处理
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    responseContent = sr.ReadToEnd().ToString();
                }
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return responseContent;
    }

}
