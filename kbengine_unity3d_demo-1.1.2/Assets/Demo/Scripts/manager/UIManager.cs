using FairyGUI;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LAYER
{
    Default = 0,
    Water = 4,
    UI = 5,

    GROUND = 8,
    PLAYER,
    NPC,
    MONSTER,

}
public class UIManager
{
    static UIManager m_instance;
    LuaClient client;
    public static UIManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = new UIManager();
            //modal 设置窗口是否模式窗口。模式窗口将阻止用户点击任何模式窗口后面的内容。当模式窗口显示时，模式窗口后可以自动覆盖一层灰色的颜色，这个颜色在这里定义：
            UIConfig.modalLayerColor = new Color(0f, 0f, 0f, 0.4f);
            //默认情况下，Window是具有点击自动排序功能的，也就是说，你点击一个窗口，系统会自动把窗口提到所有窗口的最前面，这也是所有窗口系统的规范。但你可以关闭这个功能：
            UIConfig.bringWindowToFrontOnClick = false;
        }
        return m_instance;
    }
    public void SetInfo(LuaClient client)
    {  
        this.client=client;
    }


   public class WindowInfo
    {        
        public int depth;
        public ShowType showtype;         
        public LuaTable table;    
        public LuaWindow win;
        public string pak_id;
    }
    public enum ShowType
    {
        Normal = 100,//--打开：
        PopTips = 5000,//--置顶

    } 
    Dictionary<string, WindowInfo> m_wins = new Dictionary<string, WindowInfo>();
    public  void OpenWindow(string res_address, string compname,string luaname, ShowType _type,  bool stop_low_layer_event, LuaTable param)
    {
        if (m_wins.ContainsKey(compname))
        {
            return;
        }
        WindowInfo info = new WindowInfo();
          
        UIPackage pak;
        if (ResourcesManager.all_manifest!=null)
        {
            var asset_ab = ResourcesManager.LoadUIAssetBundle(res_address);
             pak = UIPackage.AddPackage(asset_ab);
        }
        else
        {
            pak = UIPackage.AddPackage(res_address);
        }
        if (pak == null)
        {
            BBKDebug.LogWarning(string.Format("UI load failed  res_address={0}   component={1}" , res_address,compname));
            return;
        }
        LuaWindow win = new LuaWindow();
        
        info.pak_id = pak.id;
        win.contentPane = pak.CreateObject(compname).asCom;
        info.showtype = _type;       
        info.win = win;
        info.table = param;        
        win.modal = stop_low_layer_event;
        m_wins.Add(info.pak_id, info);

        if (client != null && !string.IsNullOrEmpty(luaname))
        {            
            client.CallMethod(luaname, "Start", info);           
        }
        win.BringToFront();
    }
     int layerstep = 100;
     int CountWindowLayer(WindowInfo info)
    {
        int sortingOrder = 0;
        var enume= m_wins.GetEnumerator();
        while (enume.MoveNext())
        {
            WindowInfo inf=enume.Current.Value;
            if (inf.showtype == info.showtype)
            {
                if(inf.depth > sortingOrder)
                {
                    sortingOrder = inf.depth;
                }
            }
        }       
        sortingOrder += layerstep;
        return sortingOrder;
    }
   
    public  void Close(WindowInfo info)
    {        
        UIPackage.RemovePackage(info.pak_id);
        m_wins.Remove(info.pak_id);
        info.win.Dispose();       
    }
    public  void SetLayerRecursively(GameObject obj, LAYER layer)
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




}
