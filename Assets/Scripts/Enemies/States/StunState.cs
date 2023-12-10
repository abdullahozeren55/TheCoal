using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;
    protected bool isGrounded;
    protected bool isMovementStopped;

    protected bool performCloseRangeAction;

    protected bool isPlayerInMaxAgroRange;

    protected bool isPlayerInMinAgroRange;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isGrounded = entity.CheckGround();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isMovementStopped = false;
        entity.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped)
        {
            entity.SetVelocityX(0f);
            isMovementStopped = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
