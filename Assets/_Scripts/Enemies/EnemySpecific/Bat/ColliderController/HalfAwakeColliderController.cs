using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfAwakeColliderController : MonoBehaviour
{
    private Bat bat;

    void Start()
    {
        bat = GetComponentInParent<Bat>();
    }

    void Update()
    {
        if(bat.shouldWakeUp)
        {
            Destroy(gameObject, 0.1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bat.shouldHalfAwake = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bat.shouldHalfAwake = false;
        }
    }
}
