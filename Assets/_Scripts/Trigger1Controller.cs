using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger1Controller : MonoBehaviour
{
    [SerializeField] private float timeToDestroyAfterTrigger = 5f;
    [SerializeField] private float timeToSetGolemFree;
    [SerializeField] private Golem golem;
    [SerializeField] private GameObject bigParticle;
    [SerializeField] private Transform bigParticleInstantiateTransform;
    [SerializeField] private GameObject golemHoldingPlatform;
    private Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
        {
            player = other.GetComponent<Player>();
            player.shouldLandFreeze = true;
            Invoke("SetGolemFree", timeToSetGolemFree);
            Destroy(gameObject, timeToDestroyAfterTrigger);
        }
    }

    void OnDestroy()
    {
        player.shouldLandFreeze = false;
        golem.shouldFreeze = false;
    }

    void SetGolemFree()
    {
        Destroy(golemHoldingPlatform);
        Instantiate(bigParticle, bigParticleInstantiateTransform.position, Quaternion.identity);
    }
}
