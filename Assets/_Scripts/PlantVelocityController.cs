using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantVelocityController : MonoBehaviour
{
    [Range(0f, 1f)] public float ExternalInfluenceStrength = 0.25f;
    public float EaseInTime = 0.15f;
    public float EaseOutTime = 0.15f;
    public float VelocityThreshold = 5f;

    private int externalInfluence = Shader.PropertyToID("_ExternalInfluence");

    [Header("Material Properties")]
    public float windIntensityValue = 0.5f;
    public float windScaleValue = 0.5f;
    public float windSpeedValue = 1f;
    [HideInInspector] public int windIntensity = Shader.PropertyToID("_WindIntensity");
    [HideInInspector] public int windScale = Shader.PropertyToID("_WindScale");
    [HideInInspector] public int windSpeed = Shader.PropertyToID("_WindSpeed");

    public void InfluencePlant(Material mat, float XVelocity)
    {

        mat.SetFloat(externalInfluence, XVelocity);
        
    }
}
