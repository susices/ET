﻿using System.Collections.Generic;

namespace ET
{
	public  class DlgServerList :Entity,IAwake,IUILogic, ILoad
	{

		public DlgServerListViewComponent View { get => this.Parent.GetComponent<DlgServerListViewComponent>();}

		public Dictionary<int, Scroll_Item_ServerInfo> ScrollItemServerInfoDic;

	}
}
