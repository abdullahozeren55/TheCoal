using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInCloseRangeAction;

    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private Movement movement;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData) : base(entity, stateMachine, animBoolName, entityData)
    {
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
        isPlayerInCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();
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
