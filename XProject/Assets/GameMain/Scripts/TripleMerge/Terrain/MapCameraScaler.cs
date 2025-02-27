using Sirenix.OdinInspector;
using UnityEngine;

namespace TripleMerge
{
    public class MapCameraScaler : MonoBehaviour
    {
        [LabelText("相机最小缩放")]
        public float MinScale = 3f;
        [LabelText("相机最大缩放")]
        public float MaxScale = 9f;
    }
}