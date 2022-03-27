
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgMainViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Text E_RoleLevelText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleLevelText == null )
     			{
		    		this.m_E_RoleLevelText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Center/Top/Image/E_RoleLevel");
     			}
     			return this.m_E_RoleLevelText;
     		}
     	}

		public UnityEngine.UI.Text E_GoldText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_GoldText == null )
     			{
		    		this.m_E_GoldText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Center/Top/Image (1)/E_Gold");
     			}
     			return this.m_E_GoldText;
     		}
     	}

		public UnityEngine.UI.Text E_ExpText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ExpText == null )
     			{
		    		this.m_E_ExpText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Center/Top/Image (2)/E_Exp");
     			}
     			return this.m_E_ExpText;
     		}
     	}

		public UnityEngine.UI.Button E_RoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleButton == null )
     			{
		    		this.m_E_RoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Center/Bottom/E_Role");
     			}
     			return this.m_E_RoleButton;
     		}
     	}

		public UnityEngine.UI.Image E_RoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleImage == null )
     			{
		    		this.m_E_RoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Center/Bottom/E_Role");
     			}
     			return this.m_E_RoleImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_RoleLevelText = null;
			this.m_E_GoldText = null;
			this.m_E_ExpText = null;
			this.m_E_RoleButton = null;
			this.m_E_RoleImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_RoleLevelText = null;
		private UnityEngine.UI.Text m_E_GoldText = null;
		private UnityEngine.UI.Text m_E_ExpText = null;
		private UnityEngine.UI.Button m_E_RoleButton = null;
		private UnityEngine.UI.Image m_E_RoleImage = null;
		public Transform uiTransform = null;
	}
}
