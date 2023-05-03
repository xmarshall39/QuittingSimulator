using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePointLighting : MonoBehaviour
{
    public Light KeyLight, FillLight, BackLight;
    public float baseIntensity = 5f;

#if UNITY_EDITOR
    void OnValidate()
    {
        KeyLight.intensity = baseIntensity;
        FillLight.intensity = baseIntensity * 0.5f;
        BackLight.intensity = baseIntensity * 0.2f;
    }
#endif
}
