using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;


public class GameManager : MonoBehaviour
{
    protected static bool initialize = false;
    private List<string> downloadFiles = new List<string>();

    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
    void Awake()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {

        DontDestroyOnLoad(gameObject);  //防止销毁自己

        CheckExtractResource(); //释放资源
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
    }



    /// <summary>
    /// 释放资源
    /// </summary>
    public void CheckExtractResource()
    {
        bool isExists = Directory.Exists(Const.DataPath) &&
          Directory.Exists(Const.DataPath + "lua/") && File.Exists(Const.DataPath + "files.txt");
        if (isExists)
        {
            StartCoroutine(OnUpdateResource());
            return;   //文件已经解压过了，自己可添加检查文件列表逻辑
        }
        StartCoroutine(OnExtractResource());    //启动释放协成 
    }

    IEnumerator OnExtractResource()
    {
        if (Const.IS_EDITOR_MODE)
        {
            StartCoroutine(OnUpdateResource());
            yield break;
        }
        string dataPath = Const.DataPath;  //数据目录
        string resPath = Const.AppContentPath; //游戏包资源目录

        if (Directory.Exists(dataPath)) Directory.Delete(dataPath, true);
        Directory.CreateDirectory(dataPath);

        string infile = resPath + "files.txt";
        string outfile = dataPath + "files.txt";
        if (File.Exists(outfile)) File.Delete(outfile);

        string message = "正在解包文件:>files.txt";
        Debug.Log(message);

        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;

            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
            yield return 0;
        }
        else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();

        //释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);
        foreach (var file in files)
        {
            string[] fs = file.Split('|');
            infile = resPath + fs[0];  //
            outfile = dataPath + fs[0];
            Debug.Log("正在解包文件:>" + infile);
            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("解包完成");
        yield return new WaitForSeconds(0.1f);

        //释放完成，开始启动更新资源
        StartCoroutine(OnUpdateResource());
    }
    ThreadManager threadManager;
    /// <summary>
    /// 启动更新下载，这里只是个思路演示，此处可启动线程下载更新
    /// </summary>
    IEnumerator OnUpdateResource()
    {
        downloadFiles.Clear();

        if (!Const.Update)
        {
            OnResourceInited();
            yield break;
        }
        threadManager = this.gameObject.AddComponent<ThreadManager>();
        string dataPath = Const.DataPath;  //数据目录
        string url = Const.Update_Url;
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string listUrl = url + "files.txt?v=" + random;
        Debug.LogWarning("LoadUpdate---->>>" + listUrl);

        WWW www = new WWW(listUrl); yield return www;
        if (www.error != null)
        {
            OnUpdateFailed(string.Empty);
            yield break;
        }
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        File.WriteAllBytes(dataPath + "files.txt", www.bytes);

        string filesText = www.text;
        string[] files = filesText.Split('\n');

        string message = string.Empty;
        for (int i = 0; i < files.Length; i++)
        {
            if (string.IsNullOrEmpty(files[i])) continue;
            string[] keyValue = files[i].Split('|');
            string f = keyValue[0];
            string localfile = (dataPath + f).Trim();
            string path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileUrl = url + keyValue[0] + "?v=" + random;
            bool canUpdate = !File.Exists(localfile);
            if (!canUpdate)
            {
                string remoteMd5 = keyValue[1].Trim();
                string localMd5 = Const.md5file(localfile);
                canUpdate = !remoteMd5.Equals(localMd5);
                if (canUpdate) File.Delete(localfile);
            }
            if (canUpdate)
            {   //本地缺少文件
                Debug.Log(fileUrl);
                message = "downloading>>" + fileUrl;
                //处理开始更新ui事件

                BeginDownload(fileUrl, localfile);
                while (!(IsDownOK(localfile))) { yield return new WaitForEndOfFrame(); }
            }
        }
        yield return new WaitForEndOfFrame();
        message = "更新完成!!";
        //处理更新完成ui事件

        OnResourceInited();
    }

    /// <summary>
    /// 是否下载完成
    /// </summary>
    bool IsDownOK(string file)
    {
        return downloadFiles.Contains(file);
    }

    /// <summary>
    /// 线程下载
    /// </summary>
    void BeginDownload(string url, string file)
    {     //线程下载
        object[] param = new object[2] { url, file };

        ThreadEvent ev = new ThreadEvent();
        ev.Key = NotiConst.UPDATE_DOWNLOAD;
        ev.evParams.AddRange(param);
        threadManager.AddEvent(ev, OnThreadCompleted);   //线程下载
    }

    /// <summary>
    /// 线程完成
    /// </summary>
    /// <param name="data"></param>
    void OnThreadCompleted(NotiData data)
    {
        switch (data.evName)
        {
            case NotiConst.UPDATE_DOWNLOAD: //下载一个完成
                downloadFiles.Add(data.evParam.ToString());
                break;
        }
    }

    void OnUpdateFailed(string file)
    {
        string message = "更新失败!>" + file;
        //处理下载失败
    }
    ResourcesManager resourcesManager;
    /// <summary>
    /// 资源初始化结束
    /// </summary>
    public void OnResourceInited()
    {
        //开始加载AssetBundleManifest      
        resourcesManager = this.gameObject.AddComponent<ResourcesManager>();
        var kbemain=   this.gameObject.AddComponent<KBEMain>();
        kbemain.initKBEngine();
        StartCoroutine(resourcesManager.LoadManiFest());        
        var client=    this.gameObject.AddComponent<LuaClient>();
        Util.Client=client;      
        client.StartMain();
    }

}
