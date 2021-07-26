using UnityEngine.UI;

namespace ET
{
    public class RedDotUIEntityAwakeSystem:AwakeSystem<RedDotUIEntity, string, Image, Text>
    {
        public override void Awake(RedDotUIEntity self, string path, Image image, Text text)
        {
            self.Path = path;
            self.RedDotImage = image;
            self.RedDotText = text;
            self.HasRedDotText = self.RedDotText != null;
            RedDotManagerComponent.Instance.RegisterRedDotUIEntities(path, self);
        }
    }
    
    public class RedDotUIEntityDestroySystem: DestroySystem<RedDotUIEntity>
    {
        public override void Destroy(RedDotUIEntity self)
        {
            RedDotManagerComponent.Instance.UnRegisterRedDotUIEntities(self.Path, self);
        }
    }

    public static class RedDotUIEntitySystem
    {
        public static void OnNodeValueChange(this RedDotUIEntity self, int newValue)
        {
            if (newValue==0)
            {
                self.RedDotImage.canvasRenderer.cull = true;
            }
            else
            {
                self.RedDotImage.canvasRenderer.cull = false;
                if (self.HasRedDotText)
                {
                    self.RedDotText.text = newValue.ToString();
                }
            }
        }
    }
}