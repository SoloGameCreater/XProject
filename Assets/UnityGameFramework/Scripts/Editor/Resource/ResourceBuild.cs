using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;

public static class ResourceBuild
{
    [MenuItem("YooAsset/BuildRes", false)]
    public static void Build()
    {
        BuildPackage(YooAssetSettingsData.Setting.DefaultYooPackageName, SettingsUtils.DownloadVersion, System.Guid.NewGuid().ToString().Replace("-", ""));
    }

    [MenuItem("YooAsset/ClearHostPlayModeCache", false)]
    public static void ClearHostPlayModeCache()
    {
        // 注意：为了方便调试查看，编辑器下把存储目录放到项目里。
        string projectPath = Path.GetDirectoryName(UnityEngine.Application.dataPath);
        string cachePath = Path.Combine(projectPath, YooAssetSettingsData.Setting.DefaultYooFolderName);
        if (Directory.Exists(cachePath)) Directory.Delete(cachePath, true);
        Debug.Log("YooAsset ClearHostPlayModeCache Finish.");
    }

    private static void BuildPackage(string packageName, string packageBigVersion, string packageSmallVersion)
    {
        Debug.Log($"[YooAsset] 开始构建 : {EditorUserBuildSettings.activeBuildTarget} {packageName} {packageBigVersion} {packageSmallVersion}");

        //清理老资源
        string outputPackDir = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{EditorUserBuildSettings.activeBuildTarget}/{packageName}";
        if (Directory.Exists(outputPackDir))
        {
            foreach (var p in Directory.GetDirectories(outputPackDir))
            {
                if (p.EndsWith("OutputCache") || p.EndsWith("OutputCache/") || p.EndsWith("OutputCache\\")) continue;
                Directory.Delete(p, true);
            }

            foreach (var p in Directory.GetFiles(outputPackDir))
            {
                File.Delete(p);
            }
        }

        EBuildPipeline buildPipeline = EBuildPipeline.BuiltinBuildPipeline;

        IBuildPipeline pipeline = null;
        BuildParameters buildParameters = null;

        if (buildPipeline == EBuildPipeline.BuiltinBuildPipeline)
        {
            // 构建参数
            BuiltinBuildParameters builtinBuildParameters = new BuiltinBuildParameters();

            // 执行构建
            pipeline = new BuiltinBuildPipeline();
            buildParameters = builtinBuildParameters;

            builtinBuildParameters.CompressOption = ECompressOption.LZ4;
        }
        else
        {
            ScriptableBuildParameters scriptableBuildParameters = new ScriptableBuildParameters();

            // 执行构建
            pipeline = new ScriptableBuildPipeline();
            buildParameters = scriptableBuildParameters;

            scriptableBuildParameters.CompressOption = ECompressOption.LZ4;
        }

        buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
        buildParameters.BuildTarget = EditorUserBuildSettings.activeBuildTarget;
        buildParameters.BuildPipeline = buildPipeline.ToString();
        buildParameters.BuildMode = EBuildMode.IncrementalBuild;
        buildParameters.PackageName = packageName;
        buildParameters.PackageVersion = packageSmallVersion;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.FileNameStyle = EFileNameStyle.HashName;
        buildParameters.BuildinFileCopyOption = EBuildinFileCopyOption.ClearAndCopyByTags;
        buildParameters.BuildinFileCopyParams = "BASE_RES";
#if UNITY_IOS
        buildParameters.EncryptionServices = new AESStreamEncryption();
#else
        buildParameters.EncryptionServices = new FileStreamEncryption();
#endif
        // 启用共享资源打包
        buildParameters.EnableSharePackRule = true;

        // 执行构建
        var buildResult = pipeline.Run(buildParameters, true);
        if (buildResult.Success)
        {
            Debug.Log($"[YooAsset] 构建成功 : {buildResult.OutputPackageDirectory}");

            string outputPackPath = $"{outputPackDir}/{packageBigVersion}";
            if (Directory.Exists(outputPackPath))
            {
                Directory.Delete(outputPackPath, true);
            }

            string smallVersionOutputPackPath = $"{outputPackDir}/{packageSmallVersion}";
            Directory.Move(smallVersionOutputPackPath, outputPackPath);
        }
        else
        {
            throw new Exception($"[YooAsset] 构建失败 : {buildResult.ErrorInfo}");
        }

        // 构造Version文件
        GenerateVersionFile(outputPackDir, packageBigVersion, packageSmallVersion);
    }

    // 构造Version文件，用于配合GM后台资源上传工具
    private static void GenerateVersionFile(string outputPackDir, string packageBigVersion, string packageSmallVersion)
    {
        VersionInfo versionInfo = new VersionInfo(outputPackDir, packageBigVersion, packageSmallVersion);
        string buildCode = "0";
#if UNITY_ANDROID
            buildCode = PlayerSettings.Android.bundleVersionCode.ToString();
#elif UNITY_IOS
        buildCode = PlayerSettings.iOS.buildNumber;
#else
            buildCode = "-1";
#endif
        StreamWriter versionStreamWriter = File.CreateText($"{outputPackDir}/Version.{buildCode}.txt");
        versionStreamWriter.Write(versionInfo.ToJson());
        versionStreamWriter.Close();
        versionStreamWriter.Dispose();

        Debug.Log($"[YooAsset] Version.{buildCode}.txt 构造成功。");
    }

    /// <summary>
    /// 创建加密类实例
    /// </summary>
    private static IEncryptionServices CreateEncryptionInstance(string packageName, EBuildPipeline buildPipeline)
    {
        var encryptionClassName = AssetBundleBuilderSetting.GetPackageEncyptionClassName(packageName, buildPipeline);
        var encryptionClassTypes = EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
        var classType = encryptionClassTypes.Find(x => x.FullName != null && x.FullName.Equals(encryptionClassName));
        if (classType != null)
        {
            Debug.Log($"Use Encryption {classType}");
            return (IEncryptionServices)Activator.CreateInstance(classType);
        }
        else
        {
            return null;
        }
    }
}