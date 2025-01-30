using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro; // 引入 TextMeshPro

public class TileNumberTest : MonoBehaviour
{
    private Tilemap _tilemap;
    public GameObject textPrefab; // 在 Inspector 里设置一个 TextMeshPro 预制体
    private Transform textParent; // 统一管理文本对象
    void Start()
    {
        _tilemap = transform.Find("Grass").GetComponent<Tilemap>();
        
        if (_tilemap == null || textPrefab == null)
        {
            Debug.LogError("请在Inspector中赋值 Tilemap 和 TextMeshPro 预制体");
            return;
        }

        textParent = new GameObject("TileIDContainer").transform; // 创建一个父对象存放所有文本
        GenerateTileNumbers();
    }
    void GenerateTileNumbers()
    {
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
                    CreateText(worldPos, $"{x},{y}");
                }
            }
        }
    }

    private void Update()
    {
        if(_tilemap == null) return;
    }

    
    void CreateText(Vector3 position, string text)
    {
        GameObject textObj = Instantiate(textPrefab, position, Quaternion.identity, textParent);
        TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();

        if (textMesh != null)
        {
            textMesh.text = text;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.fontSize = 3;
            textMesh.color = Color.red;
        }
    }
}
