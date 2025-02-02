
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using GameFramework;
using Excel;
using UnityEditor;
using UnityEngine;

namespace Editor.DataTableTools
{
    public sealed class DataTableGeneratorMenu
    {
        private const string DataTablePath = "Assets/ExtraRes/Configs";
        private const string DataTableExcelPath = "Assets/ExtraRes/Configs/Excel";
        
        [MenuItem("Config/Generate DataTables")]
        private static void GenerateDataTables()
        {
            var dataTableName = "MergeableItem";
            //foreach (string dataTableName in ProcedurePreload.DataTableNames)
            {
                ConvertDataTableToText(dataTableName);
                AssetDatabase.Refresh();
                return;
                DataTableProcessor dataTableProcessor = DataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!DataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    //break;
                    return;
                }
            
                DataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                DataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        private static void ConvertDataTableToText(string dataTableName)
        {
            try
            {
                // 获取 Excel 文件夹中的 .xlsx 文件
                string[] files = Directory.GetFiles(DataTableExcelPath, "*.xlsx")
                                          .Select(f => f.Replace('\\', '/'))
                                          .ToArray();

                if (!files.Contains(dataTableName)) return;

                foreach (string file in files)
                {
                    ProcessExcelFile(file);
                }

                Debug.Log("转换完成: " + dataTableName);
            }
            catch (Exception ex)
            {
                Debug.LogError("转换失败: " + ex.Message);
            }
        }

        // 处理单个 Excel 文件
        private static void ProcessExcelFile(string filePath)
        {
            using FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
    
            DataTable table = excelDataReader.AsDataSet().Tables[0];
            SaveTableToTxt(filePath, table);
        }

        // 将 DataTable 保存为 TXT 文件
        private static void SaveTableToTxt(string filePath, DataTable table)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string outputPath = Path.Combine(DataTablePath, fileName + ".txt");

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            using FileStream fs = new FileStream(outputPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(fs);
            StringBuilder sb = new StringBuilder();

            foreach (DataRow row in table.Rows)
            {
                sb.Clear();
                foreach (var item in row.ItemArray)
                {
                    sb.Append(item).Append("\t");
                }
                sw.WriteLine(sb.ToString().TrimEnd()); // 移除末尾的 Tab
            }
        }

    }
}
