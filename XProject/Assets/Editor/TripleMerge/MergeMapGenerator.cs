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
        private Transform _terrainGrid;
        private Tilemap _tilemap;

        private GameObject _regionObj;

        private const string CellPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/MergeCell/MergeableCell.prefab";
        private GameObject _mergeCellPrefab;
        
        public override void OnToolGUI(EditorWindow window)
        {
            _mapRoot = GameObject.Find("MergeMaoRoot");
            _cellRoot = _mapRoot?.transform.Find("ItemNode");
            _terrainGrid = _mapRoot?.transform.Find("Terrain");
            _tilemap = _terrainGrid?.Find("Grass").GetComponent<Tilemap>();
            _mergeCellPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(CellPrefabPath);
            Handles.BeginGUI();

            using (new GUILayout.VerticalScope("MAP TOOLS", "window", GUILayout.Height(200), GUILayout.Width(200)))
            {
                if (GUILayout.Button("Merge Map"))
                {
                    if(GenerateMap())
                        Debug.Log("Merge Map");
                    else
                        Debug.LogWarning("Generate map failed");
                }
            }
            
            Handles.EndGUI();
        }

        private bool GenerateMap()
        {
            if(_mapRoot == null) return false;
            if(_terrainGrid == null) return false;
            if(_cellRoot == null) return false;
            if (_tilemap == null) return false;
            if (_mergeCellPrefab == null) return false;
            if (_regionObj != null)
            {
                DestroyImmediate(_regionObj);
            }
            _regionObj = new GameObject("Region");
            _regionObj.transform.SetParent(_cellRoot);
            _regionObj.transform.Reset();
            
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
                        CreateText(worldPos, $"{x},{y}");
                    }
                }
            }

            return true;
        }
        void CreateText(Vector3 position, string text)
        {
            GameObject cellObj = Instantiate(_mergeCellPrefab, position, Quaternion.identity, _regionObj.transform);
            cellObj.name = $"Cell_[{text}]";
        }
    }
}