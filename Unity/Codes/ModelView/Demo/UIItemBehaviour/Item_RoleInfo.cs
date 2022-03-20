
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class Scroll_Item_RoleInfo : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_Item_RoleInfo BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Button EButton_RoleButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_EButton_RoleButton == null )
     				{
		    			this.m_EButton_RoleButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_Role");
     				}
     				return this.m_EButton_RoleButton;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_Role");
     			}
     		}
     	}

		public UnityEngine.UI.Image EButton_RoleImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_EButton_RoleImage == null )
     				{
		    			this.m_EButton_RoleImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_Role");
     				}
     				return this.m_EButton_RoleImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_Role");
     			}
     		}
     	}

		public UnityEngine.UI.Text ELabel_RoleNameText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if (this.isCacheNode)
     			{
     				if( this.m_ELabel_RoleNameText == null )
     				{
		    			this.m_ELabel_RoleNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_Role/ELabel_RoleName");
     				}
     				return this.m_ELabel_RoleNameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_Role/ELabel_RoleName");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EButton_RoleButton = null;
			this.m_EButton_RoleImage = null;
			this.m_ELabel_RoleNameText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EButton_RoleButton = null;
		private UnityEngine.UI.Image m_EButton_RoleImage = null;
		private UnityEngine.UI.Text m_ELabel_RoleNameText = null;
		public Transform uiTransform = null;
	}
}
