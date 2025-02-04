using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TripleMerge
{
    public class MergeableRegion : MonoBehaviour
    {
        public List<MergeableCell> Initialize()
        {
            var ret = ListPool<MergeableCell>.Get();

            var cellsArray = transform.GetComponentsInChildren<MergeableCell>();

            for (var i = cellsArray.Length - 1; i >= 0; i--)
            {
                var cell = cellsArray[i];

                var cellRenderer = cell.GetComponent<SpriteRenderer>();
                if (cellRenderer == null || cellRenderer.sprite == null)
                {
                    Destroy(cellRenderer.gameObject);
                    continue;
                }

                cell.Initialize(this);
                ret.Add(cell);
            }

            return ret;
        }
    }
}