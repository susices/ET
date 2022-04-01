using System;

namespace ET
{
    [Timer(TimerType.HideUITimeOut)]
    public class HideUITimeOutTimer: ATimer<HideUITimeOutComponent>
    {
        public override void Run(HideUITimeOutComponent self)
        {
            try
            {
                if (self.Parent is UIBaseWindow uiBaseWindow)
                {
                    self.DomainScene().GetComponent<UIComponent>().UnLoadWindow(uiBaseWindow.m_windowID);
                }
                else
                {
                    Log.Error("找不到UIBaseWindow父级");
                }
            }
            catch (Exception e)
            {
                Log.Error($"timer error: {self.Id}\n{e}");
            }
        }
    }

    public class HideUITimeOutComponentAwakeSystem: AwakeSystem<HideUITimeOutComponent>
    {
        public override void Awake(HideUITimeOutComponent self)
        {
            self.Timer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 30000, TimerType.HideUITimeOut, self);
        }
    }
    
    public class HideUITimeOutComponentDestroySystem : DestroySystem<HideUITimeOutComponent>
    {
        public override void Destroy(HideUITimeOutComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }
}