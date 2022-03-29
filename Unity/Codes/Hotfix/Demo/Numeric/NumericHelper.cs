using System;

namespace ET
{
    public static class NumericHelper
    {
        public static async ETTask<int> TestUpdateNumeric(Scene zoneScene)
        {
            M2C_TestUnitNumeric m2CTestUnitNumeric = null;
            try
            {
                m2CTestUnitNumeric = (M2C_TestUnitNumeric)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2M_TestUnitNumeric());
                
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                return ErrorCode.ERR_NetworkError;
            }
            if (m2CTestUnitNumeric.Error!=ErrorCode.ERR_Success)
            {
                return m2CTestUnitNumeric.Error;
            }
            
            return ErrorCode.ERR_Success;
        }
    }
}