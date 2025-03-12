using System.Collections.Generic;
using System.IO;
using TripleMerge;
using UnityEditor;
using UnityEngine;

namespace TripleMerge.Editor
{
    /// <summary>
    /// 三合地图数据导出工具
    /// </summary>
    public class MergeMapExporter : EditorWindow
    {
        private string _exportPath = "Assets/Resources/TripleMapData";
        private string _fileName = "MapData.json";
        private GameObject _mapRoot;
        private Transform _cellRoot;

        [MenuItem("Tools/三合一/导出地图数据")]
        public static void ShowWindow()
        {
            GetWindow<MergeMapExporter>("三合地图数据导出工具");
        }

        private void OnGUI()
        {
            GUILayout.Label("三合地图数据导出工具", EditorStyles.boldLabel);
            
            EditorGUILayout.Space();
            
            _mapRoot = EditorGUILayout.ObjectField("地图根节点", _mapRoot, typeof(GameObject), true) as GameObject;
            
            if (_mapRoot != null)
            {
                _cellRoot = _mapRoot.transform.Find("LogicNode/MergeableRegion/Region");
                if (_cellRoot == null)
                {
                    EditorGUILayout.HelpBox("未找到地图地块根节点 (LogicNode/MergeableRegion/Region)", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.LabelField($"找到地块数量: {_cellRoot.childCount}");
                }
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("导出设置", EditorStyles.boldLabel);
            _exportPath = EditorGUILayout.TextField("导出路径", _exportPath);
            _fileName = EditorGUILayout.TextField("文件名", _fileName);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("导出地图数据"))
            {
                ExportMapData();
            }
        }

        private void ExportMapData()
        {
            if (_mapRoot == null)
            {
                EditorUtility.DisplayDialog("错误", "请先选择地图根节点", "确定");
                return;
            }

            if (_cellRoot == null || _cellRoot.childCount == 0)
            {
                EditorUtility.DisplayDialog("错误", "未找到有效的地块数据", "确定");
                return;
            }

            // 创建导出目录
            if (!Directory.Exists(_exportPath))
            {
                Directory.CreateDirectory(_exportPath);
            }

            // 收集地块数据
            var mapData = new MapData();
            mapData.cells = new Dictionary<string, CellData>();

            foreach (Transform cellTransform in _cellRoot)
            {
                var cell = cellTransform.GetComponent<MergeableCell>();
                if (cell != null)
                {
                    // 使用坐标作为键
                    string key = $"{cell.MapCoordinate.x}_{cell.MapCoordinate.y}";
                    
                    // 创建单元格数据
                    var cellData = new CellData
                    {
                        belongRegionId = cell.BelongRegionId,
                        cellStatus = (int)cell.CellStatus,
                        purifiedPriority = cell.PurifiedPriority,
                        requiredPurifiedNum = cell.RequiredPurifiedNum,
                        initialPlacedItemId = cell.InitialPlacedItemId,
                        mapCoordinate = new int[] { cell.MapCoordinate.x, cell.MapCoordinate.y }
                    };
                    
                    // 添加到字典
                    mapData.cells.Add(key, cellData);
                }
            }

            // 序列化为JSON
            string json = JsonUtility.ToJson(mapData, true);
            
            // 保存到文件
            string fullPath = Path.Combine(_exportPath, _fileName);
            File.WriteAllText(fullPath, json);
            
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("成功", $"地图数据已导出到: {fullPath}", "确定");
            Debug.Log($"地图数据已导出到: {fullPath}");
        }
    }
} 