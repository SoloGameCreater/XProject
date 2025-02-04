using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class Utils
    {
        public static TripleMergeItemType ParseThreeMergeItemType(int itemType)
        {
            return (TripleMergeItemType)itemType;
        }

        public static GameObject InstantiateWorldGameObject(string prefabName, Transform parent)
        {
            var obj = ResourcesManager.Instance.LoadResource<GameObject>(prefabName);
            return Object.Instantiate(obj, parent);
        }
    }
}