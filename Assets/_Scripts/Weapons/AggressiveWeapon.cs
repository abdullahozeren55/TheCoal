using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

public class AggressiveWeapon : Weapon
{

    protected SO_AggressiveWeaponData aggressiveWeaponData;
    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();
    private List<IStunable> detectedStunables = new List<IStunable>();
    private List<Vector2> contactPoints = new List<Vector2>();

    private bool doneInstantiate;

    protected override void Awake()
    {
        base.Awake();

        doneInstantiate = false;

        if(weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon!");
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    private void CheckMeleeAttack()
    {

        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];
        WeaponScreenShakeDetails shakeDetails = aggressiveWeaponData.ScreenShakeDetails[attackCounter];

        foreach (Vector2 item in contactPoints.ToList())
        {
            if (!doneInstantiate)
            {
                state.ParticleManager.StartParticlesWithCertainPosRandomRot(weaponData.hitParticle, item);
                doneInstantiate = true;
            }

            contactPoints.Remove(item);
        }

        doneInstantiate = false;

        foreach (IDamageable item in detectedDamageables.ToList())
        {
            ShakeScreen(shakeDetails, impulseSource);
            item.Damage(details.damageAmount);
        }
        foreach (IKnockbackable item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackStrength, details.knockbackAngle, movement.FacingDirection);
        }
        foreach (IStunable item in detectedStunables.ToList())
        {
            item.PoiseDamage(details.poiseDamageAmount);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        Vector2 contactPoint = collision.bounds.ClosestPoint(transform.position);
        
        if(contactPoint != null)
        {      
            contactPoints.Add(contactPoint);
        }


        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            detectedDamageables.Add(damageable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

        if(knockbackable != null)
        {
            detectedKnockbackables.Add(knockbackable);
        }

        IStunable stunable = collision.GetComponent<IStunable>();

        if(stunable != null)
        {
            detectedStunables.Add(stunable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        Vector2 contactPoint = collision.bounds.ClosestPoint(transform.position);
        
        if(contactPoint != null)
        {      
            contactPoints.Remove(contactPoint);
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            detectedDamageables.Remove(damageable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

        if(knockbackable != null)
        {
            detectedKnockbackables.Remove(knockbackable);
        }

        IStunable stunable = collision.GetComponent<IStunable>();

        if(stunable != null)
        {
            detectedStunables.Remove(stunable);
        }
    }

    private void ShakeScreen(WeaponScreenShakeDetails screenShakeDetails, CinemachineImpulseSource impulseSource)
    {
        CameraShakeManager.instance.ScreenShakeFromWeapon(screenShakeDetails, impulseSource);
    }
}
