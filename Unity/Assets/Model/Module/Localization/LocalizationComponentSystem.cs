using UnityEngine;

namespace ET
{
    public class LocalizationComponentAwakeSystem: AwakeSystem<LocalizationComponent>
    {
        public override void Awake(LocalizationComponent self)
        {
            LocalizationComponent.Instance = self;
            self.LanguageTextArt = Application.systemLanguage;
            self.LanguageAudio = Application.systemLanguage;
        }
    }

    /// <summary>
    /// 本地化组件系统
    /// </summary>
    public static class LocalizationComponentSystem
    {
        /// <summary>
        /// 设置文本美术
        /// </summary>
        /// <param name="self"></param>
        /// <param name="language"></param>
        public static void SetLanguageTextArt(this LocalizationComponent self, SystemLanguage language)
        {
            self.LanguageTextArt = language;
        }

        /// <summary>
        /// 设置音频
        /// </summary>
        /// <param name="self"></param>
        /// <param name="language"></param>
        public static void SetLanguageAudio(this LocalizationComponent self, SystemLanguage language)
        {
            self.LanguageAudio = language;
        }

        /// <summary>
        /// 获取本地化文本
        /// </summary>
        /// <param name="self"></param>
        /// <param name="localizationTextId"></param>
        /// <returns></returns>
        public static string GetText(this LocalizationComponent self, int localizationTextId)
        {
            string text = null;
            LocalizationText config = LocalizationTextCategory.Instance.Get(localizationTextId);
            switch (self.LanguageTextArt)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    text = config.CN;
                    break;
                case SystemLanguage.English:
                    text = config.EN;
                    break;
            }

            if (text == null)
            {
                text = config.Default;
            }

            return text;
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

        /// <summary>
        /// 获取本地化美术资源路径
        /// </summary>
        /// <param name="self"></param>
        /// <param name="localizationArtAssetId"></param>
        /// <returns></returns>
        public static string GetArtAssetPath(this LocalizationComponent self, int localizationArtAssetId)
        {
            string path = null;
            LocalizationArtAsset config = LocalizationArtAssetCategory.Instance.Get(localizationArtAssetId);
            switch (self.LanguageTextArt)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    path = config.CN;
                    break;
                case SystemLanguage.English:
                    path = config.EN;
                    break;
            }

            if (path == null)
            {
                path = config.Default;
            }

            return path;
        }

        /// <summary>
        /// 获取本地化音频资源路径
        /// </summary>
        /// <param name="self"></param>
        /// <param name="localizationAudioAssetId"></param>
        /// <returns></returns>
        public static string GetAudioAssetPath(this LocalizationComponent self, int localizationAudioAssetId)
        {
            string path = null;
            LocalizationAudio config = LocalizationAudioCategory.Instance.Get(localizationAudioAssetId);
            switch (self.LanguageTextArt)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    path = config.CN;
                    break;
                case SystemLanguage.English:
                    path = config.EN;
                    break;
            }

            if (path == null)
            {
                path = config.Default;
            }

            return path;
        }
    }
}