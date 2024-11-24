using UnityEngine;
using UnityEngine.UI;

namespace CommonExtensions
{
    public static class Extensions
    {
        public static TComponent GetOrAddComponent<TComponent>(this Component _this) where TComponent : Component
        {
            return GetOrAddComponent<TComponent>(_this.gameObject);
        }

        public static TComponent GetOrAddComponent<TComponent>(this GameObject _this) where TComponent : Component
        {
            if (_this.TryGetComponent(out TComponent component))
            {
                return component;
            }

            return _this.AddComponent<TComponent>();
        }

        public static GameObject FindChildByPath(this GameObject _this, string path)
        {
            return _this.transform.Find(path)?.gameObject;
        }

        public static T GetComponentAtChild<T>(this GameObject _this, string path)
        {
            return FindChildByPath(_this, path).GetComponent<T>();
        }

        public static T GetComponentAtChild<T>(this Transform _this, string path)
        {
            return FindChildByPath(_this.gameObject, path).GetComponent<T>();
        }

        public static float GetNormalizedTime(this AnimatorStateInfo _this)
        {
            return _this.loop ? _this.normalizedTime - (int) _this.normalizedTime : _this.normalizedTime;
        }

        public static Vector2 XZ(this Vector3 _this)
        {
            return new Vector2(_this.x, _this.z);
        }

        public static Vector2 Rotate(this Vector2 _this, float angle)
        {
            float x    = _this.x;
            float y    = _this.y;
            float sin  = Mathf.Sin(Mathf.PI * angle / 180);
            float cos  = Mathf.Cos(Mathf.PI * angle / 180);
            float newX = x * cos  + y * sin;
            float newY = x * -sin + y * cos;
            _this.x = newX;
            _this.y = newY;
            return _this;
        }

        /// <summary>
        /// 忽略y轴
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static Vector3 IgnoreY(this Vector3 _this)
        {
            _this.y = 0f;
            return _this;
        }

        // public static bool Overlaps(this RectTransform _this, RectTransform other)
        // {
        //     return _this.GetWorldRect().Overlaps(other.GetWorldRect());
        // }
        //
        // public static Rect GetWorldRect(this RectTransform _this)
        // {
        //     _this.GetWorldCorners(Utilities.s_Corners);
        //     float x      = Utilities.s_Corners[0].x;
        //     float y      = Utilities.s_Corners[0].y;
        //     float width  = Utilities.s_Corners[2].x - x;
        //     float height = Utilities.s_Corners[2].y - y;
        //     return new Rect(x, y, width, height);
        // }

        // public static Rect GetWorldRect(this RectTransform _this, Rect rect)
        // {
        //     float x    = rect.x;
        //     float y    = rect.y;
        //     float xMax = rect.xMax;
        //     float yMax = rect.yMax;
        //     Utilities.s_Corners[0] = new Vector3(x,    y,    0.0f);
        //     Utilities.s_Corners[1] = new Vector3(x,    yMax, 0.0f);
        //     Utilities.s_Corners[2] = new Vector3(xMax, yMax, 0.0f);
        //     Utilities.s_Corners[3] = new Vector3(xMax, y,    0.0f);
        //     Matrix4x4 localToWorldMatrix = _this.localToWorldMatrix;
        //     for (int index = 0; index < 4; ++index)
        //     {
        //         Utilities.s_Corners[index] = localToWorldMatrix.MultiplyPoint(Utilities.s_Corners[index]);
        //     }
        //
        //     x = Utilities.s_Corners[0].x;
        //     y = Utilities.s_Corners[0].y;
        //     float width  = Utilities.s_Corners[2].x - x;
        //     float height = Utilities.s_Corners[2].y - y;
        //     return new Rect(x, y, width, height);
        // }
        //
        // public static Vector3 GetWorldCenter(this RectTransform _this)
        // {
        //     _this.GetWorldCorners(Utilities.s_Corners);
        //     return (Utilities.s_Corners[0] + Utilities.s_Corners[2]) / 2;
        // }

