using System;
using System.Collections.Generic;

using UnityEngine;

namespace ET
{
	
	public class UIAwakeSystem : AwakeSystem<UI, int, AssetEntity,Type>
	{
		public override void Awake(UI self, int UIType, AssetEntity UIAssetEntity, Type UIComponentType)
		{
			self.Awake(UIType, UIAssetEntity,UIComponentType);
		}
	}
	
	public sealed class UI: Entity
	{
		public AssetEntity UIAssetEntity;

		public int UIType;

		public Type UIComponentType;

		public Dictionary<int, UI> uiTypeChildren = new Dictionary<int, UI>();
		
		public void Awake(int uiType, AssetEntity uiAssetEntity,Type uiComponentType )
		{
			this.uiTypeChildren.Clear();
			UIAssetEntity = uiAssetEntity;
			UIAssetEntity.GameObject.AddComponent<ComponentView>().Component = this;
			UIAssetEntity.GameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
			this.UIType = uiType;
			this.UIComponentType = uiComponentType;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			
			base.Dispose();

			foreach (UI ui in this.uiTypeChildren.Values)
			{
				ui.Dispose();
			}
			
			this.UIAssetEntity.Dispose();
			this.UIAssetEntity = null;
			this.uiTypeChildren.Clear();
			this.UIType = 0;
			this.UIComponentType = null;

		}

		public void SetAsFirstSibling()
		{
			this.UIAssetEntity.GameObject.transform.SetAsFirstSibling();
		}

		public void Add(UI ui)
		{
			this.uiTypeChildren.Add(ui.UIType, ui);
			ui.Parent = this;
		}

		public void Remove(int UIType)
		{
			UI ui;
			if (!this.uiTypeChildren.TryGetValue(UIType, out ui))
			{
				return;
			}
			this.uiTypeChildren.Remove(UIType);
			ui.Dispose();
		}

		public UI Get(int UIType)
		{
			// wenchao 修改UI Get
			UI child;
			if (this.uiTypeChildren.TryGetValue(UIType, out child))
			{
				return child;
			}
			GameObject childGameObject = this.UIAssetEntity.GameObject.transform.Find(UIType.ToString())?.gameObject;
			if (childGameObject == null)
			{
				return null;
			}
			child = EntityFactory.Create<UI, string, GameObject>(this.Domain, UIType.ToString(), childGameObject);
			this.Add(child);
			return child;
		}
	}
}