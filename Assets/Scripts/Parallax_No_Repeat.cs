using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_No_Repeat : MonoBehaviour
{
    private float startPos;
    public GameObject player;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
    }

    void Update()
    {
        // Parallax efekti hesaplama
        float distance = player.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }

}