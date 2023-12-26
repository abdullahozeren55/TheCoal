using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat : Entity, IDamageable, IKnockbackable, IStunable
{
    public NewRat_IdleState IdleState { get; private set; }
    public NewRat_MoveState MoveState { get; private set; }
    public NewRat_ChargeState ChargeState { get; private set; }
    public NewRat_LookForPlayerState LookForPlayerState { get; private set; }
    public NewRat_JumpAttackState JumpAttackState { get; private set; }
    public NewRat_NormalAttackState NormalAttackState { get; private set; }
    public NewRat_HitState HitState { get; private set; }
    public NewRat_StunState StunState { get; private set; }

    [SerializeField] private Transform meleeAttackPosition;

    public bool IsStunned { get; private set; }

    private float lastPoiseRegTime;
    private float lastDamageTakenTime;
    [HideInInspector] public float lastKnockbackedTime;
    private float poiseRegAmount;
    private bool shouldRegenPoise;
    public float knockBackMultiplier = 1f;

    public override void Awake()
    {
        base.Awake();

        IdleState = new NewRat_IdleState(this, stateMachine, "idle", entityData, this);
        MoveState = new NewRat_MoveState(this, stateMachine, "move", entityData, this);
        ChargeState = new NewRat_ChargeState(this, stateMachine, "charge", entityData, this);
        LookForPlayerState = new NewRat_LookForPlayerState(this, stateMachine, "idle", entityData, this);
        JumpAttackState = new NewRat_JumpAttackState(this, stateMachine, "jumpAttack", entityData, meleeAttackPosition, this);
        NormalAttackState = new NewRat_NormalAttackState(this, stateMachine, "normalAttack", entityData, meleeAttackPosition, this);
        HitState = new NewRat_HitState(this, stateMachine, "hit", entityData, this);
        StunState = new NewRat_StunState(this, stateMachine, "stun", entityData, this);

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
        Movement?.SetVelocity(strength * knockBackMultiplier, angle, direction);
        lastKnockbackedTime = Time.time;
        if(stateMachine.currentState == JumpAttackState || stateMachine.currentState == NormalAttackState)
        {
            gotBackAttacked = false;
        }
        else
        {
            gotBackAttacked = !CheckPlayerInMaxAgroRange();
        }
        
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
        CameraShakeManager.instance.ScreenShakeFromProfile(screenShakeProfile, impulseSource);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
        ParticleManager?.StartParticlesWithRandomRotation(hitParticles);
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
