using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteSingleton : Singleton<ConcreteSingleton>
{
    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        if (Instance == null || Instance.Equals(null))
        {
            var newGameObject = new GameObject();
            Instance = newGameObject.AddComponent<ConcreteSingleton>();
            newGameObject.hideFlags = HideFlags.HideAndDontSave;            
            
            Debug.Log("Singleton was initialized, and is ready to be used");
            DontDestroyOnLoad(newGameObject);
        }
    }
}
