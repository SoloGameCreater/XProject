using Config.TripleMerge;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeTreasureComponent : TripleMergeComponent
    {
        protected override void OnInitialize()
        {
            
        }

        protected override void OnDispose()
        {
            
        }

        public void GenerateTreasure(Vector3 generatePosition)
        {
            Debug.LogWarning($"Generate Treasure in position {generatePosition}");
            //todo 没有宝箱了,弹出对应弹窗提示玩家游玩Match玩法来获取宝箱
            
            //todo 获取可见范围内的空CELL
            var mapManager = TripleMergeSystem.Instance.Gameplay.MapManager;
            
            //todo 消耗一个宝箱
            
            //todo 生成宝箱
            var chestCfgData = TripleMergeConfigManager.Instance.GetItemConfig(TreasureChest.TreasureChestID);
        }
    }
}