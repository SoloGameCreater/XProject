using UnityEngine;

namespace TripleMerge
{
    public class ItemBase : MonoBehaviour
    {
        protected int TouchIndex;
        
        public virtual void OnPointerDown(int touchIndex = 0)
        {
            Debug.Log($"{name} on pointer down, touchIndex is {touchIndex}");
            TouchIndex = touchIndex;
        }

        public virtual void OnPointerUp()
        {
        }

        public virtual void OnPointerClick()
        {
        }
    }
}