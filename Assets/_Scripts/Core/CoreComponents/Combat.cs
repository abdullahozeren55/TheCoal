using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{

    [SerializeField] private GameObject damageParticles;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>(); }
    private CollisionSenses collisionSenses;
    private Movement movement;
    private Stats Stats => stats ??= core.GetCoreComponent<Stats>();
    private Stats stats;
    private ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>(); 
    private ParticleManager particleManager;

    [SerializeField] private float maxKnockbackTime = 0.2f;
    private bool isKnockbackActive;
    private float knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }
    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged!");
        Stats?.DecreaseHealth(amount);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        Movement.CanSetVelocity = false;
        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if(isKnockbackActive && ((Movement?.CurrentVelocity.y <= 0.01f && CollisionSenses.Ground) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            Movement.CanSetVelocity = true;
        }
    }
}
