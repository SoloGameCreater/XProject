using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BehaviourEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Dictionary<string, System.Action> events = new Dictionary<string, System.Action>();

    public void RegisterEvent(string name, System.Action handle)
    {
        events[name] = handle;
    }

    protected void Notify(string name)
    {
        System.Action handle;
        if (events.TryGetValue(name, out handle))
        {
            handle();
        }
    }

    protected virtual void Awake()
    {
        Notify("Awake");
    }

    protected virtual void Start()
    {
        Notify("Start");
    }

    protected virtual void OnEnable()
    {
        Notify("OnEnable");
    }

    protected virtual void OnDisable()
    {
        Notify("OnDisable");
    }

    protected virtual void OnDestroy()
    {
        Notify("OnDestroy");
    }

    protected virtual void OnTransformParentChanged()
    {
        Notify("OnTransformParentChanged");
    }

    protected virtual void OnTransformChildrenChanged()
    {
        Notify("OnTransformChildrenChanged");
    }

    protected virtual void OnRectTransformDimensionsChange()
    {
        Notify("OnRectTransformDimensionsChange");
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Notify("OnPointerEnter");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        Notify("OnPointerExit");
    }
}
