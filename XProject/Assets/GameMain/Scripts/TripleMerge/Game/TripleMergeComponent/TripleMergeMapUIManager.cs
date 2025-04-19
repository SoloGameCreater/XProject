using Framework;
using UnityEngine;
using System.Collections.Generic;
using Config.TripleMerge;

namespace TripleMerge
{
    /// <summary>
    /// 三合玩法地图UI管理器
    /// </summary>
    public class TripleMergeMapUIManager : TripleMergeComponent
    {
        public Canvas UICanvas { private set; get; }
        private readonly List<ITripleMergeMapView> _displayingViewList = new();
        protected override void OnInitialize()
        {
            UICanvas = TripleMergeSystem.Instance.Gameplay.MapRoot.transform.Find("Canvas").GetComponent<Canvas>();
            UICanvas.worldCamera = Camera.main;
            GetOrCreateTransform("RegionUnlockProgressViews");
        }

        protected override void OnDispose()
        {
        }

        /// <summary>
        /// 获取或创建世界UI的父节点
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <returns>创建的Transform</returns>
        public Transform GetOrCreateTransform(string name)
        {
            Transform ret;

            if ((ret = UICanvas.transform.Find(name)) == null)
            {
                ret = new GameObject(name).transform;
                ret.SetParent(UICanvas.transform);
                ret.localPosition = Vector3.zero;
                ret.localScale = Vector3.one;
            }
            return ret;
        }
        protected override void OnUpdate()
        {
            for (var i = _displayingViewList.Count - 1; i >= 0; i--)
            {
                _displayingViewList[i].OnUpdate();
            }
        }
        /// <summary>
        /// 显示世界UI
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="prefabPath">预制体路径</param>
        /// <param name="position">显示位置</param>
        /// <returns>创建的UI实例</returns>
        public T Show<T>(string viewPrefabKey, Vector3 position, params object[] extras) where T : ITripleMergeMapView, new()
        {
            var viewInstance = Utils.InstantiateUI(viewPrefabKey, UICanvas.transform);

            var view = new T
            {
                ViewRoot = viewInstance.transform
            };

            view.ViewRoot.position = position;
            view.OnInitialize();

            view.OnShow(extras);

            _displayingViewList.Add(view);

            return view;
        }

        public void Hide(ITripleMergeMapView view)
        {
            _displayingViewList.Remove(view);

            view.OnHide();

            Object.Destroy(view.ViewRoot.gameObject);
        }
    }

}
