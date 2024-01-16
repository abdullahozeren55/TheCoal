using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private bool shouldDamageEveryone;
    [SerializeField] private bool shouldDisableParent;
    [SerializeField] private string enemyTagName;
    [SerializeField] private float hpDamageAmount = 10f;
    [SerializeField] private float knockbackStrength = 15f;
    [SerializeField] private Vector2 knockbackAngle;
    void OnTriggerEnter2D(Collider2D other)
    {

        if(shouldDamageEveryone)
        {
            if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
            {
                other.GetComponent<Player>().JumpState.ResetAmountOfJumpsLeft();
                other.GetComponent<Player>().JumpState.DecreaseAmountOfJumpsLeft();
            }

            if(!other.CompareTag("DashingPlayer"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();

                if(damageable != null)
                {
                    damageable.Damage(hpDamageAmount);
                }
            }

            IKnockbackable knockbackable = other.GetComponent<IKnockbackable>();

            if(knockbackable != null)
            {
                int facingDirection = other.GetComponentInChildren<Movement>().FacingDirection;
                knockbackable.Knockback(knockbackStrength, knockbackAngle, facingDirection);
            }
        }
        else
        {
            if(other.CompareTag("Player"))
            {
                other.GetComponent<Player>().JumpState.ResetAmountOfJumpsLeft();
                other.GetComponent<Player>().JumpState.DecreaseAmountOfJumpsLeft();
            }

            if(other.CompareTag("Player") || other.CompareTag(enemyTagName))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();

                if(damageable != null)
                {
                    damageable.Damage(hpDamageAmount);
                }

                IKnockbackable knockbackable = other.GetComponent<IKnockbackable>();

                if(knockbackable != null)
                {
                    int facingDirection = other.GetComponentInChildren<Movement>().FacingDirection;
                    knockbackable.Knockback(knockbackStrength, knockbackAngle, facingDirection);
                }
            }
        }

        

        

    }

    private void DisableParentObject()
    {
        if(shouldDisableParent)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
