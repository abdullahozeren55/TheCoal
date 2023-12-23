using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : State
{

    protected CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();}
    private CollisionSenses collisionSenses;

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;

    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInMinAgroRange;
    protected bool performCloseRangeAction;
    protected bool isGrounded;
    protected bool isOnSlope;
    protected bool isDetectingLedge;

    public HitState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData) : base(entity, stateMachine, animBoolName, entityData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isOnSlope = CollisionSenses.Slope;
            isDetectingLedge = CollisionSenses.LedgeVertical;

        }
    }

    public override void Enter()
    {
        base.Enter();
        Movement.CanSetVelocity = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        if(entity.gotBackAttacked && !isPlayerInMaxAgroRange)
        {
            Movement.Flip();
        }
        Movement.CanSetVelocity = true;
        
    }
}
