using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SaveFile.TripleMerge;
using UnityEngine;

namespace SaveFile
{
    public class SaveFileManager : Manager<SaveFileManager>
    {
        const string SaveFileKey = "StorageData";
        
        Dictionary<string, SaveFileBase> storageMap;
        
        static Dictionary<System.Type, string> gType2Name = new ();
        
        private ulong _localVersion;
        public ulong LocalVersion
        {
            get;
            set;
        }
        public bool Inited
        {
            get;
            private set;
        }
        public T GetSaveFile<T>() where T : SaveFileBase
        {
            System.Type storage_type = typeof(T);
            if (!gType2Name.TryGetValue(storage_type, out string name))
            {
                name = storage_type.Name;
                gType2Name[storage_type] = name;
            }

            return (T)storageMap[name];            
        }

        public void Init(List<SaveFileBase> storages)
        {
            if (!Inited)
            {
                storageMap = new Dictionary<string, SaveFileBase>();
                foreach (var storage in storages)
                {
                    var type = storage.GetType().Name;
                    storageMap[type] = storage;
                }
                Inited = true;
                ReadFromLocal();
            }
            else
            {
                Debug.Assert(false, "Error Init Storage !!!");
            }
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(storageMap);
        }
        public void SaveToLocal()
        {
            var tripleMergeSD = GetSaveFile<SaveFileTripleMerge>();
            if (tripleMergeSD != null)
            {
                _localVersion -= 1;
            }
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            // todo 暂时不需要数据加密
            //string jsonData = JsonConvert.SerializeObject(storageMap,setting);
            // byte[] encryptData = RijndaelManager.Instance.EncryptStringToBytes(jsonData);
            // PlayerPrefs.SetString(storageKey, System.Convert.ToBase64String(encryptData));
            // PlayerPrefs.SetString(localVersionKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(LocalVersion.ToString())));
            // PlayerPrefs.SetString(remoteVersionAckKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionACK.ToString())));
            // PlayerPrefs.SetString(remoteVersionLocalKey, System.Convert.ToBase64String(RijndaelManager.Instance.EncryptStringToBytes(RemoteVersionSYN.ToString())));
        }
        private void ReadFromLocal()
        {
            // 读取本地存档
            string jsonData = "{}";
        }
    }
}