using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Inherit from this base class to create a singleton.
/// e.g. public class MyClassName : Singleton<MyClassName> {}
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
#pragma warning disable RECS0108 // Warns about static fields in generic types
    private static object m_Lock = new object();
#pragma warning restore RECS0108 // Warns about static fields in generic types
    private static T m_Instance;

    /// <summary>
    /// Access singleton instance through this propriety.
    /// </summary>
    public static T Instance
    {
        get
        {

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Search for existing instance.
                    m_Instance = (T)FindObjectOfType(typeof(T));
                    // Create new instance if one doesn't already exist.
                    if (m_Instance == null)
                    {
                        // Need to create a new GameObject to attach the singleton to.
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        //DontDestroyOnLoad(singletonObject);
                    }
                    else
                    {
                        //DontDestroyOnLoad(m_Instance.gameObject);
                    }
                }

                return m_Instance;
            }
        }
    }

    /// <summary>
    /// ddol and Maintain uniqueness
    /// </summary>
    virtual protected void DDOL()
    {
        // Prevent desturction of this object while also preventing multiple instances
        if (IsDuplicate())
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// Checks if there are multiple instances of this class
    /// </summary>
    /// <returns></returns>
    private bool IsDuplicate()
    {
        return FindObjectsOfType(GetType()).Length > 1;
    }
}
