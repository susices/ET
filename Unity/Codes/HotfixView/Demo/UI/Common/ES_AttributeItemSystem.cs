using System;

namespace ET
{
    public static class ES_AttributeItemSystem
    {
        public static void RegisterEvent(this ES_AttributeItem self, int numericType)
        {
            self.E_AttributeBtnButton.AddListenerAsync(() => { return self.RequestAddAttribute(numericType);});
        }

        public static async ETTask RequestAddAttribute(this ES_AttributeItem self, int numericType)
        {
            try
            {
                int errorCode = await NumericHelper.RequestAddAttributePoint(self.ZoneScene(), numericType);
                if (errorCode!=ErrorCode.ERR_Success)
                {
                    Log.Error(errorCode.ToString());
                    return;
                }
                Log.Debug("加点成功");
                self.ZoneScene().GetComponent<UIComponent>().GetDlgLogic<DlgRoleInfo>().Refresh();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return;
            }
        }

        public static void Refresh(this ES_AttributeItem self, int numericType)
        {
            self.E_AttributeValueText.text = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene()).GetComponent<NumericComponent>()
                    .GetAsLong(numericType).ToString();
        }
    }
}