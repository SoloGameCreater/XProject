/*
 * UI生命周期
 * Open
 *     OnViewOpen
 * Close
 *     OnViewClose[对象销毁前]
 *     OnViewDestroy[对象销毁后]
 *
 * 备注：
 * 1.同一个Main-View类型目前不允许多开(目的：减少灵活度，降低出异常的机率)。
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.UI;

public class UIViewSystem : GlobalSystem<UIViewSystem>, IUpdatable
{
    private Dictionary<UIViewLayer, List<UIView>> windows = new Dictionary<UIViewLayer, List<UIView>>();

    public void Update(float deltaTime)
    {
        foreach (var vecs in windows)
        {
            foreach (var p in vecs.Value)
            {
                p.OnViewUpdate(deltaTime);
            }
        }
    }

    public void Open<T>(UIViewParam param = null) where T : UIView
    {
        OpenInternal<T>(null, param);
    }

    //等到最后一帧再打开界面，规避UGUI图集延迟绑定引起的白图、花图问题
    public void OpenLastFrame<T>(UIViewParam param = null) where T : UIView
    {
        CoroutineManager.Instance.StartCoroutine(_OpenLastFrame<T>(param));
    }

    private IEnumerator _OpenLastFrame<T>(UIViewParam param = null) where T : UIView
    {
        UIRoot.Instance.EnableTouch(false);
        yield return new WaitForEndOfFrame();
        UIRoot.Instance.EnableTouch(true);
        OpenInternal<T>(null, param);
    }

    public void Open<T>(string address, UIViewParam param = null) where T : UIView
    {
        OpenInternal<T>(address, param);
    }

    public void Close<T>() where T : UIView
    {
        UIView window = Get<T>();
        if (window != null)
        {
            windows[window.ViewLayer].Remove(window);
            OnClose(window);
        }
    }

    public T Get<T>() where T : UIView
    {
        UIView window = null;
        foreach (var d in windows)
        {
            foreach (var v in d.Value)
            {
                if (v.GetType() == typeof(T))
                {
                    window = v;
                }
            }
        }

        return (T)window;
    }

    public void Close(Type type)
    {
        foreach (var d in windows)
        {
            foreach (var v in d.Value)
            {
                if (v.GetType() == type)
                {
                    var window = v;
                    windows[window.ViewLayer].Remove(window);
                    OnClose(window);
                    return;
                }
            }
        }
    }

    public void CloseAll(params int[] excludeLayer)
    {
        //逆向关闭
        for (int i = (int)UIViewLayer.Max - 1; i > (int)UIViewLayer.None; i--)
        {
            bool exclude = false;
            for (int j = 0; j < excludeLayer.Length; j++)
            {
                if (excludeLayer[j] == i)
                {
                    exclude = true;
                    break;
                }
            }

            if (exclude) continue;
            if (windows.TryGetValue((UIViewLayer)i, out var layerWindows) && layerWindows != null)
            {
                List<UIView> tempList = new List<UIView>();
                foreach (var p in layerWindows) tempList.Add(p);
                layerWindows.Clear();
                for (int j = tempList.Count - 1; j >= 0; j--)
                {
                    OnClose(tempList[j]);
                }
            }
        }
    }

    public async void OnClose(UIView view)
    {
        await view.OnViewClose();
        DestoryView(view.gameObject);
        view.OnViewDestroy();
    }

    private void OpenInternal<T>(string address, UIViewParam param) where T : UIView
    {
        //检测是否已经存在
        if (null != Get<T>())
        {
            Debug.LogError($"{typeof(T)}View已经打开，请检查为什么多次Open！");
            return;
        }

        //实例化
        UIView window = CreateMainView<T>(address);
        if (!windows.TryGetValue(window.ViewLayer, out var tempList))
        {
            tempList = new List<UIView>();
            tempList.Add(window);
            windows.Add(window.ViewLayer, tempList);
        }
        else
        {
            tempList.Add(window);
        }

        //层级处理
        SortLayer();

        window.OnViewOpen(param);
    }

    private T CreateMainView<T>(string address) where T : UIView
    {
        var subPath = string.IsNullOrEmpty(address) ? TryGetAssetAddressFromAttribute(typeof(T)) : address;
        if (string.IsNullOrEmpty(subPath)) return null;
        GameObject prefab = ResourcesManager.Instance.LoadResource<GameObject>(subPath.StartsWith("Activity/") ? $"{subPath}" : $"Prefabs/UI/{subPath}");
        if (!prefab) return null;
        GameObject obj = GameObject.Instantiate(prefab, UIRoot.Instance.mRoot.transform);
        //Main-View需要保证有Canvas和GraphicRaycaster
        {
            var canvas = obj.GetComponent<Canvas>();
            if (!canvas) canvas = obj.AddComponent<Canvas>();
            CommonUtils.GetOrCreateComponent<GraphicRaycaster>(obj);
            canvas.overrideSorting = true;
        }
        var view = Activator.CreateInstance(typeof(T)) as T;
        view.SetupView(null, obj);
        return view;
    }

    private void DestoryView(GameObject window)
    {
        GameObject.Destroy(window);
    }

    private string TryGetAssetAddressFromAttribute(Type type)
    {
        string address = null;

        var assetAddressAttributes = type.GetCustomAttributes(typeof(AssetAddressAttribute), false);

        if (assetAddressAttributes.Length > 0)
        {
            var assetAddressAttribute = assetAddressAttributes[0] as AssetAddressAttribute;

            if (assetAddressAttribute != null)
            {
                address = assetAddressAttribute.assetAddress;

                if (CommonUtils.IsLE_16_10())
                {
                    if (!string.IsNullOrEmpty(assetAddressAttribute.assetAddressPad))
                    {
                        address = assetAddressAttribute.assetAddressPad;
                    }
                }
            }
        }

        return address;
    }

    public void SortLayer()
    {
        int siblingIndex = 10; //因为用的是通用LayerName:"Default",所以在这里留一线设计，作为其它系统的兼容
        for (int i = (int)UIViewLayer.None + 1; i < (int)UIViewLayer.Max; i++)
        {
            if (windows.TryGetValue((UIViewLayer)i, out var layerWindows) && layerWindows != null)
            {
                if (layerWindows.Count > 0)
                {
                    foreach (var w in layerWindows)
                    {
                        int maxOrder = 0;
                        w.SetSortingOrder(siblingIndex, ref maxOrder);
                        siblingIndex = maxOrder + 1;
                    }
                }
            }
        }
    }

    public bool HasPopup()
    {
        foreach (var p in windows)
        {
            foreach (var v in p.Value)
            {
                if (v is UIPopup)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool HasAnyView(Predicate<UIView> predicate)
    {
        foreach (var p in windows)
        {
            foreach (var v in p.Value)
            {
                if (predicate == null || predicate(v)) return true;
            }
        }

        return false;
    }
}