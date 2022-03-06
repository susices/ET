using System;
using NLog;

namespace ET
{
    [ConsoleHandler(ConsoleMode.ReloadConfig)]
    public class ReloadConfigConsoleHandler: IConsoleHandler
    {
        public async ETTask Run(ModeContex contex, string content)
        {
            switch (content)
            {
                case ConsoleMode.ReloadConfig:
                    contex.Parent.RemoveComponent<ModeContex>();
                    ConfigComponent.Instance.Load();
                    Log.Console("ReloadAllTables Success");
                    break;
                default:
                    break;
            }
            
            await ETTask.CompletedTask;
        }
    }
}