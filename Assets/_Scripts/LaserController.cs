using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField] private float hpDamageAmount = 100f;
    [SerializeField] private string enemyTagName;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag(enemyTagName))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.Damage(hpDamageAmount);
            }
        }
        
    }
}
