using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Trigger0Controller : MonoBehaviour
{
    [SerializeField] private GameObject platformParticlesSmall;
    [SerializeField] private Transform instantiatePoint;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
        {
            Instantiate(platformParticlesSmall, instantiatePoint.position, Quaternion.identity);
            impulseSource.GenerateImpulse();
            Destroy(gameObject, 0.1f);
        }
    }
}
