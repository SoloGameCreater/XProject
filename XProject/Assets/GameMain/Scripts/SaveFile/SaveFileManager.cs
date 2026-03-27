using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SaveFile
{
    public class SaveFileManager : Manager<SaveFileManager>
    {
        const string SaveFileKey = "SaveFile";
        const string LocalVersionKey = "SSaveFileVersion";
        const string ManualSaveFileKey = "SaveFile_ManualBackup";
        const string ManualVersionKey = "SSaveFileVersion_ManualBackup";
        const string ManualSavedAtKey = "SaveFile_ManualSavedAtUtcTicks";

        Dictionary<string, SaveFileBase> storageMap;

        static Dictionary<System.Type, string> gType2Name = new();

        private ulong _localVersion;
        private bool _hasUnsavedChanges;
        private bool _isReadingLocalData;
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
                    if (!_isReadingLocalData)
                    {
                        _hasUnsavedChanges = true;
                    }
                }
            }
        }
        public bool HasUnsavedChanges => _hasUnsavedChanges;
        public bool Inited { get; private set; }
        public ulong LastManualSaveUtcTicks { get; private set; }
        public bool SyncForce { get; set; }

        public T GetSaveFile<T>() where T : SaveFileBase
        {
            System.Type storage_type = typeof(T);
            if (!gType2Name.TryGetValue(storage_type, out string name))
            {
                name = storage_type.Name;
                gType2Name[storage_type] = name;
            }

            if (storageMap == null || !storageMap.TryGetValue(name, out var saveFile))
            {
                DebugUtil.LogError($"未注册存档类型: {name}");
                return null;
            }

            return (T)saveFile;
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

        public void MarkDirty()
        {
            LocalVersion++;
        }

        public bool SaveToLocal()
        {
            return SaveCurrentToSlot(SaveFileKey, LocalVersionKey, forceSave: true, saveReason: "兼容接口SaveToLocal");
        }

        public bool TryAutoSave(string triggerName, bool forceSave = false)
        {
            if (!Inited)
            {
                return false;
            }

            var reason = string.IsNullOrEmpty(triggerName) ? "触发点自动存档" : $"触发点自动存档: {triggerName}";
            return SaveCurrentToSlot(SaveFileKey, LocalVersionKey, forceSave, reason);
        }

        public bool ManualSave(string saveReason = null)
        {
            if (!Inited)
            {
                return false;
            }

            var reason = string.IsNullOrEmpty(saveReason) ? "手动存档" : $"手动存档: {saveReason}";
            if (!TryBuildEncryptedPayload(out var encryptedData, out var encryptedVersion))
            {
                return false;
            }

            if (!WritePayloadToSlot(SaveFileKey, LocalVersionKey, encryptedData, encryptedVersion, reason))
            {
                return false;
            }

            if (!WritePayloadToSlot(ManualSaveFileKey, ManualVersionKey, encryptedData, encryptedVersion, $"{reason}(备份)"))
            {
                return false;
            }

            LastManualSaveUtcTicks = (ulong)DateTime.UtcNow.Ticks;
            PlayerPrefs.SetString(ManualSavedAtKey, LastManualSaveUtcTicks.ToString());
            PlayerPrefs.Save();

            _hasUnsavedChanges = false;
            return true;
        }

        private bool SaveCurrentToSlot(string dataKey, string versionKey, bool forceSave, string saveReason)
        {
            if (!forceSave && !_hasUnsavedChanges)
            {
                return false;
            }

            if (!TryBuildEncryptedPayload(out var encryptedData, out var encryptedVersion))
            {
                return false;
            }

            if (!WritePayloadToSlot(dataKey, versionKey, encryptedData, encryptedVersion, saveReason))
            {
                return false;
            }

            PlayerPrefs.Save();
            _hasUnsavedChanges = false;
            return true;
        }

        private bool TryBuildEncryptedPayload(out string encryptedData, out string encryptedVersion)
        {
            encryptedData = string.Empty;
            encryptedVersion = string.Empty;

            try
            {
                JsonSerializerSettings setting = new JsonSerializerSettings();
                setting.NullValueHandling = NullValueHandling.Ignore;
                encryptedData = JsonConvert.SerializeObject(storageMap, setting);
                encryptedVersion = LocalVersion.ToString();
                return true;
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"Build save data failed: {e.Message}");
                return false;
            }
        }

        private bool WritePayloadToSlot(string dataKey, string versionKey, string encryptedData, string encryptedVersion, string saveReason)
        {
            try
            {
                PlayerPrefs.SetString(dataKey, encryptedData);
                PlayerPrefs.SetString(versionKey, encryptedVersion);
                return true;
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"{saveReason}写盘失败: {e.Message}");
                return false;
            }
        }

        private void ReadFromLocal()
        {
            bool loaded = false;
            bool mainSlotCorrupted = false;
            bool loadedFromManualSlot = false;
            _isReadingLocalData = true;

            if (TryReadSlot(SaveFileKey, LocalVersionKey, out var jsonData, out var localVersion))
            {
                loaded = TryPopulateFromJson(jsonData, "主存档");
                if (loaded)
                {
                    _localVersion = localVersion;
                }
                else
                {
                    mainSlotCorrupted = true;
                    ClearCorruptedSlot(SaveFileKey, LocalVersionKey, "主存档");
                }
            }

            if (!loaded && TryReadSlot(ManualSaveFileKey, ManualVersionKey, out jsonData, out localVersion))
            {
                loaded = TryPopulateFromJson(jsonData, "手动备份存档");
                if (loaded)
                {
                    _localVersion = localVersion;
                    loadedFromManualSlot = true;
                }
                else
                {
                    ClearCorruptedSlot(ManualSaveFileKey, ManualVersionKey, "手动备份存档");
                }
            }

            // 主存档损坏但手动备份可用时，自动回填主存档，避免每次启动重复报错。
            if (mainSlotCorrupted && loadedFromManualSlot)
            {
                SaveCurrentToSlot(SaveFileKey, LocalVersionKey, forceSave: true, saveReason: "主存档损坏后自动修复");
            }

            if (!loaded)
            {
                _localVersion = 0;
#if UNITY_EDITOR
                DebugUtil.LogWarning("No local storage version can read! ");
#endif
            }

            _isReadingLocalData = false;
            _hasUnsavedChanges = false;

            if (PlayerPrefs.HasKey(ManualSavedAtKey)
             && ulong.TryParse(PlayerPrefs.GetString(ManualSavedAtKey), out var manualSavedAtTicks))
            {
                LastManualSaveUtcTicks = manualSavedAtTicks;
            }
        }

        private bool TryReadSlot(string dataKey, string versionKey, out string jsonData, out ulong localVersion)
        {
            jsonData = string.Empty;
            localVersion = 0;

            if (!PlayerPrefs.HasKey(dataKey))
            {
                return false;
            }

            try
            {
                var saveFileKey = PlayerPrefs.GetString(dataKey);
                jsonData = TryDecodeLegacyBase64(saveFileKey, out var decodedJson) ? decodedJson : saveFileKey;

                if (PlayerPrefs.HasKey(versionKey))
                {
                    var rawVersion = PlayerPrefs.GetString(versionKey);
                    if (!TryReadVersion(rawVersion, out localVersion))
                    {
                        localVersion = 0;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"Read save failed dataKey={dataKey}: {e.Message}");
                return false;
            }
        }

        private bool TryReadVersion(string rawVersion, out ulong localVersion)
        {
            localVersion = 0;
            if (string.IsNullOrEmpty(rawVersion))
            {
                return false;
            }

            if (ulong.TryParse(rawVersion, out localVersion))
            {
                return true;
            }

            try
            {
                if (TryDecodeLegacyBase64(rawVersion, out var decodedVersion)
                    && ulong.TryParse(decodedVersion, out localVersion))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                // Fall back to plain text parsing below.
            }

            return false;
        }

        private void ClearCorruptedSlot(string dataKey, string versionKey, string sourceName)
        {
            try
            {
                PlayerPrefs.DeleteKey(dataKey);
                PlayerPrefs.DeleteKey(versionKey);
                PlayerPrefs.Save();
                DebugUtil.LogWarning($"{sourceName}已损坏，已清理本地坏档键位");
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"清理{sourceName}坏档失败: {e.Message}");
            }
        }

        private bool TryPopulateFromJson(string jsonData, string sourceName)
        {
            try
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

                return true;
            }
            catch (Exception e)
            {
                DebugUtil.LogError($"{sourceName}反序列化失败: {e.Message}。该存档可能已损坏或来自不兼容的历史加密格式。");
                return false;
            }
        }

        private static bool TryDecodeLegacyBase64(string rawText, out string decodedText)
        {
            decodedText = string.Empty;
            if (string.IsNullOrEmpty(rawText))
            {
                return false;
            }

            try
            {
                decodedText = Encoding.UTF8.GetString(Convert.FromBase64String(rawText));
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void Update()
        {
            if (!Inited || !SyncForce)
            {
                return;
            }

            SyncForce = false;
            TryAutoSave("兼容接口SyncForce", forceSave: true);
        }

        private void OnApplicationQuit()
        {
            TryAutoSave("应用退出", forceSave: true);
        }
    }
}
