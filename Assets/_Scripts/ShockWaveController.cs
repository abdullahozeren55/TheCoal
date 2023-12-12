using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveController : MonoBehaviour
{
    [SerializeField] private float shockWaveTime = 0.75f;

    private float elapsedTime;
    private float lerpedAmount;

    private Material mat;

    private static int waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.SetFloat(waveDistanceFromCenter, -0.1f);
        lerpedAmount = 0f;
        elapsedTime = 0f;
    }

    void Update()
    {
        if(elapsedTime <= shockWaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(-0.1f, 0.75f, (elapsedTime/shockWaveTime));
            mat.SetFloat(waveDistanceFromCenter, lerpedAmount);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
