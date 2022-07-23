using Mirror;
using UnityEngine;

/// <summary>
/// Automatically creates a singleton for networkBehaviours
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseNetworkBehaviourSingleton<T> : NetworkBehaviour where T: NetworkBehaviour
{
    [SerializeField] private bool destroyOnLoad;
    public static T instance;

    private void Awake()
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
