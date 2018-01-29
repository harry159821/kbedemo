using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using UnityEngine;

public class ThreadEvent
{
    public string Key;
    public List<object> evParams = new List<object>();
}
public class NotiConst
{
    /// <summary>
    /// Controller层消息通知
    /// </summary>

    public const string UPDATE_DOWNLOAD = "UpdateDownload";         //更新下载
    public const string UPDATE_PROGRESS = "UpdateProgress";         //更新进度

    public const string UPDATE_LOAD = "UpdateLoad";//加载资源
}

public class NotiData
{
    public string evName;
    public object evParam;

    public NotiData(string name, object param)
    {
        this.evName = name;
        this.evParam = param;
    }
}
public class ThreadManager : MonoBehaviour
{
    private Thread thread;
    private System.Action<NotiData> func;
    private Stopwatch sw = new Stopwatch();
    private string currDownFile = string.Empty;

    static readonly object m_lockObject = new object();
    static Queue<ThreadEvent> events = new Queue<ThreadEvent>();

    delegate void ThreadSyncEvent(NotiData data);
    private ThreadSyncEvent m_SyncEvent;

    void Awake()
    {
        m_SyncEvent = OnSyncEvent;
        thread = new Thread(OnUpdate);
    }

    // Use this for initialization
    void Start()
    {
        thread.Start();
    }

    /// <summary>
    /// 添加到事件队列
    /// </summary>
    public void AddEvent(ThreadEvent ev, System.Action<NotiData> func)
    {
        lock (m_lockObject)
        {
            this.func = func;
            events.Enqueue(ev);
        }
    }

    /// <summary>
    /// 通知事件
    /// </summary>
    /// <param name="state"></param>
    private void OnSyncEvent(NotiData data)
    {
        if (this.func != null) func(data);  //回调逻辑层

    }

    // Update is called once per frame
    void OnUpdate()
    {
        while (true)
        {
            lock (m_lockObject)
            {
                if (events.Count > 0)
                {
                    ThreadEvent e = events.Dequeue();
                    try
                    {
                        switch (e.Key)
                        {
                            case NotiConst.UPDATE_DOWNLOAD:
                                {    //下载文件
                                    OnDownloadFile(e.evParams);
                                }
                                break;
                            case NotiConst.UPDATE_LOAD:

                                OnLoadAssetBundle(e.evParams);
                                break;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError(ex.Message);
                    }
                }
            }
            Thread.Sleep(1);
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    void OnDownloadFile(List<object> evParams)
    {
        string url = evParams[0].ToString();
        currDownFile = evParams[1].ToString();

        using (WebClient client = new WebClient())
        {
            sw.Start();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            client.DownloadFileAsync(new System.Uri(url), currDownFile);
        }
    }

    private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
    {
        string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
        NotiData data = new NotiData(NotiConst.UPDATE_PROGRESS, value);
        if (m_SyncEvent != null) m_SyncEvent(data);

        if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
        {
            sw.Reset();

            data = new NotiData(NotiConst.UPDATE_DOWNLOAD, currDownFile);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }
    }
    List<AssetBundle> _load_assetbundle = new List<AssetBundle>();
    void OnLoadAssetBundle(List<object> evParams)
    {
        ResourcesManager.LoadTask task = evParams[0] as ResourcesManager.LoadTask;
        if (ResourcesManager.all_manifest != null)
        {
            for (int i = 0; i < task.paths.Length; i++)
            {
                var _url = task.paths[i].ToLower();
                var depend_path = string.Format("resources/{0}{1}", _url, Const.endname);
                string[] depends = ResourcesManager.all_manifest.GetAllDependencies(depend_path);
                foreach (string n in depends)
                {
                    LoadDepednece(n);
                }
                var resource_url = string.Format("{0}{1}/resources/{2}{3}", Const.DataPath, Const.osDir, _url, Const.endname);
                AssetBundle ab = AssetBundle.LoadFromFile(resource_url);
                _load_assetbundle.Add(ab);
                Object o = ab.LoadAllAssets()[0];
                if (o is GameObject)
                {
                    o = Instantiate(o);
                }
                task.objs[i] = o;
            }
            NotiData data = new NotiData(NotiConst.UPDATE_LOAD, task);
            if (m_SyncEvent != null) m_SyncEvent(data);
        }
        var count = _load_assetbundle.Count;
        for (int i = 0; i < count; i++)
        {
            _load_assetbundle[i].Unload(false);
        }
        _load_assetbundle.Clear();
    }
    void LoadDepednece(string _name)
    {
        var ab__path = string.Format("{0}{1}/resources/{2}", Const.DataPath, Const.osDir, _name);
        string[] depends = ResourcesManager.all_manifest.GetAllDependencies(_name);
        var count = depends.Length;
        for (int i = 0; i < count; i++)
        {
            LoadDepednece(depends[i]);
        }
        var asset = AssetBundle.LoadFromFile(ab__path);
        asset.LoadAllAssets();
        _load_assetbundle.Add(asset);
    }


    /// <summary>
    /// 应用程序退出
    /// </summary>
    void OnDestroy()
    {
        thread.Abort();
    }
}
