using System;
using Cosmos;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourcesManager : Manager<ResourcesManager>
    {
        private bool m_UseSd;

        public bool HasAsset(string name)
        {
            var path = $"Assets/ExtraRes/{name}";
            return GameModule.Resource.CheckLocationValid(path);
        }

        public T LoadResource<T>(string name, bool forceBundle = false, bool addToCache = true, string assetDeepPath = null) where T : Object
        {
            return GameModule.Resource.LoadAsset<T>($"Assets/ExtraRes/{name}");
        }
        public T LoadResourceByKey<T>(string key, bool forceBundle = false, bool addToCache = true, string assetDeepPath = null) where T : Object
        {
            return GameModule.Resource.LoadAsset<T>(key);
        }
        public async UniTaskVoid LoadResourceAsync<T>(string name, Action<T> OnFinished = null, bool isAddCache = true) where T : Object
        {
            var obj = await GameModule.Resource.LoadAssetAsync<T>($"Assets/ExtraRes/{name}");
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
            AtlasPathNode atlasPathNode = AtlasConfigController.Instance.GetAtlasPath(atlasName);
            if (atlasPathNode == null)
            {
                DebugUtil.LogError($"SpriteAtlas Path Error: {atlasName}, in GetAtlasPath");
                return null;
            }

            string path = m_UseSd ? atlasPathNode.SdPath : atlasPathNode.HdPath;

            var spriteAtlas = LoadResource<SpriteAtlas>(path);
            if (null == spriteAtlas)
            {
                DebugUtil.LogError($"SpriteAtlas Path Error: {atlasName}, in LoadResource");
                return null;
            }

            return spriteAtlas;
        }

        public void UseSDAtlas(bool useSd)
        {
            m_UseSd = useSd;
        }

        public void ReleaseRes(string path, bool free = false)
        {
        }

        public void UnloadSpriteAtlasImmediateVariant(string atlasName)
        {
        }
    }
}