using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity, IKnockbackable, IDamageable, IStunable
{
    public Rat_IdleState IdleState { get; private set; }
    public Rat_MoveState MoveState { get; private set; }

    public Rat_PlayerDetectedState PlayerDetectedState { get; private set; }

    public Rat_ChargeState ChargeState { get; private set; }

    public Rat_MeleeAttackState MeleeAttackState { get; private set; }

    public Rat_HitState HitState { get; private set; }

    public Rat_StunState StunState { get; private set; }

    public Rat_LookForPlayerState LookForPlayerState { get; private set; }

    [SerializeField] private Transform meleeAttackPosition;

    public bool IsStunned { get; private set; }

    private float lastPoiseRegTime;
    private float lastDamageTakenTime;
    public float lastKnockbackedTime;
    private float poiseRegAmount;
    private bool shouldRegenPoise;

    public override void Awake()
    {
        base.Awake();

        IdleState = new Rat_IdleState(this, stateMachine, "idle", entityData, this);
        MoveState = new Rat_MoveState(this, stateMachine, "move", entityData, this);
        PlayerDetectedState = new Rat_PlayerDetectedState(this, stateMachine, "playerDetected", entityData, this);
        ChargeState = new Rat_ChargeState(this, stateMachine, "charge", entityData, this);
        MeleeAttackState = new Rat_MeleeAttackState(this, stateMachine, "meleeAttack", entityData, meleeAttackPosition, this);
        HitState = new Rat_HitState(this, stateMachine, "hit", entityData, this);
        StunState = new Rat_StunState(this, stateMachine, "stun", entityData, this);
        LookForPlayerState = new Rat_LookForPlayerState(this, stateMachine, "lookForPlayer", entityData, this);

    }

    private void Start()
    {
        stateMachine.Initialize(MoveState);

    }

    public override void Update()
    {
        base.Update();

        if(shouldRegenPoise && Time.time >= lastPoiseRegTime + entityData.poiseRegCooldown && Time.time >= lastDamageTakenTime + entityData.poiseRegStartTimeAfterDamaged)
        {
            SetRandomPoiseRegAmount();
            PoiseRegen(poiseRegAmount);
        }
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
        lastKnockbackedTime = Time.time;
        if(IsStunned && stateMachine.currentState != StunState)
        {
            stateMachine.ChangeState(StunState);
        }
        else if(stateMachine.currentState != StunState && stateMachine.currentState != HitState)
        {
            stateMachine.ChangeState(HitState);
        }
    }

    public void Damage(float amount)
    {
        Stats?.DecreaseHealth(amount);
        lastDamageTakenTime = Time.time;
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void PoiseDamage(float amount)
    {
        Stats?.DecreasePoise(amount);
        IsStunned = Stats.Stun();
        shouldRegenPoise = true;
    }

    public void PoiseRegen(float amount)
    {
        Stats?.IncreasePoise(amount);
        lastPoiseRegTime = Time.time;
        IsStunned = Stats.Stun();
        shouldRegenPoise = Stats.ShouldRegenPoise();
    }

    private void SetRandomPoiseRegAmount()
    {
        poiseRegAmount = Random.Range(entityData.minPoiseRegAmount, entityData.maxPoiseRegAmount);
    }
}
