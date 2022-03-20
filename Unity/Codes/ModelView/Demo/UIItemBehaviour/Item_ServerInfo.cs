
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class Scroll_Item_ServerInfo : Entity,IAwake,IDestroy,IUIScrollItem 
	{
		private bool isCacheNode = false;
		public void SetCacheMode(bool isCache)
		{
			this.isCacheNode = isCache;
		}

		public Scroll_Item_ServerInfo BindTrans(Transform trans)
		{
			this.uiTransform = trans;
			return this;
		}

		public UnityEngine.UI.Button EButton_ServerInfoButton
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
     				if( this.m_EButton_ServerInfoButton == null )
     				{
		    			this.m_EButton_ServerInfoButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_ServerInfo");
     				}
     				return this.m_EButton_ServerInfoButton;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"EButton_ServerInfo");
     			}
     		}
     	}

		public UnityEngine.UI.Image EButton_ServerInfoImage
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
     				if( this.m_EButton_ServerInfoImage == null )
     				{
		    			this.m_EButton_ServerInfoImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_ServerInfo");
     				}
     				return this.m_EButton_ServerInfoImage;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"EButton_ServerInfo");
     			}
     		}
     	}

		public UnityEngine.UI.Text ELabel_ServerNameText
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
     				if( this.m_ELabel_ServerNameText == null )
     				{
		    			this.m_ELabel_ServerNameText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_ServerInfo/ELabel_ServerName");
     				}
     				return this.m_ELabel_ServerNameText;
     			}
     			else
     			{
		    		return UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"EButton_ServerInfo/ELabel_ServerName");
     			}
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EButton_ServerInfoButton = null;
			this.m_EButton_ServerInfoImage = null;
			this.m_ELabel_ServerNameText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EButton_ServerInfoButton = null;
		private UnityEngine.UI.Image m_EButton_ServerInfoImage = null;
		private UnityEngine.UI.Text m_ELabel_ServerNameText = null;
		public Transform uiTransform = null;
	}
}
