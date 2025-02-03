using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeSystem : GlobalSystem<TripleMergeSystem>, IUpdatable, ILateUpdatable
    {
        public TripleMergeGameplay Gameplay { get; private set; }
        public TripleMergeModel Model { get; } = new();
        
        public GameObject Root;
        public GameObject ItemRoot;
        
        public void OnEnterTripleMerge()
        {
            Root = new GameObject("TripleMergeRoot");
            ItemRoot = new GameObject("Item");
            ItemRoot.transform.parent = Root.transform;
            
            Gameplay = new TripleMergeGameplay();
            Gameplay.Init();

            // 生成一个宝箱
            var cfg = Model.GetMergeableItem(99999);
            if (cfg != null)
            {
                
            }
        }

        public void Update(float deltaTime)
        {
            Gameplay?.OnUpdate(deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            Gameplay?.OnLateUpdate(deltaTime);  
        }

        public void Exit()
        {
            Gameplay?.Release();
            Gameplay = null;
        }
    }
}