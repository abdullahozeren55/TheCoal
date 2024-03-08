using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbivalenceTrigger : MonoBehaviour
{
    [SerializeField] private GameObject ambivalence;
    [SerializeField] private bool shouldDestroy;
    [SerializeField] private float timeToStayActive = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {
            ambivalence.SetActive(true);
            if(shouldDestroy)
            {
                Destroy(ambivalence, timeToStayActive);
            }
            gameObject.SetActive(false);
        }
    }
}
