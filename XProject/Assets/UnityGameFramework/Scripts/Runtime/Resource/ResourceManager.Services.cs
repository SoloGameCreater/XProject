using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using GameFramework.Runtime;
using UnityEngine;
using YooAsset;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string _defaultHostServer;
            private readonly string _fallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                _defaultHostServer = defaultHostServer;
                _fallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{_defaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{_fallbackHostServer}/{fileName}";
            }
        }
        
        /// <summary>
        /// 资源文件流加载解密类
        /// </summary>
        private class FileStreamDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStream(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleStream bundleStream = new BundleStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            private static uint GetManagedReadBufferSize()
            {
                return 8192;
            }
        }
        
        public class AESStreamDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleAESStream bundleStream = new BundleAESStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStream(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                BundleAESStream bundleStream = new BundleAESStream(fileInfo.FileLoadPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                managedStream = bundleStream;
                return AssetBundle.LoadFromStreamAsync(bundleStream, fileInfo.ConentCRC, GetManagedReadBufferSize());
            }

            private static uint GetManagedReadBufferSize()
            {
                return 8192;
            }
        }

        /// <summary>
        /// 资源文件偏移加载解密类
        /// </summary>
        private class FileOffsetDecryption : IDecryptionServices
        {
            /// <summary>
            /// 同步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundle IDecryptionServices.LoadAssetBundle(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                managedStream = null;
                return AssetBundle.LoadFromFile(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
            }

            /// <summary>
            /// 异步方式获取解密的资源包对象
            /// 注意：加载流对象在资源包对象释放的时候会自动释放
            /// </summary>
            AssetBundleCreateRequest IDecryptionServices.LoadAssetBundleAsync(DecryptFileInfo fileInfo, out Stream managedStream)
            {
                managedStream = null;
                return AssetBundle.LoadFromFileAsync(fileInfo.FileLoadPath, fileInfo.ConentCRC, GetFileOffset());
            }

            private static ulong GetFileOffset()
            {
                return 32;
            }
        }
    }

    /// <summary>
    /// 资源文件解密流
    /// </summary>
    public class BundleStream : FileStream
    {
        public const byte KEY = 126;

        public BundleStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
        }

        public BundleStream(string path, FileMode mode) : base(path, mode)
        {
        }

        public override int Read(byte[] array, int offset, int count)
        {
            var index = base.Read(array, offset, count);
            for (int i = 0; i < array.Length; i++)
            {
                array[i] ^= KEY;
            }

            return index;
        }
    }

    public class BundleAESStream : FileStream
    {
        private static string PASSWORD = "CD2MatchFrenzy123";
        private static byte[] SALT = Encoding.UTF8.GetBytes("MatchFrenzyRandSALT123");

        //private Stream baseStream;
        private AesManaged aes;
        private ICryptoTransform encryptor;

        /// <param name="salt">//** WARNING **: MUST be unique for each stream otherwise there is NO security</param>
        public BundleAESStream(string path, FileMode mode, FileAccess access, FileShare share) : base(path, mode, access, share)
        {
            using (var key = new PasswordDeriveBytes(PASSWORD, SALT))
            {
                aes = new AesManaged();
                aes.KeySize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = new byte[16]; //zero buffer is adequate since we have to use new salt for each stream
                encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            }
        }

        private void cipher(byte[] buffer, int offset, int count, long streamPos)
        {
            //find block number
            var blockSizeInByte = aes.BlockSize / 8;
            var blockNumber = (streamPos / blockSizeInByte) + 1;
            var keyPos = streamPos % blockSizeInByte;

            //buffer
            var outBuffer = new byte[blockSizeInByte];
            var nonce = new byte[blockSizeInByte];
            var init = false;

            for (int i = offset; i < count; i++)
            {
                //encrypt the nonce to form next xor buffer (unique key)
                if (!init || (keyPos % blockSizeInByte) == 0)
                {
                    BitConverter.GetBytes(blockNumber).CopyTo(nonce, 0);
                    encryptor.TransformBlock(nonce, 0, nonce.Length, outBuffer, 0);
                    if (init) keyPos = 0;
                    init = true;
                    blockNumber++;
                }

                buffer[i] ^= outBuffer[keyPos]; //simple XOR with generated unique key
                keyPos++;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var streamPos = Position;
            var ret = base.Read(buffer, offset, count);
            cipher(buffer, offset, count, streamPos);
            return ret;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            cipher(buffer, offset, count, Position);
            base.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                encryptor?.Dispose();
                aes?.Dispose();
            }

            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// 资源文件查询服务类
    /// </summary>
    public class GameQueryServices : IBuildinQueryServices
    {
        /// <summary>
        /// 查询内置文件的时候，是否比对文件哈希值
        /// </summary>
        public static bool CompareFileCRC = false;

        public bool Query(string packageName, string fileName, string fileCRC)
        {
            // 注意：fileName包含文件格式
            return StreamingAssetsHelper.FileExists($"{YooAssetSettingsData.Setting.DefaultYooFolderName}/{packageName}/{fileName}");
        }
    }

    public sealed class StreamingAssetsHelper
    {
        private static readonly Dictionary<string, bool> _cacheData = new Dictionary<string, bool>(1000);

#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaClass _unityPlayerClass;

        public static AndroidJavaClass UnityPlayerClass
        {
            get
            {
                if (_unityPlayerClass == null)
                    _unityPlayerClass = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
                return _unityPlayerClass;
            }
        }

        private static AndroidJavaObject _currentActivity;

        public static AndroidJavaObject CurrentActivity
        {
            get
            {
                if (_currentActivity == null)
                    _currentActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                return _currentActivity;
            }
        }

        /// <summary>
        /// 利用安卓原生接口查询内置文件是否存在
        /// </summary>
        public static bool FileExists(string filePath)
        {
            if (_cacheData.TryGetValue(filePath, out bool result) == false)
            {
                result = CurrentActivity.Call<bool>("CheckAssetExist", filePath);
                _cacheData.Add(filePath, result);
            }

            return result;
        }
#else
        public static bool FileExists(string filePath)
        {
            if (_cacheData.TryGetValue(filePath, out bool result) == false)
            {
                result = System.IO.File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath, filePath));
                _cacheData.Add(filePath, result);
            }

            return result;
        }
#endif
    }
    
