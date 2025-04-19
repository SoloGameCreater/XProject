using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class Utils
    {
        public static TripleMergeItemType ParseTripleMergeItemType(int itemType)
        {
            return (TripleMergeItemType)itemType;
        }

        public static GameObject InstantiateWorldGameObject(string prefabName, Transform parent)
        {
            var obj = ResourcesManager.Instance.LoadResource<GameObject>(prefabName);
            return Object.Instantiate(obj, parent);
        }
        public static GameObject InstantiateUI(string prefabName, Transform parent)
        {
            var obj = ResourcesManager.Instance.LoadResource<GameObject>($"Prefabs/UI/TripleMerge/{prefabName}");
            return Object.Instantiate(obj, parent);
        }
    }
}