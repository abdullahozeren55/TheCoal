using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Movement Movement { get => movement ??= Core.GetCoreComponent<Movement>(); }
    private Movement movement;

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public Animator anim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    public Core Core { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform playerCheck;

    private Vector2 velocityWorkspace;

    private float currentHealth;

    private float currentStunResistance;

    private float stunResRegAmount;
    private float lastDamageTime;
    private float lastStunResRegTime;

    public int lastDamageDirection { get; private set; }

    public bool isStunned;
    public bool isDead;

    public virtual void Awake()
    {

        Core = GetComponentInChildren<Core>();
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.maxStunResistance;
        isStunned = false;
        isDead = false;
        anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();


        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();

        if(Time.time >= lastDamageTime + entityData.timeBeforeStunResStartsRegain && Time.time >= lastStunResRegTime + entityData.stunResRegCooldown)
        {
            StunResistanceRegeneration();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    private void StunResistanceRegeneration()
    {
        lastStunResRegTime = Time.time;
        SetRandomStunResistanceRegeneration();
        currentStunResistance += stunResRegAmount;
        if(currentStunResistance > 0)
            {
                isStunned = false;
            }
    }

    private void SetRandomStunResistanceRegeneration()
    {
        stunResRegAmount = Random.Range(entityData.minStunResistanceRegain, entityData.maxStunResistanceRegain); 
    }

    public virtual void OnDrawGizmos()
    {

    }

}
