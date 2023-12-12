using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{

    protected D_PlayerDetectedState stateData;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool performLongRangeAction;
    protected bool performCloseRangeAction;

    protected bool isGrounded;

    protected CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();}
    private CollisionSenses collisionSenses;

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;
    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
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
        
        if(Time.time >= startTime + stateData.longRangeActionTime)
        {
            performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
