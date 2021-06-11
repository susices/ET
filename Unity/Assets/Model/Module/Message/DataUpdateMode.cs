using System;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 数据更新模式
    /// </summary>
    public static class DataUpdateMode
    {
        /// <summary>
        /// 覆盖更新 覆盖原数据列表
        /// </summary>
        public const int Overwrite = 1;
        
        /// <summary>
        /// 差异更新 只对指定数据项列表进行覆盖 
        /// </summary>
        public const int Difference = 2;
        
    }
}