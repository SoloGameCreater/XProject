using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;

[System.Serializable]
public class VersionInfo
{
    public string Version;

    public Dictionary<string, VersionItemInfo> ResGroups;

    public string UniqueID;

    public string UnityVersion;

    public VersionInfo(string buildOutPath, string bigVersion, string smallVersion)
    {
        Version = bigVersion;
        ResGroups = new Dictionary<string, VersionItemInfo>();
        UniqueID = System.Guid.NewGuid().ToString().Replace("-", "");
        UnityVersion = Application.unityVersion;

        // System
        {
            VersionItemInfo commonVersionItemInfo = new VersionItemInfo();
            commonVersionItemInfo.Version = "1.0.0";
            commonVersionItemInfo.UpdateWholeGroup = true;
            ResGroups.Add("System", commonVersionItemInfo);

            List<string> files = new List<string>();
            files.Add("OutputCache");
            files.Add("OutputCache.manifest");
            files.Add($"BuildReport_{YooAssetSettingsData.Setting.DefaultYooPackageName}_{smallVersion}.json");
            files.Add($"{YooAssetSettingsData.Setting.ManifestFileName}_{YooAssetSettingsData.Setting.DefaultYooPackageName}_{smallVersion}.bytes");
            files.Add($"{YooAssetSettingsData.Setting.ManifestFileName}_{YooAssetSettingsData.Setting.DefaultYooPackageName}_{smallVersion}.hash");
            files.Add($"{YooAssetSettingsData.Setting.ManifestFileName}_{YooAssetSettingsData.Setting.DefaultYooPackageName}_{smallVersion}.json");
            files.Add($"{YooAssetSettingsData.Setting.ManifestFileName}_{YooAssetSettingsData.Setting.DefaultYooPackageName}.version");

            foreach (var file in files)
            {
                GameAssetBundleInfo bundleInfo = new GameAssetBundleInfo();
                bundleInfo.AssetBundleName = file;
                bundleInfo.HashString = GetMD5FromFile($"{buildOutPath}/{bigVersion}/{bundleInfo.AssetBundleName}");
                bundleInfo.Md5 = bundleInfo.HashString;
                bundleInfo.State = GameAssetState.ExistInDownLoad;
                commonVersionItemInfo.AssetBundles.Add(bundleInfo.AssetBundleName, bundleInfo);
            }
        }

        // Bundle
        {
            VersionItemInfo defaultVersionItemInfo = new VersionItemInfo();
            defaultVersionItemInfo.Version = "1.0.0";
            defaultVersionItemInfo.UpdateWholeGroup = true;
            ResGroups.Add("Bundle", defaultVersionItemInfo);

            string jsonData = File.ReadAllText($"{buildOutPath}/{bigVersion}/BuildReport_{YooAssetSettingsData.Setting.DefaultYooPackageName}_{smallVersion}.json", Encoding.UTF8);
            var buildReport = BuildReport.Deserialize(jsonData);
            foreach (var p in buildReport.BundleInfos)
            {
                GameAssetBundleInfo bundleInfo = new GameAssetBundleInfo();
                bundleInfo.AssetBundleName = p.FileName;
                bundleInfo.HashString = p.FileHash;
                bundleInfo.Md5 = p.FileHash;
                bundleInfo.State = GameAssetState.ExistInDownLoad;
                defaultVersionItemInfo.AssetBundles.Add(p.FileName, bundleInfo);
            }
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    private string GetMD5FromFile(string filePath)
    {
        string filemd5;
        using (var fileStream = File.OpenRead(filePath))
        {
            var md5 = MD5.Create();
            var fileMD5Bytes = md5.ComputeHash(fileStream);
            filemd5 = BitConverter.ToString(fileMD5Bytes).Replace("-", "").ToLower();
        }

        return filemd5;
    }
}