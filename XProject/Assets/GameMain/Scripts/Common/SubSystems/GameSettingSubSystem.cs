using Framework;
using UnityEngine;

namespace Gameplay.SubSystems
{
    public class GameSettingSubSystem : GlobalSystem<GameSettingSubSystem>, IStart
    {
        private LightmapData[] _currentLightmapDatas;

        public void Start()
        {
            Input.multiTouchEnabled = true;
        }


        public void EnableLightMap(bool enable)
        {
            if (!enable)
            {
                if (_currentLightmapDatas == null)
                {
                    _currentLightmapDatas = LightmapSettings.lightmaps;
                    LightmapSettings.lightmaps = null;
                }

            }
            else
            {
                if (LightmapSettings.lightmaps == null)
                {
                    LightmapSettings.lightmaps = _currentLightmapDatas;
                }
            }
        }
    }
}