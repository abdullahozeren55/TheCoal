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
    private float startingXVelocity;
    private float velocityLastFrame;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
        plantVelocityController = GetComponentInParent<PlantVelocityController>();
        material = GetComponent<SpriteRenderer>().material;
        startingXVelocity = material.GetFloat(externalInfluence);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == player)
        {
            if(!easeInCoroutineRunning && Mathf.Abs(playerRB.velocityX) > Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(playerRB.velocityX * plantVelocityController.ExternalInfluenceStrength));
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject == player)
        {

            if(Mathf.Abs(velocityLastFrame) > Mathf.Abs(plantVelocityController.VelocityThreshold) &&
               Mathf.Abs(playerRB.velocityX) < Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseOut());
            }
            else if(Mathf.Abs(velocityLastFrame) < Mathf.Abs(plantVelocityController.VelocityThreshold) &&
                    Mathf.Abs(playerRB.velocityX) > Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                StartCoroutine(EaseIn(playerRB.velocityX * plantVelocityController.ExternalInfluenceStrength));
            }
            else if(!easeInCoroutineRunning && !easeOutCoroutineRunning && 
                    Mathf.Abs(playerRB.velocityX) > Mathf.Abs(plantVelocityController.VelocityThreshold))
            {
                plantVelocityController.InfluencePlant(material, playerRB.velocityX * plantVelocityController.ExternalInfluenceStrength);
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
