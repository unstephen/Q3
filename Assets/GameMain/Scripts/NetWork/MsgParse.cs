using System;
using System.Text;

public class MsgParse
{
    public static void PushByte(byte b,ref byte[] curArray)
    {
        byte[] combimeArray = new byte[curArray.Length+1];
        combimeArray[curArray.Length] = b;
        Buffer.BlockCopy(curArray,0,combimeArray,0,curArray.Length);
        curArray = combimeArray;
    }

    public static void PushString(string str,ref byte[] curArray)
    {
        byte[] strArray = Encoding.UTF8.GetBytes(str);
        byte length = (byte)strArray.Length;
        byte[] combimeArray = new byte[curArray.Length+1+strArray.Length];
        combimeArray[curArray.Length] = length;
        Buffer.BlockCopy(curArray,0,combimeArray,0,curArray.Length);
        Buffer.BlockCopy(strArray,0,combimeArray,curArray.Length+1,length);
        curArray = combimeArray;
    }

    public static void PushInt(int number,ref byte[] curArray)
    {
        byte[] intArray = BitConverter.GetBytes(number);
        byte[] combimeArray = new byte[curArray.Length+4];
        Buffer.BlockCopy(curArray,0,combimeArray,0,curArray.Length);
        Buffer.BlockCopy(intArray,0,combimeArray,curArray.Length,4);
        curArray = combimeArray;
    }


    public static byte PopByte(ref byte[] data)
    {
        byte b = data[0];
        byte[] combimeArray = new byte[data.Length-1];
        Buffer.BlockCopy(data,1,combimeArray,0,data.Length-1);
        data = combimeArray;
        return b;
    }

    public static string PopString(ref byte[] data)
    {
        byte length = data[0];
        byte[] stringArray = new byte[length];
        Buffer.BlockCopy(data,1,stringArray,0,length);
		
        byte[] newData = new byte[data.Length-1-length];
        Buffer.BlockCopy(data,1+length,newData,0,data.Length-1-length);
        data = newData;
        return Encoding.UTF8.GetString(stringArray);
    }

    public static int PopInt(ref byte[] data)
    {
        var ret = BitConverter.ToInt32(data,0);
        byte[] newData = new byte[data.Length-4];
        Buffer.BlockCopy(data,4,newData,0,data.Length-4);
        data = newData;
        return ret;
    }

}