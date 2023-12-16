using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Entity
{
    //TODO: Make a hit state and make sure rat does nothing in it. Attacks will knockback the rat.
    public Rat_IdleState idleState { get; private set; }
    public Rat_MoveState moveState { get; private set; }

    public Rat_PlayerDetectedState playerDetectedState { get; private set; }

    public Rat_ChargeState chargeState { get; private set; }

    public Rat_MeleeAttackState meleeAttackState { get; private set; }

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
    private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        idleState = new Rat_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new Rat_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new Rat_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Rat_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        meleeAttackState = new Rat_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);

    }

    private void Start()
    {
        stateMachine.Initialize(moveState);

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}
