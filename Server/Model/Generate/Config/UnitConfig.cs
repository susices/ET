//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;



namespace ET
{

public sealed partial class UnitConfig :  Bright.Config.BeanBase 
{
    public UnitConfig(ByteBuf _buf) 
    {
        Id = _buf.ReadInt();
        Type = _buf.ReadInt();
        Name = _buf.ReadString();
        Desc = _buf.ReadString();
        Position = _buf.ReadInt();
        {int n = System.Math.Min(_buf.ReadSize(), _buf.Size);NumericValues = new long[n];for(var i = 0 ; i < n ; i++) { long _e;_e = _buf.ReadLong(); NumericValues[i] = _e;}}
        PostInit();
    }

    public static UnitConfig DeserializeUnitConfig(ByteBuf _buf)
    {
        return new UnitConfig(_buf);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// Type
    /// </summary>
    public int Type { get; private set; }
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// 描述
    /// </summary>
    public string Desc { get; private set; }
    /// <summary>
    /// 位置
    /// </summary>
    public int Position { get; private set; }
    /// <summary>
    /// 数值Values
    /// </summary>
    public long[] NumericValues { get; private set; }

    public const int __ID__ = -568528378;
    public override int GetTypeId() => __ID__;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        PostResolve();
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Type:" + Type + ","
        + "Name:" + Name + ","
        + "Desc:" + Desc + ","
        + "Position:" + Position + ","
        + "NumericValues:" + Bright.Common.StringUtil.CollectionToString(NumericValues) + ","
        + "}";
    }
    
    partial void PostInit();
    partial void PostResolve();
}

}
