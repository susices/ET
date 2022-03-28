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
public sealed partial class StartZoneConfig :  Bright.Config.EditorBeanBase 
{
    public StartZoneConfig()
    {
            DBName = "";
            DBConnection = "";
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
            var _fieldJson = _json["ZoneType"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsNumber) { throw new SerializationException(); }  ZoneType = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["DBName"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsString) { throw new SerializationException(); }  DBName = _fieldJson;
            }
        }
        
        { 
            var _fieldJson = _json["DBConnection"];
            if (_fieldJson != null)
            {
                if(!_fieldJson.IsString) { throw new SerializationException(); }  DBConnection = _fieldJson;
            }
        }
        
    }

    public override void SaveJson(SimpleJSON.JSONObject _json)
    {
        {
            _json["Id"] = new JSONNumber(Id);
        }
        {
            _json["ZoneType"] = new JSONNumber(ZoneType);
        }
        {

            if (DBName == null) { throw new System.ArgumentNullException(); }
            _json["DBName"] = new JSONString(DBName);
        }
        {

            if (DBConnection == null) { throw new System.ArgumentNullException(); }
            _json["DBConnection"] = new JSONString(DBConnection);
        }
    }

    public static StartZoneConfig LoadJsonStartZoneConfig(SimpleJSON.JSONNode _json)
    {
        StartZoneConfig obj = new StartZoneConfig();
        obj.LoadJson((SimpleJSON.JSONObject)_json);
        return obj;
    }
        
    public static void SaveJsonStartZoneConfig(StartZoneConfig _obj, SimpleJSON.JSONNode _json)
    {
        _obj.SaveJson((SimpleJSON.JSONObject)_json);
    }

    /// <summary>
    /// 区服Id 最大1024个
    /// </summary>
    public int Id;

    /// <summary>
    /// 区服类型
    /// </summary>
    public int ZoneType;

    /// <summary>
    /// 数据库名
    /// </summary>
    public string DBName;

    /// <summary>
    /// 数据库地址
    /// </summary>
    public string DBConnection;

}
}
