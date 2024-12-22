using System.Reflection.Emit;
using UnityEngine;

namespace SomeWhere
{
    public class TableViewCell : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform RectTrans
        {
            get
            {
                if (!_rectTransform) _rectTransform = transform as RectTransform;

                return _rectTransform;
            }
        }
    }
}