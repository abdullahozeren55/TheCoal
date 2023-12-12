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

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>(); }
    private CollisionSenses collisionSenses;
    private Movement movement;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
        
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        isMovementStopped = false;
        Movement?.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);
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
            Movement?.SetVelocityX(0f);
            isMovementStopped = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
