namespace ET
{
    /// <summary>
    /// Unit信息组件
    /// 保存一些unit要使用的数据
    /// </summary>
    public class UnitInfoComponent:Entity,ISerializeToEntity
    {
        public long PlayerId;
        public int MapIndex;
    }
}