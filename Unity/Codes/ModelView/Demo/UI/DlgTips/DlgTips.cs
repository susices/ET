namespace ET
{
	public  class DlgTips :Entity,IAwake,IUILogic
	{

		public DlgTipsViewComponent View { get => this.Parent.GetComponent<DlgTipsViewComponent>();}

		public ETTask<int> Result;
	}
}
