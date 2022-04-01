
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	public  class DlgRoleInfoViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Text E_FightValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_FightValueText == null )
     			{
		    		this.m_E_FightValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/FightValue/E_FightValue");
     			}
     			return this.m_E_FightValueText;
     		}
     	}

		public UnityEngine.UI.Text E_AttributePointValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_AttributePointValueText == null )
     			{
		    		this.m_E_AttributePointValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/AttributePoint/E_AttributePointValue");
     			}
     			return this.m_E_AttributePointValueText;
     		}
     	}

		public ES_AttributeItem ES_AttributeItemStrength
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitemstrength == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItemStrength");
		    	   this.m_es_attributeitemstrength = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitemstrength;
     		}
     	}

		public ES_AttributeItem ES_AttributeItemVitality
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitemvitality == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItemVitality");
		    	   this.m_es_attributeitemvitality = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitemvitality;
     		}
     	}

		public ES_AttributeItem ES_AttributeItemDexterity
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitemdexterity == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItemDexterity");
		    	   this.m_es_attributeitemdexterity = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitemdexterity;
     		}
     	}

		public ES_AttributeItem ES_AttributeItemSpirit
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitemspirit == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItemSpirit");
		    	   this.m_es_attributeitemspirit = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitemspirit;
     		}
     	}

		public UnityEngine.UI.Text E_DamageValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DamageValueText == null )
     			{
		    		this.m_E_DamageValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/Damage/E_DamageValue");
     			}
     			return this.m_E_DamageValueText;
     		}
     	}

		public UnityEngine.UI.Text E_DamageAddValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DamageAddValueText == null )
     			{
		    		this.m_E_DamageAddValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/DamageAdd/E_DamageAddValue");
     			}
     			return this.m_E_DamageAddValueText;
     		}
     	}

		public UnityEngine.UI.Text E_HPValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_HPValueText == null )
     			{
		    		this.m_E_HPValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/HP/E_HPValue");
     			}
     			return this.m_E_HPValueText;
     		}
     	}

		public UnityEngine.UI.Image EnergyImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_EnergyImage == null )
     			{
		    		this.m_EnergyImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/Panel/Energy");
     			}
     			return this.m_EnergyImage;
     		}
     	}

		public UnityEngine.UI.Text E_EnergyValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_EnergyValueText == null )
     			{
		    		this.m_E_EnergyValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/Energy/E_EnergyValue");
     			}
     			return this.m_E_EnergyValueText;
     		}
     	}

		public UnityEngine.UI.Text E_DefenseValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DefenseValueText == null )
     			{
		    		this.m_E_DefenseValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/Defense/E_DefenseValue");
     			}
     			return this.m_E_DefenseValueText;
     		}
     	}

		public UnityEngine.UI.Text E_DefenseAddValueText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_DefenseAddValueText == null )
     			{
		    		this.m_E_DefenseAddValueText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"BackGround/Panel/DefenseAdd/E_DefenseAddValue");
     			}
     			return this.m_E_DefenseAddValueText;
     		}
     	}

		public UnityEngine.UI.Button E_CloseBtnButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseBtnButton == null )
     			{
		    		this.m_E_CloseBtnButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"BackGround/Panel/E_CloseBtn");
     			}
     			return this.m_E_CloseBtnButton;
     		}
     	}

		public UnityEngine.UI.Image E_CloseBtnImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CloseBtnImage == null )
     			{
		    		this.m_E_CloseBtnImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"BackGround/Panel/E_CloseBtn");
     			}
     			return this.m_E_CloseBtnImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_FightValueText = null;
			this.m_E_AttributePointValueText = null;
			this.m_es_attributeitemstrength?.Dispose();
			this.m_es_attributeitemstrength = null;
			this.m_es_attributeitemvitality?.Dispose();
			this.m_es_attributeitemvitality = null;
			this.m_es_attributeitemdexterity?.Dispose();
			this.m_es_attributeitemdexterity = null;
			this.m_es_attributeitemspirit?.Dispose();
			this.m_es_attributeitemspirit = null;
			this.m_E_DamageValueText = null;
			this.m_E_DamageAddValueText = null;
			this.m_E_HPValueText = null;
			this.m_EnergyImage = null;
			this.m_E_EnergyValueText = null;
			this.m_E_DefenseValueText = null;
			this.m_E_DefenseAddValueText = null;
			this.m_E_CloseBtnButton = null;
			this.m_E_CloseBtnImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_FightValueText = null;
		private UnityEngine.UI.Text m_E_AttributePointValueText = null;
		private ES_AttributeItem m_es_attributeitemstrength = null;
		private ES_AttributeItem m_es_attributeitemvitality = null;
		private ES_AttributeItem m_es_attributeitemdexterity = null;
		private ES_AttributeItem m_es_attributeitemspirit = null;
		private UnityEngine.UI.Text m_E_DamageValueText = null;
		private UnityEngine.UI.Text m_E_DamageAddValueText = null;
		private UnityEngine.UI.Text m_E_HPValueText = null;
		private UnityEngine.UI.Image m_EnergyImage = null;
		private UnityEngine.UI.Text m_E_EnergyValueText = null;
		private UnityEngine.UI.Text m_E_DefenseValueText = null;
		private UnityEngine.UI.Text m_E_DefenseAddValueText = null;
		private UnityEngine.UI.Button m_E_CloseBtnButton = null;
		private UnityEngine.UI.Image m_E_CloseBtnImage = null;
		public Transform uiTransform = null;
	}
}
