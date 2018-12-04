//using Assets.Script.GameScenes.Mahjong;
using System;
using System.Collections;
using UnityEngine;
using LitJson;

public class Response
{

    public int code;
    public int state = 0;
    public string args;
    public Response()
    {

    }
    public Response(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            Debug.Log("json为空或者null");
        }
        else
        {
            JsonData jsonData = JsonMapper.ToObject(data);
            code = int.Parse(JsonMapper.ToJson(jsonData["code"]));
            args = JsonMapper.ToJson(jsonData["args"]);
        }

    }

    public T ToObject<T>( ) {
        return JsonMapper.ToObject<T>( args );
    }

}
