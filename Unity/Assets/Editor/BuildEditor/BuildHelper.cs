using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class BuildHelper
    {
        private const string relativeDirPrefix = "../Release";

        public static string BuildFolder = "../Release/{0}/StreamingAssets/";


        [MenuItem("Tools/web资源服务器")]
        public static void OpenFileServer()
        {
            ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
        }

        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string exeName = "ET";
            switch (type)
            {
                case PlatformType.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
            }

            string fold = string.Format(BuildFolder, type);

            if (clearFolder && Directory.Exists(fold))
            {
                Directory.Delete(fold, true);
                Directory.CreateDirectory(fold);
            }
            else
            {
                Directory.CreateDirectory(fold);
            }

            Log.Info("开始资源打包");
            BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

            Log.Info("完成资源打包");

            if (isContainAB)
            {
                FileHelper.CleanDirectory("Assets/StreamingAssets/");
                FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            }

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/Scenes/Init.unity",
                };
                Log.Info("开始EXE打包");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
                Log.Info("完成exe打包");
            }
        }

        /// <summary>
        /// 创建AssetManifest
        /// </summary>
        public static void CreateAssetManifest()
        {
            FileHelper.CleanDirectory("Assets/Bundles/AssetManifestDir/");
            AssetManifest assetManifest = new AssetManifest();
            
            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundleName in bundleNames)
            {
                var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
                foreach (var assetPath in assetPaths)
                {
                    var prefabName = Path.GetFileNameWithoutExtension(assetPath);
                    assetManifest.list.Add(new AssetInfo()
                    {
                        AssetPath = assetPath,
                        BundleName = bundleName,
                        PrefabName = prefabName,
                    });
                }  
            }
            string path = Path.Combine("Assets/Bundles/AssetManifestDir/", $"AssetManifest.bytes");
            using FileStream file = File.Create(path);
            Serializer.Serialize(file, assetManifest);
            //AssetImporter.GetAtPath(path).SaveAndReimport();
            AssetDatabase.SaveAssets();
            Debug.Log("AssetManifest 生成成功！");
        }

        /// <summary>
        /// 自动标记所有资源的AB包 用于AssetManifest和打包
        /// </summary>
        public static void SetAssetBundleForAllAssets()
        {
            var bundleNames = AssetDatabase.GetAllAssetBundleNames();
            foreach (var bundleName in bundleNames)
            {
                AssetDatabase.RemoveAssetBundleName(bundleName, true);
            }
            
            AssetImporter.GetAtPath(AssetBundleHelper.AssetManifestDirPath).assetBundleName = AssetBundleHelper.AssetManifestDirPath;
            AssetImporter.GetAtPath(AssetBundleHelper.ConfigDirPath).assetBundleName = AssetBundleHelper.ConfigDirPath;
            var assetPaths =  Directory.GetFiles("Assets/Bundles/", "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")&& !name.EndsWith(".cs") ).ToList();
            var ScenePaths = Directory.GetFiles("Assets/Scenes/", "*.unity", SearchOption.AllDirectories).Where(name => !name.EndsWith("Init.unity")).ToList();
            
            foreach (var assetPath in assetPaths)
            {
                AssetImporter.GetAtPath(assetPath).assetBundleName = $"{Path.GetDirectoryName(assetPath)}";
            }
            
            foreach (var scenePath in ScenePaths)
            {
                AssetImporter.GetAtPath(scenePath).assetBundleName = $"{scenePath}.unity3d";
            }
            Debug.Log("AssetBundle 标记成功！");
        }
        
        
        //使用深度遍历检查环
        public static void CheckLoopByAssetDatabase()
        {
            //dependenceMap保存bundle的直接依赖关系
            var dependenceMap = new Dictionary<string, HashSet<string>>();
            //string[] abs = manifest.GetAllAssetBundles();
            string[] abs = AssetDatabase.GetAllAssetBundleNames();//.Where(name=>!name.EndsWith("Assets/Bundles/AssetManifestDir".ToLower())).ToArray();

            for (int i = 0; i < abs.Length; i++)
            {
                string abName = abs[i];
                //string[] directDependencies = manifest.GetDirectDependencies(abName);
                string[] directDependencies = AssetDatabase.GetDependencies(abName);
                dependenceMap.Add(abName, new HashSet<string>(directDependencies));
            }

            //q保存需要检查环的bundle名字
            var q = new LinkedList<string>();
            foreach (var entry in dependenceMap)
            {
                q.AddLast(entry.Key);
            }

            //searchedNodeSet保存遍历过的bundle，避免重复遍历
            var searchedNodeSet = new HashSet<string>();

            //loopSet记录检查到的环
            var loopSet = new HashSet<string[]>();
            while (q.Count > 0)
            {
                string bundleName = q.First.Value;
                q.RemoveFirst();

                //stack记录深度遍历时遍历到的bundle
                var stack = new List<string>();

                //开始通过遍历检查
                SearchLoopByManifest(bundleName, stack, searchedNodeSet, dependenceMap, loopSet);

                //把遍历过的bundle从q删除
                foreach (string node in searchedNodeSet)
                {
                    q.Remove(node);
                }
            }

            //以抛出异常的方式，打印所有环信息
            int maxPrintLoopNum = 100;
            if (loopSet.Count > 0)
            {
                int i = 0;
                string log = "bundle loops:";
                foreach (string[] bundles in loopSet)
                {
                    if (i >= maxPrintLoopNum)
                    {
                        break;
                    }

                    log += i + ":";
                    for (int j = 0; j < bundles.Length + 1; j++)
                    {
                        string bundleName = bundles[j % bundles.Length];
                        log += bundleName;
                        if (j != bundles.Length)
                        {
                            log += " -> ";
                        }
                    }

                    log += "\n";
                    i++;
                }

                Debug.LogError(log);
                //throw new Exception(log);
            }
        }

        private static void SearchLoopByManifest(string bundleName, List<string> stack, HashSet<string> searchedNodeSet,
        Dictionary<string, HashSet<string>> dependenceMap, HashSet<string[]> loopSet)
        {
            if (string.IsNullOrEmpty(bundleName))
            {
                return;
            }

            int index = stack.IndexOf(bundleName);
            if (index < 0) //bundleName不在stack里，没形成环
            {
                if (!searchedNodeSet.Contains(bundleName)) //必须之前没遍历过这个结点
                {
                    searchedNodeSet.Add(bundleName);
                    stack.Add(bundleName);
                    HashSet<string> dependencies = null;
                    dependenceMap.TryGetValue(bundleName, out dependencies);

                    if (dependencies == null)
                    {
                        //Debug.Log("dependencies is null: " + bundleName);
                        //throw new Exception("dependencies is null: " + bundleName);
                    }
                    else
                    {
                        //遍历更深的结点
                        foreach (string d in dependencies)
                        {
                            SearchLoopByManifest(d, stack, searchedNodeSet, dependenceMap, loopSet);
                        }
                    }

                    //这里一定要移除，stack记录当前遍历到的结点，当前的bundleName已经遍历过了，所以要移除
                    stack.Remove(bundleName);
                }
            }
            else //存在环，记录到loopSet里
            {
                string[] loop = new string[stack.Count - index];
                for (int i = index; i < stack.Count; i++)
                {
                    loop[i - index] = stack[i];
                }

                loopSet.Add(loop);
            }
        }
    }
}