#if UNITY_ANDROID && UNITY_EDITOR
    /// <summary>
    /// 为Github对开发者的友好，采用自动补充UnityPlayerActivity.java文件的通用姿势满足各个开发者
    /// </summary>
    internal class AndroidPost : UnityEditor.Android.IPostGenerateGradleAndroidProject
    {
        public int callbackOrder => 99;
        public void OnPostGenerateGradleAndroidProject(string path)
        {
            path = path.Replace("\\", "/");
            string untityActivityFilePath = $"{path}/src/main/java/com/unity3d/player/UnityPlayerActivity.java";
            var readContent = System.IO.File.ReadAllLines(untityActivityFilePath);
            string postContent =
                "    //auto-gen-function \n" +
                "    public boolean CheckAssetExist(String filePath) \n" +
                "    { \n" +
                "        android.content.res.AssetManager assetManager = getAssets(); \n" +
                "        try \n" +
                "        { \n" +
                "            java.io.InputStream inputStream = assetManager.open(filePath); \n" +
                "            if (null != inputStream) \n" +
                "            { \n" +
                "                 inputStream.close(); \n" +
                "                 return true; \n" +
                "            } \n" +
                "        } \n" +
                "        catch(java.io.IOException e) \n" +
                "        { \n" +
                "            e.printStackTrace(); \n" +
                "        } \n" +
                "        return false; \n" +
                "    } \n" +
                "}";

            if (CheckFunctionExist(readContent) == false)
                readContent[readContent.Length - 1] = postContent;
            System.IO.File.WriteAllLines(untityActivityFilePath, readContent);
        }
        private bool CheckFunctionExist(string[] contents)
        {
            for (int i = 0; i < contents.Length; i++)
            {
                if (contents[i].Contains("CheckAssetExist"))
                {
                    return true;
                }
            }
            return false;
        }
    }
#endif
    
    // public class StreamingAssetsDefine
    // {
    //     /// <summary>
    //     /// 根目录名称（保持和YooAssets资源系统一致）
    //     /// </summary>
    //     public const string RootFolderName = "assetpack";
    // }

