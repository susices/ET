
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgServerListViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button EButton_EnterServerButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_EnterServerButton == null )
     			{
		    		this.m_EButton_EnterServerButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"BackGround/EButton_EnterServer");
     			}
     			return this.m_EButton_EnterServerButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_EnterServerImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_EnterServerImage == null )
     			{
		    		this.m_EButton_EnterServerImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/EButton_EnterServer");
     			}
     			return this.m_EButton_EnterServerImage;
     		}
     	}

		public UnityEngine.UI.LoopVerticalScrollRect ELoopScrollList_ServerListLoopVerticalScrollRect
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELoopScrollList_ServerListLoopVerticalScrollRect == null )
     			{
		    		this.m_ELoopScrollList_ServerListLoopVerticalScrollRect = UIFindHelper.FindDeepChild<UnityEngine.UI.LoopVerticalScrollRect>(this.uiTransform.gameObject,"BackGround/ELoopScrollList_ServerList");
     			}
     			return this.m_ELoopScrollList_ServerListLoopVerticalScrollRect;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_EButton_EnterServerButton = null;
			this.m_EButton_EnterServerImage = null;
			this.m_ELoopScrollList_ServerListLoopVerticalScrollRect = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_EButton_EnterServerButton = null;
		private UnityEngine.UI.Image m_EButton_EnterServerImage = null;
		private UnityEngine.UI.LoopVerticalScrollRect m_ELoopScrollList_ServerListLoopVerticalScrollRect = null;
		public Transform uiTransform = null;
	}
}
