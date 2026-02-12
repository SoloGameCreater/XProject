using UnityEngine;

/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly object m_Lock = new object();
    private static T m_Instance;
    // 应用退出阶段禁止再创建单例，避免 OnDestroy 链路反向拉起新对象。
    private static bool m_IsApplicationQuitting;
    // 记录单例被销毁的帧，阻止同一帧内被其它 OnDestroy 再次创建。
    private static int m_LastDestroyFrame = -1;

    static Manager()
    {
        Application.quitting += OnApplicationQuitting;
    }

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
            // 关闭 Domain Reload 时，静态字段会跨 Play Session 保留，这里在新 Session 首帧自动复位。
            if (m_IsApplicationQuitting && Time.frameCount == 0)
            {
                m_IsApplicationQuitting = false;
            }

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

            if (m_IsApplicationQuitting || m_LastDestroyFrame == Time.frameCount)
            {
                instance = null;
                return false;
            }

            // Search for existing instance.
            m_Instance = (T)FindObjectOfType(typeof(T));

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

    private static void OnApplicationQuitting()
    {
        m_IsApplicationQuitting = true;
    }

    private static GameObject CreateSingletonObject()
    {
        var singletonObject = new GameObject();

        return singletonObject;
    }

    // 这个方法如果override，会在Instance创建完立刻调用, 派生类可以用来默认初始化一些东西
    protected virtual void InitImmediately()
    {
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
        {
            m_Instance = null;
            m_LastDestroyFrame = Time.frameCount;
        }
    }
}
