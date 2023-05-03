using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static T CopyComponent<T>(T original, GameObject destination, bool unique = false) where T : Component
    {
        System.Type type = original.GetType();
        Component copy;
        if (unique)
        {
           copy  = destination.GetComponent<T>() == null ? destination.AddComponent(type) : destination.GetComponent<T>();
        }

        else
        {
            copy = destination.AddComponent(type);
        }
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }


    private static Settings instance;
    public static Settings Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null) Destroy(this.gameObject);

        instance = this;
    }

    [Header("Explosion")]
    public float explosionForce = 10f;
    public float explosionRadius = 3f;
    public float explosionUpwardForce = 0.75f;
    [Range(0, 1)]
    public float extraKeyModifier = 0.1f;
    
}
