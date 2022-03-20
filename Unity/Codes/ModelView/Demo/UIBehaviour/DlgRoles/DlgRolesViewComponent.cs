
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgRolesViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button EButton_DeleteRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_DeleteRoleButton == null )
     			{
		    		this.m_EButton_DeleteRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"BackGround/EButton_DeleteRole");
     			}
     			return this.m_EButton_DeleteRoleButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_DeleteRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_DeleteRoleImage == null )
     			{
		    		this.m_EButton_DeleteRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/EButton_DeleteRole");
     			}
     			return this.m_EButton_DeleteRoleImage;
     		}
     	}

		public UnityEngine.UI.Button EButton_CreateRoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CreateRoleButton == null )
     			{
		    		this.m_EButton_CreateRoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"BackGround/EButton_CreateRole");
     			}
     			return this.m_EButton_CreateRoleButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_CreateRoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CreateRoleImage == null )
     			{
		    		this.m_EButton_CreateRoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/EButton_CreateRole");
     			}
     			return this.m_EButton_CreateRoleImage;
     		}
     	}

		public UnityEngine.UI.Button EButton_EnterGameButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_EnterGameButton == null )
     			{
		    		this.m_EButton_EnterGameButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"BackGround/EButton_EnterGame");
     			}
     			return this.m_EButton_EnterGameButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_EnterGameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_EnterGameImage == null )
     			{
		    		this.m_EButton_EnterGameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/EButton_EnterGame");
     			}
     			return this.m_EButton_EnterGameImage;
     		}
     	}

		public UnityEngine.UI.InputField E_NameInputField
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_NameInputField == null )
     			{
		    		this.m_E_NameInputField = UIFindHelper.FindDeepChild<UnityEngine.UI.InputField>(this.uiTransform.gameObject,"BackGround/E_Name");
     			}
     			return this.m_E_NameInputField;
     		}
     	}

		public UnityEngine.UI.Image E_NameImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_NameImage == null )
     			{
		    		this.m_E_NameImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/E_Name");
     			}
     			return this.m_E_NameImage;
     		}
     	}

		public UnityEngine.UI.LoopHorizontalScrollRect ELoopScrollList_RolesLoopHorizontalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELoopScrollList_RolesLoopHorizontalScrollRect == null )
     			{
		    		this.m_ELoopScrollList_RolesLoopHorizontalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopHorizontalScrollRect>(this.uiTransform.gameObject,"BackGround/ELoopScrollList_Roles");
     			}
     			return this.m_ELoopScrollList_RolesLoopHorizontalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EButton_DeleteRoleButton = null;
			this.m_EButton_DeleteRoleImage = null;
			this.m_EButton_CreateRoleButton = null;
			this.m_EButton_CreateRoleImage = null;
			this.m_EButton_EnterGameButton = null;
			this.m_EButton_EnterGameImage = null;
			this.m_E_NameInputField = null;
			this.m_E_NameImage = null;
			this.m_ELoopScrollList_RolesLoopHorizontalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EButton_DeleteRoleButton = null;
		private UnityEngine.UI.Image m_EButton_DeleteRoleImage = null;
		private UnityEngine.UI.Button m_EButton_CreateRoleButton = null;
		private UnityEngine.UI.Image m_EButton_CreateRoleImage = null;
		private UnityEngine.UI.Button m_EButton_EnterGameButton = null;
		private UnityEngine.UI.Image m_EButton_EnterGameImage = null;
		private UnityEngine.UI.InputField m_E_NameInputField = null;
		private UnityEngine.UI.Image m_E_NameImage = null;
		private UnityEngine.UI.LoopHorizontalScrollRect m_ELoopScrollList_RolesLoopHorizontalScrollRect = null;
		public Transform uiTransform = null;
	}
}
