using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone, long accountId)
		{
			List<StartSceneConfig> zoneGates =ConfigComponent.Instance.Tables.StartSceneConfigCategory.Gates[zone];

			int n = accountId.GetHashCode() % zoneGates.Count;

			return zoneGates[n];
		}

		public static StartSceneConfig GetRealm(long accountId)
		{
			List<StartSceneConfig> realms = ConfigComponent.Instance.Tables.StartSceneConfigCategory.Realms;
			int n = accountId.GetHashCode() % realms.Count;
			return realms[n];
		}
	}
}
