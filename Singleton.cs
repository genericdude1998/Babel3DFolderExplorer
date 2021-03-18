﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{
    private static T m_Instance;

    public static T Instance 
    {
        get 
        {
            if (m_Instance == null) { m_Instance = FindObjectOfType<T>(); }
            if (m_Instance == null) 
            { 
                GameObject obj = new GameObject();
                Debug.Log("Generating a new singleton of " + typeof(T).Name);
                obj.name = typeof(T).Name;
                m_Instance = obj.AddComponent<T>();
            }
            return m_Instance;
        } 
    }

    public virtual void Awake() 
    {
        if (m_Instance == null) { m_Instance = this as T; }
        else { Destroy(gameObject); }
    }
}
