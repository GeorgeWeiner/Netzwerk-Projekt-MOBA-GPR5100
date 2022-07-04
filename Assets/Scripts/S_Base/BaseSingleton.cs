using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
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
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }

}
