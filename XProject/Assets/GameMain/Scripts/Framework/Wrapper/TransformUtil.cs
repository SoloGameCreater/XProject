using UnityEngine;

namespace Framework
{
    public static class TransformUtil
    {
        public static bool ToRotationH(Vector3 dir, out Quaternion rotation)
        {
            dir.y = 0.0f;
            if (dir.sqrMagnitude >float.Epsilon)
            {
                dir.Normalize();
                rotation = Quaternion.LookRotation(dir, Vector3.up);
                return true;
            }

            rotation = Quaternion.identity;
            return false;
        }

        public static bool ToRotation(Vector3 dir, Vector3 up, out Quaternion rotation)
        {
            if (dir.sqrMagnitude > float.Epsilon &&
                up.sqrMagnitude > float.Epsilon)
            {
                rotation = Quaternion.LookRotation(dir, up);
                return true;
            }

            rotation = Quaternion.identity;
            return false;
        }

        public static float GetAngleH(Quaternion rotation)
        {
            var angle = 0.0f;
            var axis = Vector3.zero;
            rotation.ToAngleAxis(out angle, out axis);
            return angle * axis.y;
        }

        public static void CopyTransform(Transform src, Transform dst)
        {
            dst.parent = src.parent;
            dst.position = src.position;
            dst.rotation = src.rotation;
            dst.localPosition = src.localPosition;
            dst.localRotation = src.localRotation;
            dst.localScale = src.localScale;
        }
    }
}