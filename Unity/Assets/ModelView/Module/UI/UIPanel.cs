using System;
using System.Collections.Generic;

using UnityEngine;

namespace ET
{
	public class UIAwakeSystem : AwakeSystem<UIPanel, int, AssetEntity>
	{
		public override void Awake(UIPanel self, int uiPanelType, AssetEntity UIAssetEntity)
		{
			self.Awake(uiPanelType, UIAssetEntity);
		}
	}
	
	public sealed class UIPanel: Entity
	{
		public AssetEntity UIPanelAssetEntity;

		public int UIPanelType;
		
		public Dictionary<int, UIPanel> SubPanels = new Dictionary<int, UIPanel>();
		
		public void Awake(int uiPanelType, AssetEntity uiAssetEntity)
		{
			this.SubPanels.Clear();
			this.UIPanelAssetEntity = uiAssetEntity;
			this.UIPanelAssetEntity.Object.AddComponent<ComponentView>().Component = this;
			this.UIPanelAssetEntity.Object.layer = LayerMask.NameToLayer(LayerNames.UI);
			this.UIPanelType = uiPanelType;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}
			base.Dispose();
			foreach (UIPanel ui in this.SubPanels.Values)
			{
				ui.Dispose();
			}
			this.UIPanelAssetEntity.Dispose();
			this.UIPanelAssetEntity = null;
			this.SubPanels.Clear();
			this.UIPanelType = 0;
		}

		public void SetAsFirstSibling()
		{
			this.UIPanelAssetEntity.Object.transform.SetAsFirstSibling();
		}

		public void AddSubPanel(UIPanel uiPanel)
		{
			this.SubPanels.Add(uiPanel.UIPanelType, uiPanel);
			uiPanel.Parent = this;
		}

		public void RemoveSubPanel(int uiPanelType)
		{
			UIPanel uiPanel;
			if (!this.SubPanels.TryGetValue(uiPanelType, out uiPanel))
			{
				return;
			}
			this.SubPanels.Remove(uiPanelType);
			uiPanel.Dispose();
		}

		public UIPanel GetSubPanel(int uiPanelType)
		{
			UIPanel child;
			if (this.SubPanels.TryGetValue(uiPanelType, out child))
			{
				return child;
			}
			return null;
		}
	}
}