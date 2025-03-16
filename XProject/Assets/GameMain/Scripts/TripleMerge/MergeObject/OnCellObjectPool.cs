using UnityEngine;
using System.Collections.Generic;
using Config.TripleMerge;

namespace TripleMerge
{
    public class OnCellObjectPool : MonoBehaviour
    {
        private const int POOL_LIMIT_SINGLE_ELEMENT_TYPE = 250;
        #if UNITY_EDITOR
        private static uint _guid = 0;
        #endif

        private static OnCellObjectPool _instance;

        private readonly Dictionary<string, Stack<OnCellObject>> _poolDictionary = new();

        private Transform _poolRoot;

        public static OnCellObject GetItem(MergeableItem itemCfg, Transform parent = null)
        {
            OnCellObject item = null;

            var type = (TripleMergeItemType)itemCfg.ItemType;
            switch (type)
            {
                case TripleMergeItemType.MergeableNormal:
                    item = Get<MergeableObject>(itemCfg.Prefab, parent);
                    break;
                case TripleMergeItemType.TreasureChest:
                    item = Get<TreasureChest>(itemCfg.Prefab, parent);
                    break;
            }

            return item;
        }

        public static T Get<T>(string prefabKey, Transform parent = null) where T : OnCellObject
        {
            if (_instance == null)
            {
                var root = new GameObject("@OnCellObjectPool")
                {
                    hideFlags = HideFlags.HideAndDontSave
                };

                _instance = root.AddComponent<OnCellObjectPool>();
                _instance._poolRoot = root.transform;

                DontDestroyOnLoad(_instance._poolRoot);
            }


            var storageKey = typeof(T).ToString();
            _instance._poolDictionary.TryAdd(storageKey, new Stack<OnCellObject>());
            if (_instance._poolDictionary[storageKey].TryPop(out var item))
            {
                item.transform.SetParent(parent == null ? _instance._poolRoot : parent);
                item.gameObject.SetActive(true);
                item.Active();
                return item as T;
            }
            
            var itemObject = Utils.InstantiateWorldGameObject($"TripleMerge/Prefabs/MergeableItem/{prefabKey}", parent == null ? _instance._poolRoot : parent);
            item = itemObject.GetOrAddComponent<T>();
            item.name = prefabKey;
            item.Initialize();
            item.Active();

            #if UNITY_EDITOR
            item.GUID = _guid++;
            #endif

            return (T) item;
        }

        public static void Recycle(OnCellObject item)
        {
            if (_instance == null)
            {
                return;
            }

            if (item == null || item.gameObject == null)
            {
                return;
            }

            if (!_instance._poolDictionary.TryGetValue(item.name, out var pool))
            {
                item.Recycle();
                Destroy(item.gameObject);
                return;
            }

            if (pool.Contains(item))
            {
                Debug.Log($"回收了重复的三合合成物{item},已终止本次回收操作!!");
                return;
            }
            
            if (pool.Count >= POOL_LIMIT_SINGLE_ELEMENT_TYPE)
            {
                Destroy(item.gameObject);

                return;
            }

            pool.Push(item);

            item.transform.SetParent(_instance._poolRoot.transform, false);
            item.gameObject.SetActive(false);
            item.Recycle();
        }
    }
}