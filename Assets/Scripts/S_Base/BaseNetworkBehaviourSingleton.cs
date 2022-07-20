using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Automatically creates a singleton for networkBehaviours
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseNetworkBehaviourSingleton<T> : NetworkBehaviour where T: NetworkBehaviour
{
    [SerializeField] bool destroyOnLoad;
    static public T instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                instance = new GameObject($"[{typeof(T)}] - SingletonInstance").AddComponent<T>();
            }
        }
        else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        if (!destroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
