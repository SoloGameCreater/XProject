using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this property.
    /// </summary>
    public static T Instance
    {
        get
        {
            lock (m_Lock)
            {
                if (m_Instance != null) return m_Instance;

#if UNITY_EDITOR
                if (m_ShuttingDown)
                {
                    // Logging or handling for editor shutdown can be done here if needed.
                }
#endif

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

                return m_Instance;
            }
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

    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }

    protected virtual void OnDestroy()
    {
        m_ShuttingDown = true;
        if (m_Instance == this)
        {
            m_Instance = null;
        }
    }
}