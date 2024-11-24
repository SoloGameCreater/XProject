using UnityEngine;

namespace Framework.Wrapper
{
    public class RendererWrapper
    {
        private readonly Renderer[] _renderers;
        private Material[][] _originalMaterials;
        private Shader[] _originalShaders;
        
        
        public RendererWrapper(Renderer[] renderers)
        {
            if (renderers == null)
            {
               Debug.LogError("RendererWrapper: renderer is null"); 
            }
            _renderers = renderers;

            if (_renderers.Length > 0)
            {
                _originalMaterials = new Material[_renderers.Length][];
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _originalMaterials[i] = _renderers[i].materials;
                }
            } 
        }

        public void SetVisible(bool visible)
        {
            if (_renderers != null)
            {
                foreach (var renderer in _renderers)
                {
                    if (renderer != null)
                    {
                        renderer.enabled = visible;
                    }
                }
            }          
        }
    }
}