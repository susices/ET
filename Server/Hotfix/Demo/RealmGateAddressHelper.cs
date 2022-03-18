using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone, long accountId)
		{
			List<StartSceneConfig> zoneGates =ConfigComponent.Instance.Tables.StartSceneConfigCategory.Gates[zone];

			int n = accountId.GetHashCode() & zoneGates.Count;

			return zoneGates[n];
		}
	}
}
