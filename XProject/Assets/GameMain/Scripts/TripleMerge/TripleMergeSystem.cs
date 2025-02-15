using Framework;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeSystem : GlobalSystem<TripleMergeSystem>, IUpdatable, ILateUpdatable
    {
        public TripleMergeGameplay Gameplay { get; private set; }
        public TripleMergeModel Model { get; } = new();
        
        public void OnEnterTripleMerge()
        {
            Gameplay = new TripleMergeGameplay();
            Gameplay.Init();

            // 生成一个宝箱
            var cfg = Model.GetMergeableItem(99999);
            if (cfg != null)
            {
                //todo 测试阶段，进入游戏自动设置为10个宝箱
                CurrencyModel.Instance.SetCurrency(CurrencyType.TreasureChest, 10);
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