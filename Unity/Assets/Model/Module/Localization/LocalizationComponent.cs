using UnityEngine;

namespace ET
{
    /// <summary>
    /// 本地化组件
    /// </summary>
    public class LocalizationComponent : Entity
    {
        public static LocalizationComponent Instance;
        public SystemLanguage Language;
    }
    
}