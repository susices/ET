//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;
using System;


namespace ET.ConfigEditor
{

[Serializable]
public sealed partial class UnitConfig :  Bright.Config.EditorBeanBase 
{
    public UnitConfig()
    {
            Name = "";
            Desc = "";
            NumericValues = System.Array.Empty<long>();
    }

    public override void LoadJson(SimpleJSON.JSONObject _json)
    {
        { 
            var _fieldJson = _json["Id"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  Id = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["Type"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  Type = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["Name"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsString) { throw new SerializationException(); }  Name = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["Desc"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsString) { throw new SerializationException(); }  Desc = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["Position"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  Position = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["NumericValues"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsArray) { throw new SerializationException(); } int _n = _fieldJson.Count; NumericValues = new long[_n]; int _index=0; foreach(SimpleJSON.JSONNode __e in _fieldJson.Children) { long __v;  if(!__e.IsNumber) { throw new SerializationException(); }  __v = __e;  NumericValues[_index++] = __v; }  
            }
        }
        
    }

    public override void SaveJson(SimpleJSON.JSONObject _json)
    {
        {
            _json["Id"] = new JSONNumber(Id);
        }
        {
            _json["Type"] = new JSONNumber(Type);
        }
        {

            if (Name == null) { throw new System.ArgumentNullException(); }
            _json["Name"] = new JSONString(Name);
        }
        {

            if (Desc == null) { throw new System.ArgumentNullException(); }
            _json["Desc"] = new JSONString(Desc);
        }
        {
            _json["Position"] = new JSONNumber(Position);
        }
        {

            if (NumericValues == null) { throw new System.ArgumentNullException(); }
            { var __cjson = new JSONArray(); foreach(var _e in NumericValues) { __cjson["null"] = new JSONNumber(_e); } _json["NumericValues"] = __cjson; }
        }
    }

    public static UnitConfig LoadJsonUnitConfig(SimpleJSON.JSONNode _json)
    {
        UnitConfig obj = new UnitConfig();
        obj.LoadJson((SimpleJSON.JSONObject)_json);
        return obj;
    }
        
    public static void SaveJsonUnitConfig(UnitConfig _obj, SimpleJSON.JSONNode _json)
    {
        _obj.SaveJson((SimpleJSON.JSONObject)_json);
    }

    /// <summary>
    /// Id
    /// </summary>
    public int Id;

    /// <summary>
    /// Type
    /// </summary>
    public int Type;

    /// <summary>
    /// 名字
    /// </summary>
    public string Name;

    /// <summary>
    /// 描述
    /// </summary>
    public string Desc;

    /// <summary>
    /// 位置
    /// </summary>
    public int Position;

    /// <summary>
    /// 数值Values
    /// </summary>
    public long[] NumericValues;

}
}
