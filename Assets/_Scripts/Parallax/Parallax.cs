using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject cameraThatFollowsObject;
    public float parallaxEffect;
    private float distance;
    private float temp;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {

        temp = cameraThatFollowsObject.transform.position.x * (1 - parallaxEffect);

        distance = cameraThatFollowsObject.transform.position.x * (parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);


        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;

        

        
    }
}
