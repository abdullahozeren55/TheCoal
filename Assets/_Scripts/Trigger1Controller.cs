using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trigger1Controller : MonoBehaviour
{
    [SerializeField] private float timeToDestroyAfterTrigger = 5f;
    [SerializeField] private float timeToSetGolemFree;
    [SerializeField] private Golem golem;
    [SerializeField] private GameObject bigParticle;
    [SerializeField] private Transform[] bigParticleInstantiateTransforms;
    [SerializeField] private GameObject golemHoldingPlatform;
    [SerializeField] private PlayableDirector cutscene;
    private Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
        {
            player = other.GetComponent<Player>();
            player.shouldLandFreeze = true;
            player.shouldCheckInputs = false;
            if(player.Movement.FacingDirection == -1)
            {
                player.Movement.Flip();
            }
            cutscene.Play();
            Invoke("SetGolemFree", timeToSetGolemFree);
            Destroy(gameObject, timeToDestroyAfterTrigger);
        }
    }

    void OnDestroy()
    {
        player.shouldLandFreeze = false;
        player.shouldCheckInputs = true;
        golem.shouldFreeze = false;
    }

    void SetGolemFree()
    {
        Destroy(golemHoldingPlatform);
        foreach (Transform point in bigParticleInstantiateTransforms)
        {
            Instantiate(bigParticle, point.position, Quaternion.identity);
        }
    }
}
