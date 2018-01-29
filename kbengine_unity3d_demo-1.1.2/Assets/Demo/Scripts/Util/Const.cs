using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class Const
{

    public static bool Update = false; //是否开启热更
    public static bool OPEN_LOG = true;//是否打开日志
    public static bool OPEN_PROFILER = false;//是否开启性能检测
    public static string ProjectName = "Demo";
    public static string Update_Url = "";//热更地址

    private static bool _ISEDITOR = false;//是否是编辑器模式
    public static string endname = ".unity3d";
    public static bool IS_EDITOR_MODE
    {

        get
        {            
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    _ISEDITOR = false;
                }
                else
                {
                    _ISEDITOR = true;
                }            
            return _ISEDITOR;
        }
    }

    private static string dataPath ;//包内文件解压目的地址

    public static string DataPath
    {
        get
        {
            if (string.IsNullOrEmpty(dataPath))
            {
                if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    dataPath = Application.persistentDataPath + "/";
                }
                else
                {
                    dataPath = Application.dataPath + "/../";
                }
            }
            return dataPath;
        }
    }
    private static string _AppContentPath ;//包内需要解压文件的地址

    public static string AppContentPath
    {
        get {
            if (string.IsNullOrEmpty(_AppContentPath))
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        _AppContentPath = "jar:file://" + Application.dataPath + "!/assets/";
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _AppContentPath = Application.dataPath + "/Raw/";
                        break;
                }
            }
            return _AppContentPath;
        }
        
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
    private static string _osDir;

    public static string osDir
    {
        get
        {
            if (string.IsNullOrEmpty(_osDir))
            {
#if UNITY_IOS
            _osDir="iOS";
#endif
#if UNITY_ANDROID
            _osDir="ANDROID";
#endif
#if UNITY_STANDALONE_WIN
                _osDir = "Windows";
#endif
            }
            return _osDir;
        }
    }
}
