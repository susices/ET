using IngameDebugConsole;

namespace ET
{
    /// <summary>
    /// 运行时命令行组件
    /// </summary>
    public class ConsoleComponent : Entity
    {
        
    }
    
    public class ConsoleComponentAwakeSystem: AwakeSystem<ConsoleComponent>
    {
        public override void Awake(ConsoleComponent self)
        {
            DebugLogConsole.AddCommand("Test", "",((string str) => { Log.Debug(str);}));
        }
    }
}