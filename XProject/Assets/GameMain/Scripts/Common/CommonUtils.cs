using System;
using GameFramework.Localization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class CommonUtils
{
    public static string GetFontAssetNameWithLanguage(Language language)
    {
        return language switch
               {
                   Language.ChineseSimplified => "zh",
                   Language.English => "LiberationSans",
                   _ => "LiberationSans"
               };
    }
    public static async Task PlayAnimationAsync(Animator animator, string aniName, Action callBack = null)
    {
        animator.Play(aniName);
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        float duration = 0f;
        if (null != controller)
        {
            foreach (var clip in controller.animationClips)
            {
                if (clip.name.Equals(aniName))
                {
                    duration = clip.length;
                    break;
                }
            }
        }

        if (duration > 0f)
        {
            await Task.Delay((int)(duration * 1000.0f));
        }

        callBack?.Invoke();
    }
    public static void NotchAdapt(RectTransform rectTransform)
    {
        if (rectTransform == null) return;
        rectTransform.offsetMax = new Vector2(0, GetSafeAreaOffset());
    }
    public static float GetSafeAreaOffset()
    {
        int safeAreaOffset = (int)(Screen.height - Screen.safeArea.yMax);
        if (safeAreaOffset == 0) return 0.0f;

        safeAreaOffset = safeAreaOffset / 2;
        float scaleRatio = UIRoot.Instance.mRootCanvas.GetComponent<CanvasScaler>().referenceResolution.y / Screen.height;
        safeAreaOffset = (int)(safeAreaOffset * scaleRatio);
        safeAreaOffset += 30;
        return -safeAreaOffset;
    }
    public static string FirstCharToUpper(string str)
    {
        char[] a = str.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);
    }
    public static T GetOrCreateComponent<T>(GameObject owner) where T : MonoBehaviour
    {
        T targetT = owner.GetComponent<T>();
        if (!targetT) targetT = owner.AddComponent<T>();

        return targetT;
    }
    /// <summary>
    /// 删除一个Transform的全部子节点
    /// </summary>
    /// <param name="rootTransform"></param>
    public static void DestroyAllChildren(Transform rootTransform)
    {
        rootTransform?.gameObject.RemoveAllChildren();
    }
    public static void RemoveAllChildren(this GameObject _this)
    {
        foreach (Transform trans in _this.transform)
        {
            UnityEngine.Object.Destroy(trans.gameObject);
        }
    }
    public static bool IsLE_16_10()
    {
        float maxR = Mathf.Max(Screen.width, Screen.height);
        float minR = Mathf.Min(Screen.width, Screen.height);
        var ratio = (maxR / minR) <= 1.605f;
        return ratio;
    }
    // 判断宽屏设备
    public static bool IsWideScreenDevice()
    {
        return ((float) Screen.width / Screen.height <= 1.5f);
    }
    public static bool IsTouchUGUI()
    {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
        if (Input.touchCount > 0 ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) : EventSystem.current.IsPointerOverGameObject())
#else
        if (EventSystem.current.IsPointerOverGameObject())
#endif
            return true;
        else
            return false;
    }
}