using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;

    protected CollisionSenses CollisionSenses { get => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();}
    private CollisionSenses collisionSenses;

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if(CollisionSenses)
        {
            isDetectingWall = CollisionSenses.WallFront;
            isDetectingLedge = CollisionSenses.LedgeVertical;
        }
        
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(stateData.movementSpeed * Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
