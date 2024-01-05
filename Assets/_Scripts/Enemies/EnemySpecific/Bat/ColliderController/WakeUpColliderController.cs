using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakeUpColliderController : MonoBehaviour
{

    private Bat bat;

    void Start()
    {
        bat = GetComponentInParent<Bat>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bat.shouldWakeUp = true;
            Destroy(gameObject, 0.1f);
        }
    }
}
