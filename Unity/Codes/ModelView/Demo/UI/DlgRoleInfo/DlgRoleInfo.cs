namespace ET
{
	public  class DlgRoleInfo :Entity,IAwake,IUILogic
	{

		public DlgRoleInfoViewComponent View { get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();} 

		 

	}
}
