using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_MoveState : MoveState
{
    private Rat enemy;

    private bool canDetectLedge;

    public Rat_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityX(entityData.movementSpeed * Movement.FacingDirection);
        canDetectLedge = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.PlayerDetectedState);
        }
        else if(isDetectingWall || (isDetectingLedge && canDetectLedge))
        {
            enemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.IdleState);
        }   
        else
        {
            Movement?.SetVelocityX(entityData.movementSpeed * Movement.FacingDirection);
            if(Time.time >= startTime + entityData.ledgeDetectionCooldown)
            {
                canDetectLedge = true;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
