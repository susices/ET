using UnityEngine;
namespace ET
{
    public class LocalizationComponentAwakeSystem : AwakeSystem<LocalizationComponent>
    {
        public override void Awake(LocalizationComponent self)
        {
            LocalizationComponent.Instance = self;
            self.Language = Application.systemLanguage;
        }
    }

    /// <summary>
    /// 本地化组件系统
    /// </summary>
    public static class LocalizationComponentSystem
    {
        public static void SetLanguage(this LocalizationComponent self, SystemLanguage language)
        {
            self.Language = language;
        }
        
        public static string GetText(this LocalizationComponent self, int localizationTextId)
        {
            switch (self.Language)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    return LocalizationTextCategory.Instance.Get(localizationTextId).CN;
                case SystemLanguage.English:
                    return LocalizationTextCategory.Instance.Get(localizationTextId).EN;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 通过Id获取文本扩展方法
        /// </summary>
        /// <param name="localizationId"></param>
        /// <returns></returns>
        public static string LocalizedText(this int localizationId)
        {
            return LocalizationComponent.Instance.GetText(localizationId);
        }
        
        
        
    }

}