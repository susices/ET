using UnityEngine;

namespace ET
{
    
    [ObjectSystem]
    public class UIBaseWindowAwakeSystem : AwakeSystem<UIBaseWindow>
    {
        public override void Awake(UIBaseWindow self)
        {
            self.WindowData = self.AddChild<WindowCoreData>();
        }
    }
    
    public  static class UIBaseWindowSystem  
    {
        public static void SetRoot(this UIBaseWindow self, Transform rootTransform)
        {
            if(self.uiTransform == null)
            {
                Log.Error($"uibaseWindows {self.WindowID} uiTransform is null!!!");
                return;
            }
            if(rootTransform == null)
            {
                Log.Error($"uibaseWindows {self.WindowID} rootTransform is null!!!");
                return;
            }
            self.uiTransform.SetParent(rootTransform, false);
            self.uiTransform.transform.localScale = Vector3.one;
        }

        public static bool IsVisible(this UIBaseWindow self)
        {
            UIComponent uiComponent = self.GetParent<UIComponent>();
            if (uiComponent==null)
            {
                Log.Error("无法获取父级UIComponent");
                return false;
            }

            return uiComponent.VisibleWindowsDic.ContainsKey((int)self.WindowID);
        }
    }
}
