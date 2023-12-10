using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity
{
    public Rat_IdleState idleState { get; private set; }
    public Rat_MoveState moveState { get; private set; }

    public Rat_PlayerDetectedState playerDetectedState { get; private set; }

    public Rat_ChargeState chargeState { get; private set; }

    public Rat_MeleeAttackState meleeAttackState { get; private set; }

    public Rat_StunState stunState { get; private set; }

    public Rat_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_PlayerDetectedState playerDetectedStateData;
    [SerializeField]
    private D_ChargeState chargeStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new Rat_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Rat_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Rat_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Rat_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new Rat_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Rat_StunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new Rat_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if(isDead && stateMachine.currentState != deadState)
        {
            stateMachine.ChangeState(deadState);
        }
        else if(isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