// #if UNITY_EDITOR
//     public sealed class StreamingAssetsHelper
//     {
//         public static void Init()
//         {
//         }
//
//         public static bool FileExists(string packageName, string fileName, string fileCRC)
//         {
//             string filePath = Path.Combine(Application.streamingAssetsPath, YooAssetSettingsData.Setting.DefaultYooFolderName, packageName, fileName);
//             if (File.Exists(filePath))
//             {
//                 if (GameQueryServices.CompareFileCRC)
//                 {
//                     string crc32 = YooAsset.HashUtility.FileCRC32(filePath);
//                     return crc32 == fileCRC;
//                 }
//                 else
//                 {
//                     return true;
//                 }
//             }
//             else
//             {
//                 return false;
//             }
//         }
//     }
// #else
// public sealed class StreamingAssetsHelper
// {
//     private class PackageQuery
//     {
//         public readonly Dictionary<string, BuildinFileManifest.Element> Elements = new Dictionary<string, BuildinFileManifest.Element>(1000);
//     }
//
//     private static bool _isInit = false;
//     private static readonly Dictionary<string, PackageQuery> _packages = new Dictionary<string, PackageQuery>(10);
//
//     /// <summary>
//     /// 初始化
//     /// </summary>
//     public static void Init()
//     {
//         if (_isInit == false)
//         {
//             _isInit = true;
//
//             var manifest = Resources.Load<BuildinFileManifest>("BuildinFileManifest");
//             if (manifest != null)
//             {
//                 foreach (var element in manifest.BuildinFiles)
//                 {
//                     if (_packages.TryGetValue(element.PackageName, out PackageQuery package) == false)
//                     {
//                         package = new PackageQuery();
//                         _packages.Add(element.PackageName, package);
//                     }
//                     package.Elements.Add(element.FileName, element);
//                 }
//             }
//         }
//     }
//
//     /// <summary>
//     /// 内置文件查询方法
//     /// </summary>
//     public static bool FileExists(string packageName, string fileName, string fileCRC32)
//     {
//         if (_isInit == false)
//             Init();
//
//         if (_packages.TryGetValue(packageName, out PackageQuery package) == false)
//             return false;
//
//         if (package.Elements.TryGetValue(fileName, out var element) == false)
//             return false;
//
//         if (GameQueryServices.CompareFileCRC)
//         {
//             return element.FileCRC32 == fileCRC32;
//         }
//         else
//         {
//             return true;
//         }
//     }
// }
// #endif


// #if UNITY_EDITOR
//     internal class PreprocessBuild : UnityEditor.Build.IPreprocessBuildWithReport
//     {
//         public int callbackOrder
//         {
//             get { return 0; }
//         }
//
//         /// <summary>
//         /// 在构建应用程序前处理
//         /// 原理：在构建APP之前，搜索StreamingAssets目录下的所有资源文件，然后将这些文件信息写入内置清单，内置清单存储在Resources文件夹下。
//         /// </summary>
//         public void OnPreprocessBuild(UnityEditor.Build.Reporting.BuildReport report)
//         {
//             string saveFilePath = "Assets/AATemp/Resources/BuildinFileManifest.asset";
//             if (File.Exists(saveFilePath))
//             {
//                 File.Delete(saveFilePath);
//                 UnityEditor.AssetDatabase.SaveAssets();
//                 UnityEditor.AssetDatabase.Refresh();
//             }
//
//             string folderPath = $"{Application.dataPath}/StreamingAssets/{YooAssetSettingsData.Setting.DefaultYooFolderName}";
//             DirectoryInfo root = new DirectoryInfo(folderPath);
//             if (root.Exists == false)
//             {
//                 Debug.LogWarning($"没有发现YooAsset内置目录 : {folderPath}");
//                 return;
//             }
//
//             var manifest = ScriptableObject.CreateInstance<BuildinFileManifest>();
//             FileInfo[] files = root.GetFiles("*", SearchOption.AllDirectories);
//             foreach (var fileInfo in files)
//             {
//                 if (fileInfo.Extension == ".meta")
//                     continue;
//                 if (fileInfo.Name.StartsWith("PackageManifest_"))
//                     continue;
//
//                 BuildinFileManifest.Element element = new BuildinFileManifest.Element();
//                 element.PackageName = fileInfo.Directory.Name;
//                 element.FileCRC32 = YooAsset.HashUtility.FileCRC32(fileInfo.FullName);
//                 element.FileName = fileInfo.Name;
//                 manifest.BuildinFiles.Add(element);
//             }
//
//             if (Directory.Exists("Assets/AATemp/Resources") == false)
//                 Directory.CreateDirectory("Assets/AATemp/Resources");
//             UnityEditor.AssetDatabase.CreateAsset(manifest, saveFilePath);
//             UnityEditor.AssetDatabase.SaveAssets();
//             UnityEditor.AssetDatabase.Refresh();
//             Debug.Log($"一共{manifest.BuildinFiles.Count}个内置文件，内置资源清单保存成功 : {saveFilePath}");
//         }
//     }
// #endif
}