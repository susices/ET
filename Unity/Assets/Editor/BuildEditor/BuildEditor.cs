using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public class BundleInfo
    {
        public List<string> ParentPaths = new List<string>();
    }

    public enum PlatformType
    {
        None,
        Android,
        IOS,
        PC,
        MacOS,
    }

    public enum BuildType
    {
        Development,
        Release,
    }

    public class BuildEditor: EditorWindow
    {
        private const string settingAsset = "Assets/Editor/BuildEditor/ETBuildSettings.asset";

        private readonly Dictionary<string, BundleInfo> dictionary = new Dictionary<string, BundleInfo>();

        private PlatformType activePlatform;
        private PlatformType platformType;
        private bool clearFolder;
        private bool isBuildExe;
        private bool isContainAB;
        private BuildType buildType;
        private BuildOptions buildOptions;
        private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;

        private ETBuildSettings buildSettings;

        [MenuItem("Tools/打包工具")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<BuildEditor>(true, "打包工具");
            window.minSize = new Vector2(420, 220);
            window.maxSize = new Vector2(700, 400);
        }

        private void OnEnable()
        {
#if UNITY_ANDROID
			activePlatform = PlatformType.Android;
#elif UNITY_IOS
			activePlatform = PlatformType.IOS;
#elif UNITY_STANDALONE_WIN
            activePlatform = PlatformType.PC;
#elif UNITY_STANDALONE_OSX
			activePlatform = PlatformType.MacOS;
#else
			activePlatform = PlatformType.None;
#endif
            platformType = activePlatform;

            if (!File.Exists(settingAsset))
            {
                buildSettings = new ETBuildSettings();
                AssetDatabase.CreateAsset(buildSettings, settingAsset);
            }
            else
            {
                buildSettings = AssetDatabase.LoadAssetAtPath<ETBuildSettings>(settingAsset);

                clearFolder = buildSettings.clearFolder;
                isBuildExe = buildSettings.isBuildExe;
                isContainAB = buildSettings.isContainAB;
                buildType = buildSettings.buildType;
                buildAssetBundleOptions = buildSettings.buildAssetBundleOptions;
            }
        }

        private void OnDisable()
        {
            SaveSettings();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("打包平台:");
            this.platformType = (PlatformType) EditorGUILayout.EnumPopup(platformType);
            this.clearFolder = EditorGUILayout.Toggle("清理资源文件夹: ", clearFolder);
            this.isBuildExe = EditorGUILayout.Toggle("是否打包EXE: ", this.isBuildExe);
            this.isContainAB = EditorGUILayout.Toggle("是否同将资源打进EXE: ", this.isContainAB);
            this.buildType = (BuildType) EditorGUILayout.EnumPopup("BuildType: ", this.buildType);
            EditorGUILayout.LabelField("BuildAssetBundleOptions(可多选):");
            this.buildAssetBundleOptions = (BuildAssetBundleOptions) EditorGUILayout.EnumFlagsField(this.buildAssetBundleOptions);

            switch (buildType)
            {
                case BuildType.Development:
                    this.buildOptions = BuildOptions.Development | BuildOptions.AutoRunPlayer | BuildOptions.ConnectWithProfiler |
                            BuildOptions.AllowDebugging;
                    break;
                case BuildType.Release:
                    this.buildOptions = BuildOptions.None;
                    break;
            }
            
            GUILayout.Space(5);

            if (GUILayout.Button("自动标记AB 生成AssetManifest", GUILayout.ExpandHeight(true)))
            {
                BuildHelper.SetAssetBundleForAllAssets();
                BuildHelper.CreateAssetManifest();
            }
            
            // GUILayout.Space(5);
            //
            // if (GUILayout.Button("解析AssetManifest", GUILayout.ExpandHeight(true)))
            // {
            //     var bytes =  File.ReadAllBytes(Path.Combine("Assets/Bundles/AssetManifest/", $"AssetManifest.bytes"));
            //
            //     AssetManifest assetManifest =  ProtobufHelper.FromBytes(typeof (AssetManifest), bytes, 0, bytes.Length) as AssetManifest;
            // }


            GUILayout.Space(5);
            
            if (GUILayout.Button("开始打包", GUILayout.ExpandHeight(true)))
            {
                if (this.platformType == PlatformType.None)
                {
                    ShowNotification(new GUIContent("请选择打包平台!"));
                    return;
                }

                if (platformType != activePlatform)
                {
                    switch (EditorUtility.DisplayDialogComplex("警告!", $"当前目标平台为{activePlatform}, 如果切换到{platformType}, 可能需要较长加载时间", "切换", "取消", "不切换"))
                    {
                        case 0:
                            activePlatform = platformType;
                            break;
                        case 1:
                            return;
                        case 2:
                            platformType = activePlatform;
                            break;
                    }
                }

                BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB,
                    this.clearFolder);
            }

            GUILayout.Space(5);
        }

        private void SaveSettings()
        {
            buildSettings.clearFolder = clearFolder;
            buildSettings.isBuildExe = isBuildExe;
            buildSettings.isContainAB = isContainAB;
            buildSettings.buildType = buildType;
            buildSettings.buildAssetBundleOptions = buildAssetBundleOptions;

            EditorUtility.SetDirty(buildSettings);
            AssetDatabase.SaveAssets();
        }

        //使用深度遍历检查环
        private static void CheckLoopByManifest(AssetBundleManifest manifest)
        {
            //dependenceMap保存bundle的直接依赖关系
            var dependenceMap = new Dictionary<string, HashSet<string>>();
            string[] abs = manifest.GetAllAssetBundles();
            for (int i = 0; i < abs.Length; i++)
            {
                string abName = abs[i];
                string[] directDependencies = manifest.GetDirectDependencies(abName);
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

                throw new Exception(log);
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
                        throw new Exception("dependencies is null: " + bundleName);
                    }

                    //遍历更深的结点
                    foreach (string d in dependencies)
                    {
                        SearchLoopByManifest(d, stack, searchedNodeSet, dependenceMap, loopSet);
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