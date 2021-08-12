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

            if (string.IsNullOrEmpty(text))
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
        /// 获取本地化资源路径
        /// </summary>
        /// <param name="self"></param>
        /// <param name="localizationArtAssetId"></param>
        /// <returns></returns>
        public static string GetAssetPath(this LocalizationComponent self, int localizationArtAssetId)
        {
            switch (localizationArtAssetId)
            {
                case var id when id<=10000:
                    return self.GetUIAssetPath(id);
                case var id when id<=20000:
                    return self.GetSceneAssetPath(id);
                case var id when id<=40000:
                    return self.GetModelAssetPath(id);
                case var id when id<50000:
                    return self.GetAudioAssetPath(id);
                default:
                    return null;
            }
        }

        public static string GetUIAssetPath(this LocalizationComponent self, int localizationAudioAssetId)
        {
            string path = null;
            var config = LocalizationUICategory.Instance.Get(localizationAudioAssetId);
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
            if (string.IsNullOrEmpty(path))
            {
                path = config.Default;
            }
            return path;
        }
        
        public static string GetSceneAssetPath(this LocalizationComponent self, int localizationSceneAssetId)
        {
            string path = null;
            var config = LocalizationSceneCategory.Instance.Get(localizationSceneAssetId);
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
            if (string.IsNullOrEmpty(path))
            {
                path = config.Default;
            }
            return path;
        }
        
        public static string GetModelAssetPath(this LocalizationComponent self, int localizationModelAssetId)
        {
            string path = null;
            var config = LocalizationModelCategory.Instance.Get(localizationModelAssetId);
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
            if (string.IsNullOrEmpty(path))
            {
                path = config.Default;
            }
            return path;
        }
        
        /// <summary>
        /// 获取本地化音频资源路径
        /// </summary>
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
            if (string.IsNullOrEmpty(path))
            {
                path = config.Default;
            }
            return path;
        }
        
        /// <summary>
        /// 获取本地化资源路径扩展方法
        /// </summary>
        /// <param name="localizationAssetId"></param>
        /// <returns></returns>
        public static string LocalizedAssetPath(this int localizationAssetId)
        {
            return LocalizationComponent.Instance.GetAssetPath(localizationAssetId);
        }

        public static int CachePoolMillSeconds(this LocalizationComponent self, int localizationAssetId)
        {
            switch (localizationAssetId)
            {
                case var id when id<=10000:
                    return LocalizationUICategory.Instance.Get(localizationAssetId).CachePoolMillSeconds;
                case var id when id<=20000:
                    return LocalizationSceneCategory.Instance.Get(localizationAssetId).CachePoolMillSeconds;
                case var id when id<=40000:
                    return LocalizationModelCategory.Instance.Get(localizationAssetId).CachePoolMillSeconds;
                default:
                    return 0;
            }
        }
    }
}