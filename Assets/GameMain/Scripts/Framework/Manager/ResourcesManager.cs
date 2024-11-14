using System;
using Cosmos;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourcesManager : MonoSingleton<ResourcesManager>
    {
        private bool m_UseSd;

        public bool HasAsset(string name)
        {
            var path = $"Assets/Export/{name}";
            //return GameModule.Resource.CheckLocationValid(path);
            return true;
        }

        public T LoadResource<T>(string name, bool forceBundle = false, bool addToCache = true, string assetDeepPath = null) where T : Object
        {
            //todo 这里思考一下
            //return GameModule.Resource.LoadAsset<T>($"Assets/Export/{name}");
            return null;
        }

        public Sprite GetSpriteVariant(string atlasName, string spriteName, bool forceBundle = false, bool ignoreErrorLog = false)
        {
            var spriteAtlas = LoadSpriteAtlasVariant(atlasName);
            if (null == spriteAtlas)
            {
                Debug.LogError($"SpriteAtlas Path Error: {atlasName}, in GetSpriteVariant");
                return null;
            }

            return spriteAtlas.GetSprite(spriteName);
        }

        public SpriteAtlas LoadSpriteAtlasVariant(string atlasName, bool forceBundle = false)
        {
            AtlasPathNode atlasPathNode = AtlasConfigController.Instance.GetAtlasPath(atlasName);
            if (atlasPathNode == null)
            {
                Debug.LogError($"SpriteAtlas Path Error: {atlasName}, in GetAtlasPath");
                return null;
            }

            string path = m_UseSd ? atlasPathNode.SdPath : atlasPathNode.HdPath;

            var spriteAtlas = LoadResource<SpriteAtlas>(path);
            if (null == spriteAtlas)
            {
                Debug.LogError($"SpriteAtlas Path Error: {atlasName}, in LoadResource");
                return null;
            }

            return spriteAtlas;
        }

        public void UseSDAtlas(bool useSd)
        {
            m_UseSd = useSd;
        }
    }
}