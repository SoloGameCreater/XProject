using System.Collections.Generic;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeGameplay
    {
        public TripleMergeMapManager MapManager { get; } = new();
        public TripleMergeCameraComponent CameraInputManager { get; } = new();
        public TripleMergeTreasureComponent TreasureManager { get; } = new();
        public TripleMergeMapUIManager MapUIManager { get; } = new();

        private readonly List<ITripleMergeComponent> _components = new();
        // 三合功能根节点
        public Transform MapRoot { private set; get; }
        
        // load map root
        const string MapRootPrefabAssetName = "TripleMerge/Prefabs/MapRoot";
        public void Init()
        {
            //TripleMergeSystem.Instance.Model.ClearData();
            _components.Clear();
            _components.Add(MapUIManager);
            _components.Add(MapManager);
            _components.Add(CameraInputManager);
            _components.Add(TreasureManager);

            var mainCamera = Camera.main;
            var cameraTrans = mainCamera.transform;

            MapRoot = Utils.InstantiateWorldGameObject(MapRootPrefabAssetName, cameraTrans.parent).transform;
            
            _components.ForEach(c => c.OnInitialize());
        }

        public void OnUpdate(float deltaTime)
        {
            _components.ForEach(c => c.OnUpdate());
        }

        public void OnLateUpdate(float deltaTime)
        {
            _components.ForEach(c => c.OnLateUpdate());
        }
        public void OnFixedUpdate()
        {
            _components.ForEach(c => c.OnFixedUpdate());
        }
        public void Release()
        {
            _components.ForEach(c => c.OnDispose());
            _components.Clear();
        }
    }
}