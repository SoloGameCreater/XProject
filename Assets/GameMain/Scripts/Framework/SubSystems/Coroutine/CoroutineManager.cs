using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class CoroutineManager : GlobalSystem<CoroutineManager>, IInitable
    {
        private Dictionary<string, MBCoroutine> mbs = new Dictionary<string, MBCoroutine>();
        private GameObject root = GameObjectFactory.Create(true);
        public void Init()
        {
            root.name = GetType().ToString();
        }

        public void Release()
        {
            GameObjectFactory.Destroy(root);
            mbs.Clear();
        }

        private MBCoroutine GetMBCoroutine(string group)
        {
            MBCoroutine temp;
            if (mbs.TryGetValue(group, out temp)) return temp;
            GameObject obj = new GameObject();
            obj.transform.parent = root.transform;
            temp = obj.AddComponent<MBCoroutine>();
            mbs.Add(group, temp);
            return temp;
        }

        public Coroutine StartCoroutine(IEnumerator routine, string group = "default")
        {
            MBCoroutine mb = GetMBCoroutine(group);
            return (mb != null && routine != null) ? mb.StartCoroutine(routine) : null;
        }

        public void StopCoroutine(Coroutine routine, string group = "default")
        {
            MBCoroutine mb = GetMBCoroutine(group);
            if (mb != null && routine != null) mb.StopCoroutine(routine);
        }

        public void StopAllCoroutines(string group)
        {
            MBCoroutine mb = GetMBCoroutine(group);
            if (mb != null) mb.StopAllCoroutines();
        }
    }
}