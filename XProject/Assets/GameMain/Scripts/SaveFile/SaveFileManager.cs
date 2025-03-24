using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rijndael;
using SaveFile.TripleMerge;
using UnityEngine;

namespace SaveFile
{
    public class SaveFileManager : Manager<SaveFileManager>
    {
        const string SaveFileKey = "SaveFile";
        const string LocalVersionKey = "SSaveFileVersion";

        Dictionary<string, SaveFileBase> storageMap;

        static Dictionary<System.Type, string> gType2Name = new();

        private const float localInterval = 1.0f;
        ulong lastSavedLocalVersion;
        private ulong _localVersion;
        public ulong LocalVersion
        {
            get
            {
                return _localVersion;
            }
            set
            {
                if (_localVersion != value)
                {
                    _localVersion = value;
                    //DebugUtil.LogWarning("LocalVersion changed: " + _localVersion);
                }
            }
        }
        public bool Inited { get; private set; }
        float LocalTickTime { get; set; }
        public bool SyncForce { get; set; }
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
                Debug.Assert(false, "init save file error!!!");
            }
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(storageMap);
        }
        public void SaveToLocal()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            string jsonData = JsonConvert.SerializeObject(storageMap, setting);
            PlayerPrefs.SetString(SaveFileKey,
             System.Convert.ToBase64String(RijndaelEncryptionManager.Instance.Encrypt(jsonData)));
            PlayerPrefs.SetString(LocalVersionKey,
             System.Convert.ToBase64String(RijndaelEncryptionManager.Instance.Encrypt(LocalVersion.ToString())));
            lastSavedLocalVersion = LocalVersion;
        }
        private void ReadFromLocal()
        {
            // 读取存档
            if (PlayerPrefs.HasKey(SaveFileKey))
            {
                var saveFileKey = PlayerPrefs.GetString(SaveFileKey);
                byte[] encryptData = System.Convert.FromBase64String(saveFileKey);
                var jsonData = RijndaelEncryptionManager.Instance.Decrypt(encryptData);
                FromJson(jsonData);
            }
            else
            {
#if UNITY_EDITOR
                DebugUtil.LogWarning("No local storage data can read! ");
#endif
            }

            // 读取本地存档版本
            if (PlayerPrefs.HasKey(LocalVersionKey))
            {
                var versionKey = PlayerPrefs.GetString(LocalVersionKey);
                string strVersion = RijndaelEncryptionManager.Instance.Decrypt(System.Convert.FromBase64String(versionKey));
                LocalVersion = ulong.Parse(strVersion);
            }
            else
            {
                LocalVersion = 0;
#if UNITY_EDITOR
                DebugUtil.LogWarning("No local storage version can read! ");
#endif
            }
            _localVersion = LocalVersion;
        }

        private void FromJson(string jsonData)
        {
            var jObj = JObject.Parse(jsonData);
            foreach (var type in storageMap.Keys)
            {
                var token = jObj[type];
                if (token == null)
                {
                    continue;
                }

                var str = token.ToString();
                JsonSerializerSettings setting = new JsonSerializerSettings();

                setting.NullValueHandling = NullValueHandling.Ignore;
                JsonConvert.PopulateObject(str, storageMap[type], setting);
            }
        }
        private void Update()
        {
            if (!Inited)
                return;

            LocalTickTime += Time.deltaTime;
            if (SyncForce || (LocalTickTime > localInterval && LocalVersion > lastSavedLocalVersion))
            {
                SyncForce = false;
                LocalTickTime = 0.0f;
                SaveToLocal();
            }
        }
    }
}