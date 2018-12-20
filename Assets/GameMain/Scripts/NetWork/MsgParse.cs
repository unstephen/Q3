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
        //MsgParse.ReverseBytes(strArray);
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
        MsgParse.ReverseBytes(intArray);
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
    //翻转byte数组
    public static void ReverseBytes(byte[] bytes)
    {
        byte tmp;
        int len = bytes.Length;

        for (int i = 0; i < len / 2; i++ )
        {
            tmp = bytes[len - 1 - i];
            bytes[len - 1 - i] = bytes[i];
            bytes[i] = tmp;
        }
    }

//规定转换起始位置和长度
    public static void ReverseBytes(byte[] bytes, int start, int len)
    {
        int end = start + len - 1;
        byte tmp;
        int i = 0;
        for (int index = start; index < start + len/2; index++,i++)
        {
            tmp = bytes[end - i];
            bytes[end - i] = bytes[index];
            bytes[index] = tmp;
        }
    }

// 翻转字节顺序 (16-bit)
    public static UInt16 ReverseBytes(UInt16 value)
    {
        return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
    }


// 翻转字节顺序 (32-bit)
    public static UInt32 ReverseBytes(UInt32 value)
    {
        return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
               (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
    }


// 翻转字节顺序 (64-bit)
    public static UInt64 ReverseBytes(UInt64 value)
    {
        return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
               (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
               (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
               (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
    }
 
    public static string PopString(ref byte[] data)
    {
        byte length = data[0];
        byte[] stringArray = new byte[length];
        Buffer.BlockCopy(data,1,stringArray,0,length);
		
        byte[] newData = new byte[data.Length-1-length];
        Buffer.BlockCopy(data,1+length,newData,0,data.Length-1-length);
        data = newData;
       // MsgParse.ReverseBytes(stringArray);
        return Encoding.UTF8.GetString(stringArray);
    }

    public static int PopInt(ref byte[] data)
    {
        byte[] intArray = new byte[4];
        Buffer.BlockCopy(data,0,intArray,0,4);
        MsgParse.ReverseBytes(intArray);
        byte[] newData = new byte[data.Length-4];
        Buffer.BlockCopy(data,4,newData,0,data.Length-4);
        data = newData;
        return BitConverter.ToInt32(intArray,0);
    }

}