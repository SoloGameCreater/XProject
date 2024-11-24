using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Wrapper
{
    public class CellContainerWrapper : GameObjectWrapper
    {
        private GameObject temp;
        private List<GameObject> cellList = new List<GameObject>();
        private int currentIdx = -1;

        public CellContainerWrapper(GameObject template) : base(template)
        {
            if (!template)
            {
                Debug.LogError("[CellContainerWrapper] template can not be null");
                return;
            }

            temp = template;
            temp.SetActive(false);
        }

        internal GameObject CellGetter()
        {
            currentIdx++;
            return GetCellAt(currentIdx);
        }

        private GameObject GetCellInList(int index)
        {
            cellList[index].SetActive(true);
            return cellList[index];
        }

        public GameObject GetCellAt(int index)
        {
            if (index < cellList.Count) return GetCellInList(index);
            if (index == cellList.Count) return NewCell();
            Debug.LogError("[CellContainerWrapper.GetSellAt] index error!!!");
            return null;
        }

        protected GameObject NewCell()
        {
            var obj = GameObject.Instantiate(_gameObject, GetParent());
            obj.SetActive(true);
            cellList.Add(obj);
            return obj;
        }

        public void Clear()
        {
            foreach (var cell in cellList)
            {
                cell.SetActive(false);
            }

            currentIdx = -1;
        }

        public void Update(Action<Func<GameObject>> action)
        {
            Clear();
            action(CellGetter);
        }

        public void ForEach(Action<GameObject> action)
        {
            for (int i = 0; i <= currentIdx; i++)
            {
                action(GetCellAt(i));
            }
        }

        public void UpdateWith<T>(IEnumerable<T> collection, Action<T, GameObject> action)
        {
            UpdateWith(collection, (item, go, _) => action?.Invoke(item, go));
        }

        public void UpdateWith<T>(IEnumerable<T> collection, Action<T, GameObject, int> action)
        {
            var tempIdx = -1;
            foreach (var data in collection)
            {
                tempIdx++;
                action?.Invoke(data, GetCellAt(tempIdx), tempIdx);
            }

            currentIdx = tempIdx;

            var maxIdx = cellList.Count - 1;
            while (tempIdx < maxIdx)
            {
                tempIdx++;
                cellList[tempIdx].SetActive(false);
            }
        }
    }

    public abstract class CellContainerWrapperBase<T> : GameObjectWrapper
    {
        protected CellContainerWrapper innerWrapper;

        protected CellContainerWrapperBase(GameObject go) : base(go)
        {
            innerWrapper = new CellContainerWrapper(go);
        }

        protected abstract T ConvertOrCreate(GameObject go);

        private T Getter()
        {
            return ConvertOrCreate(innerWrapper.CellGetter());
        }

        public T GetCellAt(int index)
        {
            return ConvertOrCreate(innerWrapper.GetCellAt(index));
        }

        public virtual void Clear()
        {
            innerWrapper.Clear();
        }

        public void Update(Action<Func<T>> action)
        {
            action(Getter);
        }

        public void ForEach(Action<T> action)
        {
            innerWrapper.ForEach(go => action(ConvertOrCreate(go)));
        }

        public void UpdateWith<U>(IEnumerable<U> collection, Action<U, T> action)
        {
            UpdateWith(collection, (item, comp, _) => action?.Invoke(item, comp));
        }

        public void UpdateWith<U>(IEnumerable<U> collection, Action<U, T, int> action)
        {
            innerWrapper.UpdateWith(collection, (c, go, idx) => action(c, ConvertOrCreate(go), idx));
        }
    }

    public class CellContainerWrapper<T> : CellContainerWrapperBase<T> where T : GameObjectWrapper
    {
        Dictionary<int, T> dictionary = new Dictionary<int, T>();

        public CellContainerWrapper(GameObject go) : base(go)
        {
        }

        protected override T ConvertOrCreate(GameObject go)
        {
            if (!go) return null;

            var key = go.GetInstanceID();
            if (!dictionary.TryGetValue(key, out T ret))
            {
                ret = (T)Activator.CreateInstance(typeof(T), args: new object[] { go });
                dictionary.Add(key, ret);
            }

            return ret;
        }
    }

    public class MonoContainerWrapper<T> : CellContainerWrapperBase<T> where T : MonoBehaviour
    {
        public MonoContainerWrapper(GameObject go) : base(go)
        {
            ConvertOrCreate(go);
            innerWrapper = new CellContainerWrapper(go);
        }

        protected override T ConvertOrCreate(GameObject go)
        {
            if (!go) return null;
            var comp = go.GetComponent<T>();
            if (comp) return comp;
            comp = go.AddComponent<T>();
            return comp;
        }
    }
}