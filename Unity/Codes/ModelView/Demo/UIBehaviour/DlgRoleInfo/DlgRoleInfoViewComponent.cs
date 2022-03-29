
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

		public ES_AttributeItem ES_AttributeItem1
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitem1 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItem1");
		    	   this.m_es_attributeitem1 = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitem1;
     		}
     	}

		public ES_AttributeItem ES_AttributeItem2
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitem2 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItem2");
		    	   this.m_es_attributeitem2 = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitem2;
     		}
     	}

		public ES_AttributeItem ES_AttributeItem3
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_es_attributeitem3 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"BackGround/Panel/ES_AttributeItem3");
		    	   this.m_es_attributeitem3 = this.AddChild<ES_AttributeItem,Transform>(subTrans);
     			}
     			return this.m_es_attributeitem3;
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

		public void DestroyWidget()
		{
			this.m_E_FightValueText = null;
			this.m_es_attributeitemstrength?.Dispose();
			this.m_es_attributeitemstrength = null;
			this.m_es_attributeitem1?.Dispose();
			this.m_es_attributeitem1 = null;
			this.m_es_attributeitem2?.Dispose();
			this.m_es_attributeitem2 = null;
			this.m_es_attributeitem3?.Dispose();
			this.m_es_attributeitem3 = null;
			this.m_E_DamageValueText = null;
			this.m_E_DamageAddValueText = null;
			this.m_E_HPValueText = null;
			this.m_EnergyImage = null;
			this.m_E_EnergyValueText = null;
			this.m_E_DefenseValueText = null;
			this.m_E_DefenseAddValueText = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Text m_E_FightValueText = null;
		private ES_AttributeItem m_es_attributeitemstrength = null;
		private ES_AttributeItem m_es_attributeitem1 = null;
		private ES_AttributeItem m_es_attributeitem2 = null;
		private ES_AttributeItem m_es_attributeitem3 = null;
		private UnityEngine.UI.Text m_E_DamageValueText = null;
		private UnityEngine.UI.Text m_E_DamageAddValueText = null;
		private UnityEngine.UI.Text m_E_HPValueText = null;
		private UnityEngine.UI.Image m_EnergyImage = null;
		private UnityEngine.UI.Text m_E_EnergyValueText = null;
		private UnityEngine.UI.Text m_E_DefenseValueText = null;
		private UnityEngine.UI.Text m_E_DefenseAddValueText = null;
		public Transform uiTransform = null;
	}
}
