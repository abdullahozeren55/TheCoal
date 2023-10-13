using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coal_Eye_Blink : MonoBehaviour
{
    private bool canBlink;

    private SpriteRenderer SR;


    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        InvokeRepeating("BlinkEye", 0f, 3.1f);
    }

    void BlinkEye()
    {
        SR.sortingOrder = 1;
        Invoke("EyeClosed", 3.0f);
    }

    void EyeClosed()
    {
        SR.sortingOrder = -1;
    }
}
