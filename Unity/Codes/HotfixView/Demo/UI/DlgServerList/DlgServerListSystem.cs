using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class DlgServerListLoadSystem : LoadSystem<DlgServerList>
	{
		public override void Load(DlgServerList self)
		{
			self.RegisterUIEvent();
			if (self.GetParent<UIBaseWindow>().IsVisible())
			{
				self.ShowWindow();				
			}
		}
	}

	public static  class DlgServerListSystem
	{

		public static void RegisterUIEvent(this DlgServerList self)
		{
			self.View.EButton_EnterServerButton.AddListenerAsync(() => { return self.OnClickEnterServerBtn(); });
			
			self.View.ELoopScrollList_ServerListLoopVerticalScrollRect.AddItemRefreshListener((Transform transform, int Index) =>
			{
				self.OnLoopListItemRefreshHandler(transform,Index);
			});
		}

		public static void ShowWindow(this DlgServerList self, Entity contextData = null)
		{
			int count = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList.Count;
			self.AddUIScrollItems(ref self.ScrollItemServerInfoDic,count);
			self.View.ELoopScrollList_ServerListLoopVerticalScrollRect.SetVisible(true,count);
		}

		public static void HideWindow(this DlgServerList self)
		{
			self.RemoveUIScrollItems(ref self.ScrollItemServerInfoDic);
		}

		public static void OnSelectServerItemHandler(this DlgServerList self, long serverId)
		{
			self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId = int.Parse(serverId.ToString());
			Log.Debug($"当前选择的服务器Id是{serverId.ToString()}");
			self.View.ELoopScrollList_ServerListLoopVerticalScrollRect.RefillCells();
		}

		public static void OnLoopListItemRefreshHandler(this DlgServerList self, Transform transform, int Index)
		{
			try
			{
				Scroll_Item_ServerInfo scrollItemServerInfo = self.ScrollItemServerInfoDic[Index].BindTrans(transform);
				var serverInfo = self.ZoneScene().GetComponent<ServerInfosComponent>().ServerInfoList[Index];
				scrollItemServerInfo.ELabel_ServerNameText.text = serverInfo.ServerName;
				scrollItemServerInfo.EButton_ServerInfoImage.color =
						serverInfo.Id == self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId? Color.red : Color.cyan;
				scrollItemServerInfo.EButton_ServerInfoButton.AddListener(()=>{self.OnSelectServerItemHandler(serverInfo.Id);});
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

		public static async ETTask OnClickEnterServerBtn(this DlgServerList self)
		{
			bool isSelectServer = self.ZoneScene().GetComponent<ServerInfosComponent>().CurrentServerId!=0;
			if (!isSelectServer)
			{
				Log.Error("未选择区服！");
				return;
			}

			try
			{
				int errorCode = await LoginHelper.GetRoles(self.ZoneScene());
				if (errorCode!=ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}

				await self.DomainScene().GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Roles);
				self.DomainScene().GetComponent<UIComponent>().HideWindow(WindowID.WindowID_ServerList);
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
				return;
			}
		}
	}
}
