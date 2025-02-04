using CommonExtensions;
using TripleMerge;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Editor.TripleMerge
{
    [EditorTool("Map Generator")]
    public class MergeMapGenerator : EditorTool
    {
        private GameObject _mapRoot;
        private Transform _cellRoot;
        private Tilemap _tilemap;

        private GameObject _regionObj;

        private const string CellPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/MergeCell/MergeableCell.prefab";
        private GameObject _mergeCellPrefab;

        public override void OnToolGUI(EditorWindow window)
        {
            _mapRoot = GameObject.Find("AreaRootEditor");
            _cellRoot = _mapRoot?.transform.Find("LogicNode/MergeableRegion");
            _tilemap = _mapRoot?.transform.Find("Terrain/Grass").GetComponent<Tilemap>();
            _mergeCellPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CellPrefabPath);
            Handles.BeginGUI();

            using (new GUILayout.VerticalScope("MAP TOOLS", "window", GUILayout.Height(200), GUILayout.Width(200)))
            {
                if (GUILayout.Button("Merge Map"))
                {
                    if (GenerateMap())
                        Debug.Log("Merge Map");
                    else
                        Debug.LogWarning("Generate map failed");
                }
            }

            Handles.EndGUI();
        }

        private bool GenerateMap()
        {
            if (_mapRoot == null) return false;
            if (_cellRoot == null) return false;
            if (_tilemap == null) return false;
            if (_mergeCellPrefab == null) return false;
            if (_regionObj != null)
            {
                DestroyImmediate(_regionObj);
            }

            _regionObj = new GameObject("Region");
            _regionObj.transform.SetParent(_cellRoot);
            _regionObj.transform.Reset();
            // 合成地块管理
            _regionObj.AddComponent<MergeableRegion>();

            // 获取地图grid信息
            BoundsInt bounds = _tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPosition);

                    if (tile != null)
                    {
                        Vector3 worldPos = _tilemap.GetCellCenterWorld(cellPosition);

                        // 在对应位置创建cell
                        CreateCell(worldPos, $"{x},{y}");
                    }
                }
            }

            return true;
        }

        void CreateCell(Vector3 position, string text)
        {
            GameObject cellObj = Instantiate(_mergeCellPrefab, position, Quaternion.identity, _regionObj.transform);
            cellObj.AddComponent<MergeableCell>();
            cellObj.name = $"Cell_[{text}]";
        }
    }
}