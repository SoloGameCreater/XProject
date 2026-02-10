using System.Collections.Generic;
using Framework;
using GameplayRuntime;
using SaveFile;

namespace Gameplay.SubSystems
{
    public class SaveFileSystem : GlobalSystem<SaveFileSystem>,IInitable
    {
        public void Init()
        {
            List<SaveFileBase> saveFileBases = new List<SaveFileBase>();
            saveFileBases.Add(new SaveFileCurrency());

            GameplayCatalog.Instance.CollectSaveFiles(saveFileBases);

            // 按类型去重，避免重复注册导致 SaveFileManager 初始化冲突
            HashSet<System.Type> addedTypes = new HashSet<System.Type>();
            List<SaveFileBase> distinctSaveFiles = new List<SaveFileBase>();
            for (int i = 0; i < saveFileBases.Count; i++)
            {
                var saveType = saveFileBases[i].GetType();
                if (addedTypes.Add(saveType))
                {
                    distinctSaveFiles.Add(saveFileBases[i]);
                }
            }
            SaveFileManager.Instance.Init(distinctSaveFiles);
        }

        public void Release()
        {
            
        }
    }
}
