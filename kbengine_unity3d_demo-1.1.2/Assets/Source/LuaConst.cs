using UnityEngine;

public static class LuaConst
{
    private static string _luaDir;                //lua逻辑代码目录


    private static string _toluaDir;        //tolua lua文件目录



    public static string luaResDir = string.Format("{0}/{1}/Lua", Application.persistentDataPath, Const.osDir);      //手机运行时lua文件下载目录    

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN    
    public static string zbsDir = "D:/ZeroBraneStudio/lualibs/mobdebug";        //ZeroBraneStudio目录       
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
	public static string zbsDir = "/Applications/ZeroBraneStudio.app/Contents/ZeroBraneStudio/lualibs/mobdebug";
#else
    public static string zbsDir = luaResDir + "/mobdebug/";
#endif    

    public static bool openLuaSocket = false;            //是否打开Lua Socket库
    public static bool openLuaDebugger = false;         //是否连接lua调试器

    public static string luaDir
    {
        get
        {
            if (ResourcesManager.all_manifest == null)
            {
                _luaDir = Application.dataPath + "/"+Const.ProjectName + "/Lua";
            }
            else
            {
                _luaDir = Application.persistentDataPath + "/Lua";
            }
            return _luaDir;
        }

        
    }

    public static string ToluaDir
    {
        get
        {
            if (ResourcesManager.all_manifest == null)
            {
                _toluaDir = Application.dataPath + "/" + Const.ProjectName + "/ToLua/Lua";
            }
            else
            {
                _toluaDir = Application.persistentDataPath + "/ToLua/Lua";
            }         
            return _toluaDir;
        }

        
    }
}