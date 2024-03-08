using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbivalenceForeground2JumpUpTrigger : MonoBehaviour
{
    [SerializeField] private AmbivalenceForeground2Controller controller;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {
            controller.SetJumpOn();
            gameObject.SetActive(false);
        }
    }
}
