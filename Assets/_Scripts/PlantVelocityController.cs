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

    public void InfluencePlant(Material mat, float XVelocity)
    {
        mat.SetFloat(externalInfluence, XVelocity);
    }
}