        public static bool IsStateFinish(this Animator _this, int stateHash, int layer = 0)
        {
            AnimatorStateInfo stateInfo = _this.GetCurrentAnimatorStateInfo(layer);
            return stateHash != stateInfo.shortNameHash || stateInfo.normalizedTime >= 1.0f;
        }

        public static bool Approximately(this Vector3 position, Vector3 comparePosition)
        {
            return Mathf.Approximately(position.x, comparePosition.x) && Mathf.Approximately(position.y, comparePosition.y) && Mathf.Approximately(position.z, comparePosition.z);
        }

        public static string GetHierarchyPath(this Transform _this, Transform stopParent = null, char split = '/')
        {
            string path = _this.name;
            while (_this.parent != null && _this.parent != stopParent)
            {
                _this = _this.parent;
                path  = $"{_this.name}{split}{path}";
            }

            return path;
        }

        public static void Stretch(this RectTransform rectTransform)
        {
            rectTransform.anchorMin          = Vector2.zero;
            rectTransform.anchorMax          = Vector2.one;
            rectTransform.offsetMin          = Vector2.zero;
            rectTransform.offsetMax          = Vector2.zero;
            rectTransform.anchoredPosition3D = Vector2.zero;
            rectTransform.localRotation      = Quaternion.identity;
            rectTransform.localScale         = Vector3.one;
        }

        public static void Adapt(this Image image)
        {
            if (image == null || image.sprite == null)
            {
                return;
            }
            
            var imgShopIconRectSize = image.GetComponent<RectTransform>().sizeDelta;
            if (image.sprite.texture.height < image.sprite.texture.width)
            {
                var baseX = 1f;
                if (image.sprite.texture.width < imgShopIconRectSize.x)
                {
                    baseX = image.sprite.texture.width * 1.0f / imgShopIconRectSize.x;
                }

                image.transform.localScale = new Vector3(baseX, baseX * (image.sprite.texture.height * 1.0f / image.sprite.texture.width) * (imgShopIconRectSize.x / imgShopIconRectSize.y), 1);
            }
            else
            {
                var baseY = 1f;
                if (image.sprite.texture.height < imgShopIconRectSize.y)
                {
                    baseY = image.sprite.texture.height * 1.0f / imgShopIconRectSize.y;
                }

                image.transform.localScale = new Vector3(baseY * (image.sprite.texture.width * 1.0f / image.sprite.texture.height) * (imgShopIconRectSize.y / imgShopIconRectSize.x), baseY, 1);
            }
        }
        
        public static GameObject CreateOrGetChild(this GameObject self, string name)
        {
            var childTransform = self.transform.Find(name);
            if (childTransform != null)
            {
                return childTransform.gameObject;
            }
            
            var child = new GameObject(name);
            child.transform.SetParent(self.transform);
            child.transform.localPosition = Vector3.zero;
            child.transform.localScale = Vector3.one;

            return child;
        }
        
        public static float GetAnimationTime(this Animator _this, string animName)
        {
            var clips = _this.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name.Equals(animName))
                {
                    return clip.length;
                }
            }

            return 0f;
        }
        
        public static void Reset(this Transform _this)
        {
            if (_this == null) {
                return;
            }
            _this.localScale = Vector3.one;
            _this.localRotation = Quaternion.identity;
            _this.localPosition = Vector3.zero;
        }

        public static void SetAlpha(this Graphic _this, float alpha)
        {
            var color = _this.color;
            color.a = alpha;
            _this.color = color;
        }

        public static void SetLocalXY(this Transform _this, Vector2 xy)
        {
            var pos = _this.localPosition;
            pos.x = xy.x;
            pos.y = xy.y;
            _this.localPosition = pos;
        }
        
        public static void SetWorldXY(this Transform _this, Vector2 xy)
        {
            var pos = _this.position;
            pos.x = xy.x;
            pos.y = xy.y;
            _this.position = pos;
        }
    }
}