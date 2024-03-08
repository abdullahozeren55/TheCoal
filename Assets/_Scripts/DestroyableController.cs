using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DestroyableController : MonoBehaviour, IDestroyable
{
    [SerializeField] private WeaponScreenShakeDetails shakeDetails;
    [SerializeField] private GameObject gameObjectToInstantiate;
    [SerializeField] private GameObject deathParticles;
    private Movement playerMovement;
    private CinemachineImpulseSource impulseSource;
    private Transform particleContainer;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Movement>();
    }
    public void Destroy()
    {
        if(impulseSource != null)
        {
            CameraShakeManager.instance.ScreenShakeFromWeapon(shakeDetails, impulseSource);
        }
        if(deathParticles != null)
        {
            if(playerMovement.FacingDirection == 1)
            {
                Instantiate(deathParticles, transform.position, Quaternion.identity, particleContainer);
            }
            else
            {
                Instantiate(deathParticles, transform.position, Quaternion.Euler(0f, 180f, 0f), particleContainer);
            }
        }
        if(gameObjectToInstantiate != null)
        {
            Quaternion rotation = transform.rotation;
            Instantiate(gameObjectToInstantiate, transform.position, rotation);
        }
        Destroy(gameObject);
    }
}
