using System.Collections.Generic;

namespace ET
{
	public  class DlgLogin :Entity,IAwake,IUILogic, ILoad
	{

		public DlgLoginViewComponent View { get => this.Parent.GetComponent<DlgLoginViewComponent>();} 

		
	}
}
