using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public delegate void LoadCompleteCallback(LoadTask task);
    public class LoadTask
    {
        public string[] paths;
        public LoadCompleteCallback onComplete;
        public string luaOnComplete;
        public LuaTable luaTable;

        public Object[] objs;
        public object param;     

        public LoadTask()
        {

        }

        public void LoadEnd()
        {
            try
            {
                if (onComplete != null)
                {
                    onComplete(this);
                }

                if (luaTable != null && string.IsNullOrEmpty(luaOnComplete) == false)
                {
                    LuaFunction func = luaTable[luaOnComplete] as LuaFunction;
                    if (func != null)
                    {
                        func.Call(luaTable, this);
                    }
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogException(e);
            }
        }
    }

    public static AssetBundleManifest all_manifest;
    public IEnumerator LoadManiFest()
    {
        string path = string.Format("{0}{1}/{2}", Const.DataPath, Const.osDir, Const.osDir);

        if (File.Exists(path) == false)
        {

            BBKDebug.LogError("ResourcesManager::LoadManiFest error! is not exist!!!path=" + path);
            yield break;
        }

        AssetBundle manifestBundle = AssetBundle.LoadFromFile(path);
        all_manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
    }
    ThreadManager threadManager;
    //默认所有prefab实例化
    public void LoadResource(string[] url, LoadCompleteCallback onComplete, object param)
    {
        LoadTask task = new LoadTask();
        task.paths = url;
        task.onComplete = onComplete;
        task.param = param;
        task.objs = new Object[url.Length];
        if (all_manifest != null)
        {
            //多线程加载
            ThreadEvent ev = new ThreadEvent();
            ev.Key = NotiConst.UPDATE_LOAD;
            ev.evParams.Add(task);
            if (threadManager == null)
            {
                threadManager = this.gameObject.GetComponent<ThreadManager>();
            }
            threadManager.AddEvent(ev, OnLoadCompleted);
        }
        else
        {
            for (int i = 0; i < url.Length; i++)
            {
                Object o = Resources.Load(url[i]);
                if (o is GameObject)
                {
                    o = Instantiate(o);
                }
                task.objs[i] = o;
            }
            task.LoadEnd();
        }

    }

    //同步加载ui组件

    public static AssetBundle LoadUIAssetBundle(string url)
    {
        var depend_path = string.Format("resources/{0}{1}", url.ToLower(), Const.endname);
        string[] depends = ResourcesManager.all_manifest.GetAllDependencies(depend_path);
        foreach (string n in depends)
        {
            LoadDepednece(n);
        }
        var resource_url = string.Format("{0}{1}/resources/{2}{3}", Const.DataPath, Const.osDir, url.ToLower(), Const.endname);
        AssetBundle ab = AssetBundle.LoadFromFile(resource_url);     
        return ab;
    }

    static void LoadDepednece(string _name)
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
    }
    void OnLoadCompleted(NotiData data)
    {
        if (data.evName.Equals(NotiConst.UPDATE_LOAD))
        {
            var task = data.evParam as LoadTask;
            task.LoadEnd();
        }
    }


}
