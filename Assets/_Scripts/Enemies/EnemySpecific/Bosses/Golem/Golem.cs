using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Entity, IDamageable, IStunable
{
    public Golem_FreezeState FreezeState { get; private set; }
    public Golem_IdleState IdleState { get; private set; }
    public Golem_Attack0State Attack0State { get; private set; }
    public Golem_Attack1State Attack1State { get; private set; }
    public Golem_Attack2State Attack2State { get; private set; }
    public Golem_AttackHeadState AttackHeadState { get; private set; }

    public bool IsStunned { get; private set; }

    public float flipCooldown = 0.5f;

    public GameObject attack0SpikesParent;
    public Animator[] attack0SpikesAnims;
    public GameObject headSpike;
    [HideInInspector] public Animator headSpikeAnim;
    public bool shouldFreeze;
    public GameObject groundedDustParticle;
    public Transform groundedDustPoint;

    [SerializeField] private Transform meleeAttackPosition;
    public int startFacingDirection;

    [HideInInspector] public bool isPlayerOnHead;
    

    private float lastDamageTakenTime;
    private float lastPoiseRegTime;
    private float poiseRegAmount;
    private bool shouldRegenPoise;

    public override void Awake()
    {
        base.Awake();
        attack0SpikesParent.SetActive(false);

        headSpikeAnim = headSpike.GetComponent<Animator>();
        headSpike.SetActive(false);

        

        FreezeState = new Golem_FreezeState(this, stateMachine, "idle", entityData, this);
        IdleState = new Golem_IdleState(this, stateMachine, "idle", entityData, this);
        Attack0State = new Golem_Attack0State(this, stateMachine, "attack0", entityData, meleeAttackPosition, this);
        Attack1State = new Golem_Attack1State(this, stateMachine, "attack1", entityData, meleeAttackPosition, this);
        Attack2State = new Golem_Attack2State(this, stateMachine, "attack2", entityData, meleeAttackPosition, this);
        AttackHeadState = new Golem_AttackHeadState(this, stateMachine, "attack0", entityData, meleeAttackPosition, this);
    }

    private void Start()
    {
        stateMachine.Initialize(FreezeState);
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
