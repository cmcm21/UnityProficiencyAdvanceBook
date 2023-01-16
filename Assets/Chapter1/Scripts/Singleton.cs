using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    protected static T Instance
    {
        get => instance;
        set => instance = value;
    }

    private static bool _quitting = false;

    protected static void Init()
    {
        
    }

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnDestroy()
    {
        if (!_quitting)
        {
            instance = null;
            Init();
        }
    }
}
