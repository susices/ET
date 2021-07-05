using System.Collections.Generic;

using UnityEngine;

namespace ET
{
	
	public class UIAwakeSystem : AwakeSystem<UI, string, AssetEntity>
	{
		public override void Awake(UI self, string name, AssetEntity UIAssetEntity)
		{

			self.Awake(name, UIAssetEntity);
		}
	}
	
	public sealed class UI: Entity
	{
		public AssetEntity UIAssetEntity;
		
		public string Name { get; private set; }

		public Dictionary<string, UI> nameChildren = new Dictionary<string, UI>();
		
		public void Awake(string name, AssetEntity uiAssetEntity)
		{
			this.nameChildren.Clear();
			UIAssetEntity = uiAssetEntity;
			UIAssetEntity.GameObject.AddComponent<ComponentView>().Component = this;
			UIAssetEntity.GameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
			this.Name = name;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (UI ui in this.nameChildren.Values)
			{
				ui.Dispose();
			}
			
			this.UIAssetEntity.Dispose();
			this.nameChildren.Clear();
		}

		public void SetAsFirstSibling()
		{
			this.UIAssetEntity.GameObject.transform.SetAsFirstSibling();
		}

		public void Add(UI ui)
		{
			this.nameChildren.Add(ui.Name, ui);
			ui.Parent = this;
		}

		public void Remove(string name)
		{
			UI ui;
			if (!this.nameChildren.TryGetValue(name, out ui))
			{
				return;
			}
			this.nameChildren.Remove(name);
			ui.Dispose();
		}

		public UI Get(string name)
		{
			UI child;
			if (this.nameChildren.TryGetValue(name, out child))
			{
				return child;
			}
			GameObject childGameObject = this.UIAssetEntity.GameObject.transform.Find(name)?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}
			child = EntityFactory.Create<UI, string, GameObject>(this.Domain, name, childGameObject);
			this.Add(child);
			return child;
		}
	}
}