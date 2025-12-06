using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this property.
    /// </summary>
    public static T Instance
    {
        get
        {
            TryGetInstance(out var instance);
            return instance;
        }
    }

    public static bool TryGetInstance(out T instance, bool createWhenMissing = true)
    {
        lock (m_Lock)
        {
            if (m_Instance != null)
            {
                instance = m_Instance;
                return true;
            }

            if (!createWhenMissing)
            {
                instance = null;
                return false;
            }

#if !BAN_FINDOBJECTOFTYPE && !REPLACE_FINDOBJECTOFTYPE
            // Search for existing instance.
            m_Instance = (T)FindObjectOfType(typeof(T));
#endif

            // Create new instance if one doesn't already exist.
            if (m_Instance == null)
            {
                var singletonObject = CreateSingletonObject();
                m_Instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T) + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
                (m_Instance as Manager<T>).InitImmediately();
            }

            instance = m_Instance;
            return instance != null;
        }
    }

    private static GameObject CreateSingletonObject()
    {
        var singletonObject = new GameObject();

#if NORMALIZE_SINGLE_PARENT && UNITY_EDITOR
    var root = GameObject.Find("SINGLETON_ROOT") ?? new GameObject("SINGLETON_ROOT");
    DontDestroyOnLoad(root);
    singletonObject.transform.SetParent(root.transform);
#endif

        return singletonObject;
    }
    
#if REPLACE_FINDOBJECTOFTYPE
    public Manager()
    {
        m_Instance = this as T;
    }
#endif

    // 这个方法如果override，会在Instance创建完立刻调用, 派生类可以用来默认初始化一些东西
    protected virtual void InitImmediately()
    {
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
        {
            m_Instance = null;
        }
    }
}
