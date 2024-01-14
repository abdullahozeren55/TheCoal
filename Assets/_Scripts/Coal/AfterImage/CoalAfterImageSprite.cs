using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalAfterImageSprite : MonoBehaviour
{

    private float activeTime = 0.6f;
    private float timeActivated;
    private float alpha;
    private float alphaSet = 0.8f; //the alpha when it first enabled
    private float alphaMultiplier = 0.9f; //multiply alpha with this to decrease it over time
    private Transform player;

    private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Coal").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if(Time.time >= (timeActivated + activeTime))
        {
            CoalAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
