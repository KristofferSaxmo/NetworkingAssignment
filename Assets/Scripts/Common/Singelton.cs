using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                // If the instance is still null, create a new GameObject and add the component
                GameObject singletonObject = new GameObject(typeof(T).Name);
                instance = singletonObject.AddComponent<T>();
                Debug.Log("Creating Instance");
            }

            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != FindObjectOfType<T>())
        {
            // If there is another instance already existing, destroy the new one
            Destroy(FindObjectOfType<T>());
        }

        return instance;
    }
}
