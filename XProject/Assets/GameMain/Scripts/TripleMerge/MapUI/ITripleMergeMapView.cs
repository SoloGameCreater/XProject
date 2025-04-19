using UnityEngine;

namespace TripleMerge
{
    public interface ITripleMergeMapView
    {
        public Transform ViewRoot { get; set; }
        
        public void OnInitialize();
        
        public void OnShow(params object[] extra);

        public void OnHide();

        public void OnUpdate();
    }
}
