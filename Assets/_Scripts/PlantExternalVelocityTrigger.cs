using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantExternalVelocityTrigger : MonoBehaviour
{
    private PlantVelocityController plantVelocityController;
    private GameObject player;
    private Material material;
    private Rigidbody2D playerRB;
    private bool easeInCoroutineRunning;
    private bool easeOutCoroutineRunning;

    private int externalInfluence = Shader.PropertyToID("_ExternalInfluence");
    private float startingXVelocity = 1f;
    private float velocityLastFrame;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        plantVelocityController = GetComponentInParent<PlantVelocityController>();
        material = GetComponent<SpriteRenderer>().material;

        material.SetFloat(plantVelocityController.windIntensity, 0f);
        material.SetFloat(plantVelocityController.windScale, 0f);
        material.SetFloat(plantVelocityController.windSpeed, 0f);
    }

    void Update()
    {
        if(!easeInCoroutineRunning && !easeOutCoroutineRunning && material.GetFloat(externalInfluence) == startingXVelocity)
        {
            material.SetFloat(plantVelocityController.windIntensity, 0f);
            material.SetFloat(plantVelocityController.windScale, 0f);
            material.SetFloat(plantVelocityController.windSpeed, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            if(!easeInCoroutineRunning && !easeOutCoroutineRunning && Mathf.Abs(playerRB.velocityX) > Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(playerRB.velocityX * plantVelocityController.ExternalInfluenceStrength));
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == player)
        {

            if(!easeOutCoroutineRunning && !easeOutCoroutineRunning &&
                Mathf.Abs(velocityLastFrame) > Mathf.Abs(plantVelocityController.VelocityThreshold) &&
                Mathf.Abs(playerRB.velocityX) < Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseOut());
            }
            else if(!easeInCoroutineRunning && !easeOutCoroutineRunning &&
                    Mathf.Abs(velocityLastFrame) < Mathf.Abs(plantVelocityController.VelocityThreshold) &&
                    Mathf.Abs(playerRB.velocityX) > Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(playerRB.velocityX * plantVelocityController.ExternalInfluenceStrength));
            }

            velocityLastFrame = playerRB.velocityX;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            StartCoroutine(EaseOut());
        }
    }

    private IEnumerator EaseIn(float XVelocity)
    {
        easeInCoroutineRunning = true;

        material.SetFloat(plantVelocityController.windIntensity, plantVelocityController.windIntensityValue);
        material.SetFloat(plantVelocityController.windScale, plantVelocityController.windScaleValue);
        material.SetFloat(plantVelocityController.windSpeed, plantVelocityController.windSpeedValue);

        float elapsedTime = 0f;
        while(elapsedTime < plantVelocityController.EaseInTime)
        {
            elapsedTime += Time.deltaTime;
            
            float lerpedAmount = Mathf.Lerp(startingXVelocity, XVelocity, elapsedTime / plantVelocityController.EaseInTime);
            plantVelocityController.InfluencePlant(material, lerpedAmount);

            yield return null;
        }

        easeInCoroutineRunning = false;
    }

    private IEnumerator EaseOut()
    {
        easeOutCoroutineRunning = true;

        material.SetFloat(plantVelocityController.windIntensity, plantVelocityController.windIntensityValue);
        material.SetFloat(plantVelocityController.windScale, plantVelocityController.windScaleValue);
        material.SetFloat(plantVelocityController.windSpeed, plantVelocityController.windSpeedValue);

        float currentXInfluence = material.GetFloat(externalInfluence);

        float elapsedTime = 0f;
        while(elapsedTime < plantVelocityController.EaseOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedAmount = Mathf.Lerp(currentXInfluence, startingXVelocity, elapsedTime / plantVelocityController.EaseOutTime);
            plantVelocityController.InfluencePlant(material, lerpedAmount);

            yield return null;
        }

        easeOutCoroutineRunning = false;
    }
}
