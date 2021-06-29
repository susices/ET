using System;

namespace ET
{
    

    [ObjectSystem]
    public class BuffActionDispatcherComponentLoadSystem: LoadSystem<BuffActionDispatcherComponent>
    {
        public override void Load(BuffActionDispatcherComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherComponentAwakeSystem: AwakeSystem<BuffActionDispatcherComponent>
    {
        public override void Awake(BuffActionDispatcherComponent self)
        {
            
        }
    }

    [ObjectSystem]
    public class BuffActionDispatcherComponentDestroySystem: DestroySystem<BuffActionDispatcherComponent>
    {
        public override void Destroy(BuffActionDispatcherComponent self)
        {
            
        }
    }
    
    
    public static class BuffActionDispatcherComponentSystem
    {
        public static void Load(this BuffActionDispatcherComponent self)
        {
            self.BuffActions.Clear();

            var types = Game.EventSystem.GetTypes(typeof (BaseBuffActionAttribute));
            foreach (Type type in types)
            {
                ABuffAction aBuffAction = Activator.CreateInstance(type) as ABuffAction;
                if (aBuffAction==null)
                {
                    Log.Error($"{type.Name} is not a BuffAction!");
                    continue;
                }
                self.BuffActions.Add(type.Name, aBuffAction);
            }
        }
    }
    
}