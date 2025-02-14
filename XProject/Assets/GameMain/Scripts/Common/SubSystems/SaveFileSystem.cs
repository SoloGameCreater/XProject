using System.Collections.Generic;
using Framework;
using SaveFile;
using SaveFile.TripleMerge;

namespace Gameplay.SubSystems
{
    public class SaveFileSystem : GlobalSystem<SaveFileSystem>,IInitable
    {
        public void Init()
        {
            List<SaveFileBase> saveFileBases = new List<SaveFileBase>();
            saveFileBases.Add(new SaveFileCurrency());
            saveFileBases.Add(new SaveFileTripleMerge());
            SaveFileManager.Instance.Init(saveFileBases);
        }

        public void Release()
        {
            
        }
    }
}