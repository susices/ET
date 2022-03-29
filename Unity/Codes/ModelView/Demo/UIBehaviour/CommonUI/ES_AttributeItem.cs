
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class ES_AttributeItem : Entity,ET.IAwake<UnityEngine.Transform>,IDestroy 
	{
		public UnityEngine.UI.Text E_AttributeValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AttributeValueText == null )
     			{
		    		this.m_E_AttributeValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"E_AttributeValue");
     			}
     			return this.m_E_AttributeValueText;
     		}
     	}

		public UnityEngine.UI.Button E_AttributeBtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AttributeBtnButton == null )
     			{
		    		this.m_E_AttributeBtnButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_AttributeBtn");
     			}
     			return this.m_E_AttributeBtnButton;
     		}
     	}

		public UnityEngine.UI.Image E_AttributeBtnImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AttributeBtnImage == null )
     			{
		    		this.m_E_AttributeBtnImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_AttributeBtn");
     			}
     			return this.m_E_AttributeBtnImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_AttributeValueText = null;
			this.m_E_AttributeBtnButton = null;
			this.m_E_AttributeBtnImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_AttributeValueText = null;
		private UnityEngine.UI.Button m_E_AttributeBtnButton = null;
		private UnityEngine.UI.Image m_E_AttributeBtnImage = null;
		public Transform uiTransform = null;
	}
}
