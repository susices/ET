using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;
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
                AssetImporter.GetAtPath(scenePath).assetBundleName = $"{scenePath}3d";
            }
            Debug.Log("AssetBundle 标记成功！");
        }

        private static Dictionary<string, string[]> SortedDepCache;
        private static Dictionary<string, string[]> DependenciesCache;
        

        public static void LoopDependencyCheck()
        {
            string path = Path.Combine("Assets/Bundles/AssetManifestDir/", $"AssetManifest.bytes");
            var data = File.OpenRead(path);
            var assetManifist = Serializer.Deserialize<AssetManifest>(data);
            SortedDepCache = new Dictionary<string, string[]>();
            DependenciesCache = new Dictionary<string, string[]>();
            try
            {
                foreach (var assetInfo in assetManifist.GetAll().Values)
                {
                    GetSortedDependencies(assetInfo.BundleName, SortedDepCache);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            Debug.Log("循环依赖检查完毕");
            SortedDepCache = null;
            DependenciesCache = null;
        }
        
        private static void GetSortedDependencies(string assetBundleName, Dictionary<string, string[]> SortedDepCache)
        {
            if (SortedDepCache.TryGetValue(assetBundleName, out var SorteDeps))
            {
                return ;
            }
            var info = new Dictionary<string, int>();
            using var list = ListComponent<string>.Create();
            CollectDependencies(list.List, assetBundleName, info);
            string[] ss = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            SortedDepCache[assetBundleName] = ss;
        }
        
        private static void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
        {
            parents.Add(assetBundleName);
            string[] deps = GetDependencies(assetBundleName);
            foreach (string parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;
                }

                info[parent] += deps.Length;
            }

            foreach (string dep in deps)
            {
                if (parents.Contains(dep))
                {
                    Debug.LogError($"发现循环依赖: BundleName: {assetBundleName}  Dependency：{dep}");
                    break;
                }
                CollectDependencies(parents, dep, info);
            }

            parents.RemoveAt(parents.Count - 1);
        }
        
        private static string[] GetDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (DependenciesCache.TryGetValue(assetBundleName, out dependencies))
            {
                return dependencies;
            }
            dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
            DependenciesCache.Add(assetBundleName, dependencies);
            return dependencies;
        }

        public static void TestEntity()
        {
            SceneEntityManifest manifest = new SceneEntityManifest();
            manifest.list.Add(new SceneEntityBuildInfo(){SceneEntityInfo = new CharacterInfo(){Name = "Test"}, Position = new float[]{1,2,3}});
            var bytes =  ProtobufHelper.ToBytes(manifest);
            manifest = ProtobufHelper.FromBytes(typeof(SceneEntityManifest), bytes,0,bytes.Length) as SceneEntityManifest;
            var test = manifest.list;
        }
        
        public static void TestEntityRead()
        {
            
        }
        
    }
}
