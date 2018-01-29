using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
public class PackResourceAssetBundle
{
    [MenuItem("Tools/PackAssetBundle")]
    static void DoIt()
    {
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/" + Const.ProjectName + "/Resources");
        if (di.Exists)
        {
            List<AssetBundleBuild> list = new List<AssetBundleBuild>();
            var files = di.GetFiles("*.*", SearchOption.AllDirectories);
            var count = files.Length;
            for (int i = 0; i < count; i++)
            {
                var file = files[i];
                var path = file.FullName.Replace("\\", "/");
                if (!file.Name.EndsWith(".meta"))
                {
                    AssetBundleBuild ab = new AssetBundleBuild();

                    if (file.Name.Contains("@"))
                    {
                        int len1 = path.IndexOf(Const.ProjectName) + Const.ProjectName.Length + 1;
                        int len2 = path.LastIndexOf("@");
                        ab.assetBundleName = path.Substring(len1, len2 - len1) + Const.endname;

                    }
                    else
                    {
                        int len1 = path.IndexOf(Const.ProjectName) + Const.ProjectName.Length + 1;
                        int len2 = path.LastIndexOf(".");
                        ab.assetBundleName = path.Substring(len1, len2 - len1) + Const.endname;
                    }
                    ab.assetNames = new string[] { path.Substring(path.IndexOf("Assets")) };

                    list.Add(ab);
                }
            }
            string out_path = Application.dataPath + "/../" + Const.osDir;
            DirectoryInfo d2 = new DirectoryInfo(out_path);
            if (!d2.Exists)
            {
                d2.Create();
            }
            BuildTarget bt = BuildTarget.NoTarget;


#if UNITY_ANDROID
            bt = BuildTarget.Android;
#elif UNITY_IOS
            bt = BuildTarget.iOS;
#else
            bt = BuildTarget.StandaloneWindows64;
#endif


            BuildPipeline.BuildAssetBundles(Application.dataPath + "/../" + Const.osDir, list.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, bt);
            BuildFileIndex();
        }
    }
    static void BuildFileIndex()
    {

        string resPath = (Application.dataPath + "/../" + Const.osDir + "/").Replace("\\", "/");

        ///----------------------创建文件列表-----------------------

        string newFilePath = Application.dataPath + "/../" + Const.osDir + "/files.txt";

        if (File.Exists(newFilePath)) File.Delete(newFilePath);
        DirectoryInfo di = new DirectoryInfo(resPath);
        var files = di.GetFiles("*.*", SearchOption.AllDirectories);



        FileStream fs = new FileStream(newFilePath, FileMode.CreateNew);

        StreamWriter sw = new StreamWriter(fs);

        for (int i = 0; i < files.Length; i++)
        {

            var file = files[i];
            if (file.Name. EndsWith(".meta") || file.Name.Contains(".DS_Store")) continue;



            string md5 = Util.md5file(file.FullName);
            var path=    Path.GetFullPath(resPath).Replace("\\", "/");
            string value = (file.FullName.Replace("\\", "/")). Replace(path, string.Empty);

            sw.WriteLine(value + "|" + md5);

        }

        sw.Close(); fs.Close();

    }
}