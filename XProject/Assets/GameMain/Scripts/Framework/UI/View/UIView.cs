using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class AssetAddressAttribute : Attribute
{
    public string assetAddress;
    public string assetAddressPad;

    public AssetAddressAttribute(string address, string assetPad = "")
    {
        assetAddress = address;
        assetAddressPad = assetPad;
    }
}
    
public abstract class UIViewParam { }

public class UIView
{
    private Canvas[] canvases;
    private int[] originSortingOrder;
    private UIView parentView;
    private List<UIView> childViews;

    public GameObject gameObject;
    public Transform transform => gameObject.transform;
    public UIView ParentView => parentView;

    public virtual UIViewLayer ViewLayer => UIViewLayer.Normal;
    
    public void SetupView(UIView parent, GameObject gameObject)
    {
        parentView = parent;
        this.gameObject = gameObject;
        childViews = new List<UIView>();
        canvases = transform.GetComponentsInChildren<Canvas>(true);
        if (canvases.Length > 0)
        {
            originSortingOrder = new int[canvases.Length];
            for (int i = 0; i < canvases.Length; i++)
            {
                originSortingOrder[i] = canvases[i].sortingOrder;
            }
        }
        
        ComponentBinder.BindingComponent(this, transform);
    }

    public void SetSortingOrder(int siblingIndex, ref int maxOrder)
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (!canvases[i])
            {
                continue;
            }
            
            canvases[i].overrideSorting = true;
            canvases[i].sortingOrder = originSortingOrder[i] + siblingIndex;
            if (canvases[i].sortingOrder > maxOrder)
            {
                maxOrder = canvases[i].sortingOrder;
            }
        }
        
        for (int i = 0; i < childViews.Count; i++)
        {
            childViews[i].SetSortingOrder(siblingIndex, ref maxOrder);
        }
    }
    
    public virtual void OnViewOpen(UIViewParam param) { }

    public virtual async Task OnViewClose()
    {
        for (int i = 0; i < childViews.Count; i++)
        {
            await childViews[i].OnViewClose();
        }
    }
    
    public virtual void OnViewDestroy()
    {
        for (int i = 0; i < childViews.Count; i++)
        {
            childViews[i].OnViewDestroy();
        }
    }

    public virtual void OnViewUpdate(float deltaTime)
    {
        for (int i = 0; i < childViews.Count; i++)
        {
            childViews[i].OnViewUpdate(deltaTime);
        }
    }

    protected Coroutine StartCoroutine(IEnumerator routine, string group = "default")
    {
        return CoroutineManager.Instance.StartCoroutine(routine, group);
    }

    protected void StopCoroutine(Coroutine routine, string group = "default")
    {
        CoroutineManager.Instance.StopCoroutine(routine, group);
    }

    protected void StopAllCoroutines(string group)
    {
        CoroutineManager.Instance.StopAllCoroutines(group);
    }
    
    protected T AddChildView<T>(GameObject childGameobject, UIViewParam param = null) where T : UIView
    {
        var view = Activator.CreateInstance(typeof(T)) as T;
        childViews.Add(view);
        view.SetupView(this, childGameobject);
        if(canvases.Length > 0) UIViewSystem.Instance.SortLayer();
        view.OnViewOpen(param);
        return view;
    }

    protected T AddChildView<T>(string prefabStr, Transform parent, UIViewParam param = null) where T: UIView
    {
        GameObject prefab = ResourcesManager.Instance.LoadResource<GameObject>(prefabStr.StartsWith("Activity/") ? $"{prefabStr}" : $"Prefabs/UI/{prefabStr}");
        GameObject obj = GameObject.Instantiate(prefab, parent);
        return AddChildView<T>(obj, param);
    }

    protected void RemoveChildView(UIView childView)
    {
        if (childViews.Remove(childView))
        {
            UIViewSystem.Instance.OnClose(childView);
        }
    }
    
    protected virtual void DoViewClose()
    {
        UIViewSystem.Instance.Close(GetType());
    }
}