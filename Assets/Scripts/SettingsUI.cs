using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class SettingsUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(var field in typeof(Settings).GetFields())
        {
            if (field.FieldType == typeof(int))
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
