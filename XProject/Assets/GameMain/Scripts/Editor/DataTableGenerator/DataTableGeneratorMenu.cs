using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using GameFramework;
using UnityEditor;
using UnityEngine;
using ExcelDataReader;

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
            ConvertExcelToJson(DataTableExcelPath + "/" + dataTableName + ".xlsx", DataTablePath + "/" + dataTableName + ".json");
            return;
            //foreach (string dataTableName in ProcedurePreload.DataTableNames)
            {
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

        private static void ConvertExcelToJson(string excelFilePath, string jsonOutputPath)
        {
            // 打开Excel文件
            FileStream stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateReader(stream);
            try
            {
                // 读取Excel文件
                DataSet result = excelReader.AsDataSet();

                // 获取第一个工作表
                DataTable table = result.Tables[0];

                // 创建一个用于存储行的列表
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

                // 获取列名
                string[] columnNames = new string[table.Columns.Count];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    columnNames[i] = table.Columns[i].ColumnName;
                }

                // 遍历行并添加到列表
                // 0 名称
                // 1 类型
                // 2 文字描述
                // >=3 值
                var nameRow = table.Rows[0];
                var typeRow = table.Rows[1];
                var titleRow = table.Rows[2];

                int index = 0;
                foreach (DataRow row in table.Rows)
                {
                    // 前三行跳过
                    if (index < 3)
                    {
                        index++;
                        continue;
                    }

                    Dictionary<string, object> rowDict = new Dictionary<string, object>();
                    for (int i = 0; i < nameRow.ItemArray.Length; i++)
                    {
                        string typeName = typeRow[i].ToString();
                        if (typeName == "note") continue;
                        if (DBNull.Value.Equals(row[i])) continue;
                        
                        if(typeName == "string")
                            rowDict[nameRow[i].ToString()] = row[i].ToString();
                        else if(typeName == "int")
                            rowDict[nameRow[i].ToString()] = Convert.ToInt32(row[i]);
                        else if(typeName == "float")
                            rowDict[nameRow[i].ToString()] = Convert.ToSingle(row[i]);
                        else if(typeName == "bool")
                            rowDict[nameRow[i].ToString()] = row[i].ToString() == "1";
                        else if (typeName == "number")
                        {
                            if (row[i].ToString().Contains('.'))
                                rowDict[nameRow[i].ToString()] = Convert.ToSingle(row[i]);
                            else
                                rowDict[nameRow[i].ToString()] = Convert.ToInt32(row[i]);
                        }
                        else
                        {
                            rowDict[nameRow[i].ToString()] = row[i];
                            Debug.LogWarning($"没有处理的类型 {typeName}");
                        }
                        //rowDict[columnNames[i]] = row[i];
                    }

                    rows.Add(rowDict);
                }

                // 将列表转换为JSON格式
                string json = JsonConvert.SerializeObject(rows, Formatting.Indented);

                // 输出到文件
                File.WriteAllText(jsonOutputPath, json);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                // 关闭读取器
                excelReader.Close();
            }

            Debug.Log("Excel转换为JSON成功！");
        }
    }
}