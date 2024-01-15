using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_HeadColliderController : MonoBehaviour
{
    private Golem golemScript;

    private void Awake()
    {
        golemScript = GetComponentInParent<Golem>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("DashingPlayer"))
        {
            golemScript.isPlayerOnHead = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("DashingPlayer"))
        {
            golemScript.isPlayerOnHead = false;
        }    
    }
}
