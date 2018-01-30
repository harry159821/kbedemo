using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

public static class Util
{

    public static int Int(object o)
    {

        return Convert.ToInt32(o);

    }



    public static float Float(object o)
    {

        return (float)Math.Round(Convert.ToSingle(o), 2);

    }



    public static long Long(object o)
    {

        return Convert.ToInt64(o);

    }



    public static int Random(int min, int max)
    {

        return UnityEngine.Random.Range(min, max);

    }



    public static float Random(float min, float max)
    {

        return UnityEngine.Random.Range(min, max);

    }

    /// <summary>

    /// 计算字符串的MD5值

    /// </summary>

    public static string md5(string source)
    {

        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        byte[] data = System.Text.Encoding.UTF8.GetBytes(source);

        byte[] md5Data = md5.ComputeHash(data, 0, data.Length);

        md5.Clear();



        string destString = "";

        for (int i = 0; i < md5Data.Length; i++)
        {

            destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');

        }

        destString = destString.PadLeft(32, '0');

        return destString;

    }



    /// <summary>

    /// 计算文件的MD5值

    /// </summary>

    public static string md5file(string file)
    {

        try
        {

            FileStream fs = new FileStream(file, FileMode.Open);

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();

            byte[] retVal = md5.ComputeHash(fs);

            fs.Close();



            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < retVal.Length; i++)
            {

                sb.Append(retVal[i].ToString("x2"));

            }

            return sb.ToString();

        }
        catch (Exception ex)
        {

            throw new Exception("md5file() fail, error:" + ex.Message);

        }

    }
    /**
     * 递归设置层级
     * 
     */
    public static void SetLayerRecursively(GameObject obj, LAYER layer)
    {
        if (obj != null)
        {
            obj.layer = (int)layer;

            Transform tran = obj.transform;
            for (int index = 0; index != tran.childCount; ++index)
            {
                Transform child = tran.GetChild(index);
                SetLayerRecursively(child.gameObject, layer);
            }
        }
    }
    public static void Log(string str)

    {

        if (Const.OPEN_LOG)
            Debug.Log(str);

    }



    public static void LogWarning(string str)

    {
        if (Const.OPEN_LOG)          
        Debug.LogWarning(str);
    }
    public static void LogError(string str)

    {
        if (Const.OPEN_LOG)
            Debug.LogError(str);

    }
    public static byte[] Utf8ToByte(object utf8)

    {

        return System.Text.Encoding.UTF8.GetBytes((string)utf8);

    }



    public static string ByteToUtf8(byte[] bytes)

    {

        return System.Text.Encoding.UTF8.GetString(bytes);

    }



    public static void ArrayCopy(byte[] srcdatas, long srcLen, byte[] dstdatas, long dstLen, long len)

    {

        Array.Copy(srcdatas, srcLen, dstdatas, dstLen, len);

    }
    public static void createFile(string path, string name, byte[] datas)

    {

        deleteFile(path, name);

        Log("createFile: " + path + "/" + name);

        FileStream fs = new FileStream(path + "/" + name, FileMode.OpenOrCreate, FileAccess.Write);

        fs.Write(datas, 0, datas.Length);

        fs.Close();

        fs.Dispose();

    }



    public static byte[] loadFile(string path, string name, bool printerr)

    {

        FileStream fs;



        try

        {

            fs = new FileStream(path + "/" + name, FileMode.Open, FileAccess.Read);

        }

        catch (Exception e)

        {

            if (printerr)

            {

                LogError("loadFile: " + path + "/" + name);

                LogError(e.ToString());

            }



            return new byte[0];

        }



        byte[] datas = new byte[fs.Length];

        fs.Read(datas, 0, datas.Length);

        fs.Close();

        fs.Dispose();



        Util.Log("loadFile: " + path + "/" + name + ", datasize=" + datas.Length);

        return datas;

    }



    public static void deleteFile(string path, string name)

    {

        //Dbg.DEBUG_MSG("deleteFile: " + path + "/" + name);



        try

        {

            File.Delete(path + "/" + name);

        }

        catch (Exception e)

        {

            LogError(e.ToString());

        }

    }



    public static string bytesToString(byte[] bytes)

    {

        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        return encoding.GetString(bytes);

    }



    public static byte[] stringToBytes(string str)

    {

        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

        return encoding.GetBytes(str);

    }
    static LuaClient _Client;
    public static LuaClient Client
    {
        get
        {
            return _Client;
        }
        set
        {
            _Client = value;
        }
    }
}

