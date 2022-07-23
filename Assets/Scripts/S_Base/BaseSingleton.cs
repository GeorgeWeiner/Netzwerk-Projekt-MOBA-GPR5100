using UnityEngine;

public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
