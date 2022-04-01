using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public static  class DlgRoleInfoSystem
	{

		public static void RegisterUIEvent(this DlgRoleInfo self)
		{
			self.View.ES_AttributeItemStrength.RegisterEvent(NumericType.Strength);
			self.View.ES_AttributeItemVitality.RegisterEvent(NumericType.Vitality);
			self.View.ES_AttributeItemDexterity.RegisterEvent(NumericType.Dexterity);
			self.View.ES_AttributeItemSpirit.RegisterEvent(NumericType.Spirit);
			self.RegisterCloseEvent<DlgRoleInfo>(self.View.E_CloseBtnButton);
		}

		public static void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
		{
			self.Refresh();
		}

		public static void Refresh(this DlgRoleInfo self)
		{
			self.View.ES_AttributeItemStrength.Refresh(NumericType.Strength);
			self.View.ES_AttributeItemVitality.Refresh(NumericType.Vitality);
			self.View.ES_AttributeItemDexterity.Refresh(NumericType.Dexterity);
			self.View.ES_AttributeItemSpirit.Refresh(NumericType.Spirit);
			NumericComponent numericComponent =
					UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene()).GetComponent<NumericComponent>();
			self.View.E_FightValueText.text = numericComponent.GetAsLong(NumericType.FightValue).ToString();
			self.View.E_AttributePointValueText.text = numericComponent.GetAsInt(NumericType.AttributePoint).ToString();
		}
	}
}
