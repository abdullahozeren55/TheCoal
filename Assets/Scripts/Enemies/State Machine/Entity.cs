using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

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

    public virtual void Start()
    {
        facingDirection = 1;
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.maxStunResistance;
        isStunned = false;
        isDead = false;

        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        atsm = aliveGO.GetComponent<AnimationToStateMachine>();


        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
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

    public virtual void SetVelocityX(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocityY(float velocity)
    {
        velocityWorkspace.Set(rb.velocity.x, velocity);
        rb.velocity = velocityWorkspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right * facingDirection, entityData.wallCheckDistance, entityData.whatIsWall);
    }
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, entityData.groundCheckDistance, entityData.whatIsLedge);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, entityData.groundCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right * facingDirection, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right * facingDirection, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveGO.transform.right * facingDirection, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual void Damage(AttackDetails attackDetails)
    {

        lastDamageTime = Time.time;

        currentHealth -= attackDetails.hpDamageAmount;
        currentStunResistance -= attackDetails.stunResDamageAmount;

        if(attackDetails.position.x > aliveGO.transform.position.x)
        {
            lastDamageDirection = -1;
        }
        else
        {
            lastDamageDirection = 1;
        }
        Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
        SetVelocity(entityData.damageHopVelocity, entityData.damageHopAngle, lastDamageDirection);

        if(currentHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            isDead = false;
        }

        if(currentStunResistance <= 0)
        {
            isStunned = true;
        }
        else
        {
            isStunned = false;
        }
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

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.localScale = new Vector3(aliveGO.transform.localScale.x * -1f, 1f, 1f);
    }

    public virtual void OnDrawGizmos()
    {

    }

}
