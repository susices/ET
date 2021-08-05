using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ET;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
    public class ExcelExportEditor : Editor
    {
        [MenuItem("/Tools/导出Excel")]
        public static void ExcelExport()
        {
            var process =  ProcessHelper.Run("ExcelExporter.exe", $"", "../Tools/ExcelExporter/Bin/");
            Debug.Log("导出开始");
            while (!process.HasExited)
            {
                Thread.Sleep(1000);
            }
            Debug.Log("导出完成");
            var textAsset = AssetDatabase.LoadAssetAtPath<LocalizationTextAsset>
                    ("Assets/EditorRes/LocalizationTextAsset.asset");
            textAsset.LoadTextAsset();
        }
    }
}

