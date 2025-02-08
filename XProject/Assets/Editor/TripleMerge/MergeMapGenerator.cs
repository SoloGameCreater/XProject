using CommonExtensions;
using TripleMerge;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace TripleMerge.Editor
{
    [EditorTool("Map Generator")]
    public class MergeMapGenerator : EditorTool
    {
        private GameObject _mapRoot;
        private Transform _cellRoot;
        private Tilemap _tilemap;

        private GameObject _regionObj;

        private const string EditorRootPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/Area/AreaRootEditor.prefab";
        private const string CellPrefabPath = "Assets/ExtraRes/TripleMerge/Prefabs/MergeCell/MergeableCell.prefab";
        private GameObject _mergeCellPrefab;

        private Sprite _cellA;
        private Sprite _cellB;
        
        private const string CellAPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_03_01.png";
        private const string CellBPath = "Assets/ExtraRes/TMatchRessss/World_Grass/grass_04_01.png";

        public override void OnToolGUI(EditorWindow window)
        {
            _mapRoot = GameObject.Find("AreaRootEditor");
            if (_mapRoot == null)
            {
                _mapRoot = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(EditorRootPrefabPath));
                _mapRoot.name = "AreaRootEditor";
            }

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
        public override bool IsAvailable()
        {
            return SceneManager.GetActiveScene().name.Equals("MergeScene");
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
            _cellA = AssetDatabase.LoadAssetAtPath<Sprite>(CellAPath);
            _cellB = AssetDatabase.LoadAssetAtPath<Sprite>(CellBPath);
            _regionObj = new GameObject("Region");
            _regionObj.transform.SetParent(_cellRoot);
            _regionObj.transform.Reset();
            // 合成地块管理
            _regionObj.AddComponent<MergeableRegion>();

            // 获取地图grid信息
            BoundsInt bounds = _tilemap.cellBounds;
            bool isCellA = true;
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
                        CreateCell(worldPos, x, y,isCellA);
                        isCellA = !isCellA;
                    }
                }
            }

            return true;
        }

        void CreateCell(Vector3 position, int x, int y, bool isCellA)
        {
            var cellObj = Instantiate(_mergeCellPrefab, position,
                Quaternion.identity, _regionObj.transform);
            
            cellObj.name = $"Cell_[{x},{y}]";
            cellObj.GetComponent<SpriteRenderer>().sprite = isCellA ? _cellA : _cellB;
            var cell = cellObj.AddComponent<MergeableCell>();
            cell.CellStatus = MergeableCell.ECellStatus.Locked;
            cell.MapCoordinate = new Vector2Int(x, y);
        }
    }
}