using UnityEngine;
using System;
using System.Collections.Generic;

namespace Extension
{
    public static class GameObjectVisitor
    {
        public delegate bool condition(GameObject obj);

        public static GameObject FindChild(this GameObject _this, string name)
        {
            return FindChildInCondition(_this, delegate(GameObject obj) { return (obj.name == name); });
        }

        public static GameObject FindChildInCondition(this GameObject _this, condition cond)
        {
            if (_this == null)
                return null;

            Transform transform = _this.GetComponent<Transform>();
            foreach (Transform trans in transform)
            {
                if (cond(trans.gameObject))
                {
                    return trans.gameObject;
                }
                else
                {
                    GameObject obj = trans.gameObject.FindChildInCondition(cond);
                    if (obj)
                        return obj;
                }
            }

            return null;
        }

        public static void FindChildren(this GameObject _this, string name, List<GameObject> objs)
        {
            FindChildrenInCondition(_this, objs, delegate(GameObject obj) { return (obj.name == name); });
        }

        public static void FindChildrenInCondition(this GameObject _this, List<GameObject> objs, condition cond)
        {
            if (_this == null)
                return;

            Transform transform = _this.GetComponent<Transform>();
            foreach (Transform trans in transform)
            {
                if (cond(trans.gameObject))
                {
                    objs.Add(trans.gameObject);
                }

                trans.gameObject.FindChildrenInCondition(objs, cond);
            }
        }
    }

    public static class GameObject_MonoBehaviour
    {
        public static T SafeGetComponent<T>(this GameObject _this)
            where T : Component
        {
            if (_this == null)
                return null;

            return _this.GetComponent<T>();
        }

        public static T GetOrCreateComponent<T>(this GameObject _this)
            where T : Component
        {
            if (_this == null)
                return null;

            var ret = _this.GetComponent<T>();
            return (ret == null) ? _this.AddComponent<T>() : ret;
        }
    }
}