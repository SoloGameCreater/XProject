using Config.TripleMerge;
using UnityEngine;

namespace TripleMerge
{
    public class TripleMergeTreasureComponent : TripleMergeComponent
    {
        protected override void OnInitialize()
        {
            TripleMergeSystem.Instance.Model.LoadUnlockMergeableItems();
        }

        protected override void OnDispose()
        {
            
        }

        public void GenerateTreasure(Vector3 generatePosition)
        {
            Debug.LogWarning($"Generate Treasure in position {generatePosition}");
            //todo 没有宝箱了,弹出对应弹窗提示玩家游玩Match玩法来获取宝箱
            if (!CurrencyModel.Instance.IsCurrencyEnough(CurrencyType.TreasureChest, 1))
            {
                Debug.LogWarning("Not Enough Treasure");
                return;
            }
            // 获取可见范围内的空CELL
            var mapManager = TripleMergeSystem.Instance.Gameplay.MapManager;
            if (!mapManager.TryGetNearestEmptyCell(out var targetCell))
            {
                Debug.LogWarning("No Empty Cell");
                return;
            }
            // 消耗一个宝箱
            CurrencyModel.Instance.CostCurrency(CurrencyType.TreasureChest);
            
            //todo 生成宝箱
            var chestCfgData = TripleMergeConfigManager.Instance.GetItemConfig(TreasureChest.TreasureChestID);
            var chestInstance = OnCellObjectPool.GetItem(chestCfgData) as TreasureChest;
            if(chestInstance == null)
            {
                Debug.LogError("Treasure Chest Not Found");
                return;
            }
            chestInstance.SetCfgData(chestCfgData);
            
            // 转换正确的生成位置,先将视口坐标的z轴置0
            generatePosition.z = 0;
            // 将视口坐标转换为三合玩法相机的世界坐标
            var generateWorldPosition = mapManager.MapCamera.ViewportToWorldPoint(generatePosition);
            generateWorldPosition.z = 0;
            // 将宝箱设置到对应的出生位置
            chestInstance.transform.position = generateWorldPosition;
            // 因为宝箱会从出生位置飞行到对应的地块,所以飞行过程中提高其sortingOrder
            chestInstance.OnFlyBegin();
            // 将宝箱放置到目标地块
            targetCell.PlaceItem(chestInstance,
                () =>
                {
                    chestInstance.OnFlyEnd();
                    Debug.Log("Treasure on placed");
                },
                () =>
                {
                    //EventDispatcher.Instance.DispatchEventImmediately(EventEnum.ThreeMergeOnMapContentChanged);
                    Debug.Log("Treasure on merged");
                });
        }
    }
}