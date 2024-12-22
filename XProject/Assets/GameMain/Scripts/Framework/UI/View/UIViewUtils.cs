using System;
using UnityEngine;
using UnityEngine.UI;

public static class UIViewUtils
{
    public static GameObject BindEvent(this UIView view, string target, Action action = null, bool playAudio = true)
    {
        return view.BindEvent(target, null, (obj) => action?.Invoke(), playAudio);
    }

    public static GameObject BindEvent(this UIView view, string target, GameObject par = null, Action<GameObject> action = null, bool playAudio = true)
    {
        if (!par) par = view.gameObject;

        var obj = par.transform.Find(target).gameObject;
        if (obj != null)
        {
            var button = obj.GetComponent<Button>();
            if (button)
            {
                button.onClick.AddListener
                (
                    delegate()
                    {
                        if (playAudio)
                        {
                            //AudioSysManager.Instance.PlaySound(SfxNameConst.button_s);
                        }

                        action?.Invoke(obj);
                    }
                );
            }
        }
        else
        {
            Debug.LogError($"未找到{view.gameObject.name}/{target}");
        }
        return obj;
    }
    
    public static GameObject FindObj(this UIView view, string path, GameObject par = null)
    {
        if (!par) par = view.gameObject;
        var obj = par.transform.Find(path).gameObject;
        return obj;
    }
    
    public static GameObject GetItem(this UIView view, string key, GameObject parObj = null)
    {
        if (parObj == null) parObj = view.gameObject;
        var obj = view.FindObj(key, parObj);
        if (obj == null) Debug.LogError($"GetItem failed, window controller name : {view.GetType()},  key = {key}");
        return obj;
    }

    public static T GetItem<T>(this UIView view, string key, GameObject parObj = null)
    {
        var go = view.GetItem(key, parObj);
        return view.GetItem<T>(go);
    }

    public static T GetItem<T>(this UIView view, GameObject go)
    {
        if (go != null)
        {
            var com = go.GetComponent<T>();
            if (com == null)  Debug.LogError($"GetItem failed, window controller name : {view.GetType()},  game object name = {go.name}, Component type:{typeof(T)}");
            return com;
        }
        return default(T);
    }
}