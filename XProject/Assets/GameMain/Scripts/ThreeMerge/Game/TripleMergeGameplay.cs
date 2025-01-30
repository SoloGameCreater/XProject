using System.Collections.Generic;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeGameplay
    {
        public TripleMergeMapManager MapManager { get; } = new();
        public TripleMergeCameraManager CameraManager { get; } = new();

        private List<ITripleMergeComponent> _components = new();
        
        public Transform MapRoot { private set; get; }
        
        public void Init()
        {
            TripleMergeSystem.Instance.Model.ClearData();
            _components.Clear();
            _components.Add(MapManager);
            _components.Add(CameraManager);
            
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