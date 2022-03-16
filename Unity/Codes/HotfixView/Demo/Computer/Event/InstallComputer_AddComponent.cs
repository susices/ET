using ET.EventType;

namespace ET
{
    public class InstallComputer_AddComponent : AEvent<InstallComputer>
    {
        protected override async ETTask Run(InstallComputer a)
        {
            await ETTask.CompletedTask;
            Computer computer = a.Computer;
            computer.AddComponent<PCCaseComponent>();
            computer.AddComponent<MonitorsComponent>();
            computer.AddComponent<MouseComponent>();
            computer.AddComponent<KeyBoardComponent>();
            computer.Start();
        }
    }
}