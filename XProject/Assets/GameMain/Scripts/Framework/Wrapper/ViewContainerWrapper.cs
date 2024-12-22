using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Wrapper
{
    public class ViewContainerWrapper<T> : CellContainerWrapperBase<T> where T : UIView
    {
        private readonly Dictionary<int, T> _dictionary = new();
        private readonly Func<GameObject, T> _viewCreator;

        public ViewContainerWrapper(GameObject go, Func<GameObject, T> viewCreator) : base(go)
        {
            _viewCreator = viewCreator;
        }

        protected override T ConvertOrCreate(GameObject go)
        {
            if (!go) return null;

            var key = go.GetInstanceID();
            if (!_dictionary.TryGetValue(key, out T ret))
            {
                ret = _viewCreator(go);
                _dictionary.Add(key, ret);
            }

            return ret;
        }
    }

}