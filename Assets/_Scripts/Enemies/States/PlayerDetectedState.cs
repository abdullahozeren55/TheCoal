using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;

    protected bool isGrounded;
    protected bool isOnSlope;
    protected bool isDetectingLedge;

    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private Movement movement;
    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData) : base(entity, stateMachine, animBoolName, entityData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isOnSlope = CollisionSenses.Slope;
            isDetectingLedge = CollisionSenses.LedgeVertical;
        }
        
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityX(0f);

        performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if(Time.time >= startTime + entityData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
