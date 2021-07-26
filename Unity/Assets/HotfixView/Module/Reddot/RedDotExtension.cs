using UnityEngine.UI;

namespace ET
{
    public static class RedDotExtension
    {
        public static void CreateRedDotUI(this UIPanel self, string path, Image image, Text text = null)
        {
            EntityFactory.CreateWithParent<RedDotUIEntity, string, Image, Text>(self, path, image, text);
        }
    }
}