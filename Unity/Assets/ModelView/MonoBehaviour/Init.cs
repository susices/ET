using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace ET
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
				
				DontDestroyOnLoad(gameObject);

				string[] assemblyNames = { "Unity.Model.dll", "Unity.Hotfix.dll", "Unity.ModelView.dll", "Unity.HotfixView.dll" };
				
				// foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				// {
				// 	string assemblyName = assembly.ManifestModule.Name;
				// 	
				// 	if (!assemblyNames.Contains(assemblyName))
				// 	{
				// 		continue;
				// 	}
				// 	Game.EventSystem.Add(assembly);	
				// }
				
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					string assemblyName = $"{assembly.GetName().Name}.dll";
					
					if (!assemblyNames.Contains(assemblyName))
					{
						continue;
					}
					Game.EventSystem.Add(assembly);	
				}
				
				ProtobufHelper.Init();
				
				Game.Options = new Options();
				
				Game.EventSystem.Publish(new EventType.AppStart());
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			ThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
			
		}
		
	}
}