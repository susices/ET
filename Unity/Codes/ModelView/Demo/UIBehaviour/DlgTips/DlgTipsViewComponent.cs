
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgTipsViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Image E_TipsBackgroundImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_TipsBackgroundImage == null )
     			{
		    		this.m_E_TipsBackgroundImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_TipsBackground");
     			}
     			return this.m_E_TipsBackgroundImage;
     		}
     	}

		public UnityEngine.UI.Text ELabel_TipsTextText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_ELabel_TipsTextText == null )
     			{
		    		this.m_ELabel_TipsTextText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_TipsBackground/ELabel_TipsText");
     			}
     			return this.m_ELabel_TipsTextText;
     		}
     	}

		public UnityEngine.UI.Button EButton_CancelButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CancelButton == null )
     			{
		    		this.m_EButton_CancelButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Cancel");
     			}
     			return this.m_EButton_CancelButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_CancelImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_CancelImage == null )
     			{
		    		this.m_EButton_CancelImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Cancel");
     			}
     			return this.m_EButton_CancelImage;
     		}
     	}

		public UnityEngine.UI.Text E_CancelBtnText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CancelBtnText == null )
     			{
		    		this.m_E_CancelBtnText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Cancel/E_CancelBtn");
     			}
     			return this.m_E_CancelBtnText;
     		}
     	}

		public UnityEngine.UI.Button EButton_ConfirmButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_ConfirmButton == null )
     			{
		    		this.m_EButton_ConfirmButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Confirm");
     			}
     			return this.m_EButton_ConfirmButton;
     		}
     	}

		public UnityEngine.UI.Image EButton_ConfirmImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EButton_ConfirmImage == null )
     			{
		    		this.m_EButton_ConfirmImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Confirm");
     			}
     			return this.m_EButton_ConfirmImage;
     		}
     	}

		public UnityEngine.UI.Text E_ConfirmBtnText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ConfirmBtnText == null )
     			{
		    		this.m_E_ConfirmBtnText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_TipsBackground/EButton_Confirm/E_ConfirmBtn");
     			}
     			return this.m_E_ConfirmBtnText;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_TipsBackgroundImage = null;
			this.m_ELabel_TipsTextText = null;
			this.m_EButton_CancelButton = null;
			this.m_EButton_CancelImage = null;
			this.m_E_CancelBtnText = null;
			this.m_EButton_ConfirmButton = null;
			this.m_EButton_ConfirmImage = null;
			this.m_E_ConfirmBtnText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Image m_E_TipsBackgroundImage = null;
		private UnityEngine.UI.Text m_ELabel_TipsTextText = null;
		private UnityEngine.UI.Button m_EButton_CancelButton = null;
		private UnityEngine.UI.Image m_EButton_CancelImage = null;
		private UnityEngine.UI.Text m_E_CancelBtnText = null;
		private UnityEngine.UI.Button m_EButton_ConfirmButton = null;
		private UnityEngine.UI.Image m_EButton_ConfirmImage = null;
		private UnityEngine.UI.Text m_E_ConfirmBtnText = null;
		public Transform uiTransform = null;
	}
}
