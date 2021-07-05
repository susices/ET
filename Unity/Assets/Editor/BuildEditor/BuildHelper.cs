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
            FileHelper.CleanDirectory("Assets/Bundles/AssetManifest/");
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
            string path = Path.Combine("Assets/Bundles/AssetManifest/", $"AssetManifest.bytes");
            using FileStream file = File.Create(path);
            Serializer.Serialize(file, assetManifest);
            AssetImporter.GetAtPath(path).assetBundleName = "assetmanifest.unity3d";
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

            var assetPaths =  Directory.GetFiles("Assets/Bundles/", "*.*", SearchOption.AllDirectories).Where(name => !name.EndsWith(".meta")&& !name.EndsWith(".cs") ).ToList();
            foreach (var assetPath in assetPaths)
            {
                AssetImporter.GetAtPath(assetPath).assetBundleName = Path.GetDirectoryName(assetPath);
            }

        }
    }
}
