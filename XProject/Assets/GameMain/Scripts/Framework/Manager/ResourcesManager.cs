using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourcesManager : Manager<ResourcesManager>
    {
        private bool m_UseSd;

        private sealed class CacheEntry
        {
            public Object Asset;
            public int RefCount;
        }

        private readonly Dictionary<string, CacheEntry> m_AssetCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> m_AtlasPathCache = new(StringComparer.OrdinalIgnoreCase);

        private static string NormalizePath(string path)
        {
            return string.IsNullOrEmpty(path) ? string.Empty : path.Replace('\\', '/').Trim();
        }

        private static string BuildFullPath(string path)
        {
            string normalized = NormalizePath(path);
            if (string.IsNullOrEmpty(normalized))
            {
                return string.Empty;
            }

            if (normalized.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
            {
                return normalized;
            }

            return $"Assets/ExtraRes/{normalized}";
        }

        private void RetainAsset(string path, Object asset)
        {
            if (asset == null)
            {
                return;
            }

            string fullPath = BuildFullPath(path);
            if (string.IsNullOrEmpty(fullPath))
            {
                return;
            }

            if (!m_AssetCache.TryGetValue(fullPath, out CacheEntry cacheEntry))
            {
                cacheEntry = new CacheEntry
                {
                    Asset = asset,
                    RefCount = 0
                };
                m_AssetCache.Add(fullPath, cacheEntry);
            }
            else if (cacheEntry.Asset == null)
            {
                cacheEntry.Asset = asset;
            }

            cacheEntry.RefCount++;
        }

        public bool HasAsset(string name)
        {
            var path = BuildFullPath(name);
            return GameModule.Resource.CheckLocationValid(path);
        }

        public T LoadResource<T>(string name, bool forceBundle = false, bool addToCache = true, string assetDeepPath = null) where T : Object
        {
            string fullPath = BuildFullPath(name);
            T asset = GameModule.Resource.LoadAsset<T>(fullPath);
            RetainAsset(fullPath, asset);
            return asset;
        }

        public T LoadResourceByKey<T>(string key, bool forceBundle = false, bool addToCache = true, string assetDeepPath = null) where T : Object
        {
            string fullPath = BuildFullPath(key);
            T asset = GameModule.Resource.LoadAsset<T>(fullPath);
            RetainAsset(fullPath, asset);
            return asset;
        }

        public async UniTaskVoid LoadResourceAsync<T>(string name, Action<T> OnFinished = null, bool isAddCache = true) where T : Object
        {
            string fullPath = BuildFullPath(name);
            var obj = await GameModule.Resource.LoadAssetAsync<T>(fullPath);
            RetainAsset(fullPath, obj);
            OnFinished?.Invoke(obj);
        }

        public Sprite GetSpriteVariant(string atlasName, string spriteName, bool forceBundle = false, bool ignoreErrorLog = false)
        {
            var spriteAtlas = LoadSpriteAtlasVariant(atlasName);
            if (null == spriteAtlas)
            {
                DebugUtil.LogError($"SpriteAtlas Path Error: {atlasName}, in GetSpriteVariant");
                return null;
            }

            return spriteAtlas.GetSprite(spriteName);
        }

        public SpriteAtlas LoadSpriteAtlasVariant(string atlasName, bool forceBundle = false)
        {
            if (string.IsNullOrEmpty(atlasName))
            {
                DebugUtil.LogError("SpriteAtlas Name is empty, in LoadSpriteAtlasVariant");
                return null;
            }

            AtlasPathNode atlasPathNode = AtlasConfigController.Instance.GetAtlasPath(atlasName);
            if (atlasPathNode == null)
            {
                DebugUtil.LogError($"SpriteAtlas Path Error: {atlasName}, in GetAtlasPath");
                return null;
            }

            string path = atlasPathNode.HdPath;
            m_AtlasPathCache[atlasName] = path;

            string fullPath = BuildFullPath(path);
            if (m_AssetCache.TryGetValue(fullPath, out CacheEntry cachedEntry) && cachedEntry.Asset is SpriteAtlas cachedAtlas)
            {
                cachedEntry.RefCount++;
                return cachedAtlas;
            }

            var spriteAtlas = LoadResource<SpriteAtlas>(path, addToCache: true);
            if (null == spriteAtlas)
            {
                DebugUtil.LogError($"SpriteAtlas Path Error: {atlasName}, in LoadResource");
                return null;
            }

            return spriteAtlas;
        }

        public void ReleaseRes(string path, bool free = false)
        {
            string fullPath = BuildFullPath(path);
            if (string.IsNullOrEmpty(fullPath))
            {
                DebugUtil.LogWarning("ReleaseRes path is empty.");
                return;
            }

            if (!m_AssetCache.TryGetValue(fullPath, out CacheEntry cacheEntry))
            {
                return;
            }

            if (free)
            {
                cacheEntry.RefCount = 0;
            }
            else
            {
                cacheEntry.RefCount = Mathf.Max(0, cacheEntry.RefCount - 1);
            }

            if (cacheEntry.RefCount > 0)
            {
                return;
            }

            if (cacheEntry.Asset != null)
            {
                GameModule.Resource.UnloadAsset(cacheEntry.Asset);
            }

            m_AssetCache.Remove(fullPath);
        }

        public void UnloadSpriteAtlasImmediateVariant(string atlasName)
        {
            if (string.IsNullOrEmpty(atlasName))
            {
                return;
            }

            if (!m_AtlasPathCache.TryGetValue(atlasName, out string atlasPath))
            {
                AtlasPathNode atlasPathNode = AtlasConfigController.Instance.GetAtlasPath(atlasName);
                if (atlasPathNode == null)
                {
                    DebugUtil.LogWarning($"Unload atlas failed, path not found: {atlasName}");
                    return;
                }

                atlasPath = atlasPathNode.HdPath;
            }

            ReleaseRes(atlasPath, true);
            m_AtlasPathCache.Remove(atlasName);
        }
    }
}
