using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    protected bool flipAfterIdle;
    protected bool isIdleTimeOver;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInCloseRangeAction;
    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isGrounded;

    protected float idleTime;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private Movement movement;

    protected ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();

    private ParticleManager particleManager;

    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData) : base(entity, stateMachine, animBoolName, entityData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

        if(CollisionSenses)
        {
            isDetectingLedge = CollisionSenses.LedgeVertical;
            isDetectingWall = CollisionSenses.WallFront;
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if(flipAfterIdle)
        {
            Movement?.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        idleTime = Random.Range(entityData.minIdleTime, entityData.maxIdleTime);
    }
}
