using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	
	public class DlgRolesLoadSystem : LoadSystem<DlgRoles>
	{
		public override void Load(DlgRoles self)
		{
			self.RegisterUIEvent();
			if (self.GetParent<UIBaseWindow>().IsVisible())
			{
				self.RefreshRoleItems();
			}
		}
	}

	public static  class DlgRolesSystem
	{

		public static void RegisterUIEvent(this DlgRoles self)
		{
			self.View.EButton_CreateRoleButton.AddListenerAsync(() => { return self.OnCreateRoleClickHandler();});
			self.View.EButton_DeleteRoleButton.AddListenerAsync(() => { return self.OnDeleteRoleClickHandler();});
			self.View.EButton_EnterGameButton.AddListenerAsync(() => { return self.OnEnterGameClickHandler(); });
			self.View.ELoopScrollList_RolesLoopHorizontalScrollRect.AddItemRefreshListener((Transform transform, int index) =>
			{
				self.OnLoopListItemRefreshHandler(transform,index);
			});
		}

		public static void ShowWindow(this DlgRoles self, Entity contextData = null)
		{
			self.RefreshRoleItems();
		}

		public static void RefreshRoleItems(this DlgRoles self)
		{
			int count = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos.Count;
			self.AddUIScrollItems(ref self.ScrollItemRoleInfos, count);
			self.View.ELoopScrollList_RolesLoopHorizontalScrollRect.SetVisible(true,count);
		}

		public static void OnLoopListItemRefreshHandler(this DlgRoles self, Transform transform, int index)
		{
			try
			{
				Scroll_Item_RoleInfo scrollItemRoleInfo = self.ScrollItemRoleInfos[index].BindTrans(transform);
				var roleInfo = self.ZoneScene().GetComponent<RoleInfosComponent>().RoleInfos[index];
				scrollItemRoleInfo.ELabel_RoleNameText.text = roleInfo.Name;
				scrollItemRoleInfo.EButton_RoleImage.color =
						roleInfo.Id == self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId? Color.red : Color.cyan;
				scrollItemRoleInfo.EButton_RoleButton.AddListener(() =>
				{
					self.OnClickRoleHandler(roleInfo.Id);
				});
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
		}

		public static void OnClickRoleHandler(this DlgRoles self, long selectRoleId)
		{
			self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId = selectRoleId;
			Log.Debug($"当前选择的角色Id是 {selectRoleId.ToString()} xxxxx");
			self.View.ELoopScrollList_RolesLoopHorizontalScrollRect.RefillCells();
		}

		public static async ETTask OnCreateRoleClickHandler(this DlgRoles self)
		{
			try
			{
				string name = self.View.E_NameInputField.text;
				if (string.IsNullOrEmpty(name))
				{
					Log.Error("Name is Null");
					return;
				}

				int errorCode = await LoginHelper.CreateRole(self.ZoneScene(), name);
				if (errorCode!=ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}
				self.RefreshRoleItems();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
			await ETTask.CompletedTask;
		}

		public static async ETTask OnDeleteRoleClickHandler(this DlgRoles self)
		{
			try
			{
				if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
				{
					Log.Error("没有选择角色");
					return;
				}

				int result = await DlgTipsHelper.ShowTips(self.ZoneScene(),"确定要删除角色吗？？？？");
				if (result!=TipsResultType.Confirm)
				{
					return;
				}

				int errorCode = await LoginHelper.DeleteRole(self.ZoneScene());
				if (errorCode!=ErrorCode.ERR_Success)
				{
					Log.Error(errorCode.ToString());
					return;
				}
				self.RefreshRoleItems();
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
			}
			await ETTask.CompletedTask;
		}

		public static async ETTask OnEnterGameClickHandler(this DlgRoles self)
		{
			if (self.ZoneScene().GetComponent<RoleInfosComponent>().CurrentRoleId == 0)
			{
				Log.Error("没有选择角色");
				return;
			}

			try
			{
				{
					int errorCode = await LoginHelper.GetRealmKey(self.ZoneScene());
					if (errorCode!=ErrorCode.ERR_Success)
					{
						Log.Error(errorCode.ToString());
						return;
					}
				}

				{
					int errorCode = await LoginHelper.EnterGame(self.ZoneScene());
					if (errorCode!=ErrorCode.ERR_Success)
					{
						Log.Error(errorCode.ToString());
						return;
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e.ToString());
				return;
			}
			await ETTask.CompletedTask;
			
		}
	}
}
