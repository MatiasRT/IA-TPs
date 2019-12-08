﻿using UnityEngine;

public class MBSingleton<T> : MonoBehaviour where T : MBSingleton<T>
{
    private static MBSingleton<T> instance = null;

    public static T Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MBSingleton<T>>();
            }

            return (T)instance;
        }
    }

    public static bool IsAvailable()
    {
        return instance != null;
    }

    protected virtual void Initialize()
    {

    }

    protected virtual void Awake()
    {
        instance = this;

        Initialize();
    }
}