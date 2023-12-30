using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    public static CoroutineRunner instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
