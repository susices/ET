using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Linq;
using BM;

namespace ET
{
	public class CodeLoader: IDisposable
	{
		public static CodeLoader Instance = new CodeLoader();

		public Action Update;
		public Action LateUpdate;
		public Action OnApplicationQuit;

		private Assembly assembly;

		private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
		
		private Type[] allTypes;
		
		public CodeMode CodeMode { get; set; }

		private CodeLoader()
		{
		}

		public void Dispose()
		{
			this.appDomain?.Dispose();
		}

		public async ETTask Initialize()
		{
			await ETTask.CompletedTask;
			AssetComponentConfig.DefaultBundlePackageName = "AllBundles";
			var updatePackageBundles = new Dictionary<string, bool>();
			updatePackageBundles.Add(AssetComponentConfig.DefaultBundlePackageName, true);
			UpdateBundleDataInfo updateBundleDataInfo = await AssetComponent.CheckAllBundlePackageUpdate(updatePackageBundles);
			if (updateBundleDataInfo.NeedUpdate)
			{
				Debug.Log($"需要更新, 大小： {updateBundleDataInfo.NeedUpdateSize.ToString()}");
				await AssetComponent.DownLoadUpdate(updateBundleDataInfo);
			}
			await AssetComponent.Initialize(AssetComponentConfig.DefaultBundlePackageName);
			Start();
		}
		
		public void Start()
		{
			switch (this.CodeMode)
			{
				case CodeMode.Mono:
				{
					byte[] assBytes = AssetComponent.Load<TextAsset>("Assets/Bundles/Code/Code.dll.bytes").bytes;
					byte[] pdbBytes = AssetComponent.Load<TextAsset>("Assets/Bundles/Code/Code.pdb.bytes").bytes;
					
					assembly = Assembly.Load(assBytes, pdbBytes);
					this.allTypes = assembly.GetTypes();
					IStaticMethod start = new MonoStaticMethod(assembly, "ET.Entry", "Start");
					start.Run();
					break;
				}
				case CodeMode.ILRuntime:
				{
					byte[] assBytes = AssetComponent.Load<TextAsset>("Assets/Bundles/Code/Code.dll.bytes").bytes;
					byte[] pdbBytes = AssetComponent.Load<TextAsset>("Assets/Bundles/Code/Code.pdb.bytes").bytes;
					
					//byte[] assBytes = File.ReadAllBytes(Path.Combine("../Unity/", Define.BuildOutputDir, "Code.dll"));
					//byte[] pdbBytes = File.ReadAllBytes(Path.Combine("../Unity/", Define.BuildOutputDir, "Code.pdb"));
				
					appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
					this.appDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
					MemoryStream assStream = new MemoryStream(assBytes);
					MemoryStream pdbStream = new MemoryStream(pdbBytes);
					appDomain.LoadAssembly(assStream, pdbStream, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

					ILHelper.InitILRuntime(appDomain);

					this.allTypes = appDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToArray();
					IStaticMethod start = new ILStaticMethod(appDomain, "ET.Entry", "Start", 0);
					start.Run();
					break;
				}
				case CodeMode.Reload:
				{
					byte[] assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Data.dll"));
					byte[] pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, "Data.pdb"));
					
					assembly = Assembly.Load(assBytes, pdbBytes);
					this.LoadLogic();
					IStaticMethod start = new MonoStaticMethod(assembly, "ET.Entry", "Start");
					start.Run();
					break;
				}
			}
		}

		// 热重载调用下面三个方法
		// CodeLoader.Instance.LoadLogic();
		// Game.EventSystem.Add(CodeLoader.Instance.GetTypes());
		// Game.EventSystem.Load();
		public void LoadLogic()
		{
			if (this.CodeMode != CodeMode.Reload)
			{
				throw new Exception("CodeMode != Reload!");
			}
			
			// 傻屌Unity在这里搞了个傻逼优化，认为同一个路径的dll，返回的程序集就一样。所以这里每次编译都要随机名字
			string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Logic_*.dll");
			if (logicFiles.Length != 1)
			{
				throw new Exception("Logic dll count != 1");
			}

			string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
			byte[] assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.dll"));
			byte[] pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.pdb"));

			Assembly hotfixAssembly = Assembly.Load(assBytes, pdbBytes);
			
			List<Type> listType = new List<Type>();
			listType.AddRange(this.assembly.GetTypes());
			listType.AddRange(hotfixAssembly.GetTypes());
			this.allTypes = listType.ToArray();
		}

		public Type[] GetTypes()
		{
			return this.allTypes;
		}
	}
}