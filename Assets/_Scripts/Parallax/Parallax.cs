using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    public GameObject mainCamera;
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

        temp = mainCamera.transform.position.x * (1 - parallaxEffect);

        distance = mainCamera.transform.position.x * (parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);


        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;

        

        
    }
}
