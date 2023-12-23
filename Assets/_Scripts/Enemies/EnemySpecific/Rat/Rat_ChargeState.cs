using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_ChargeState : ChargeState
{

    private Rat enemy;

    public Rat_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
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

        Movement?.SetVelocityX(entityData.chargeSpeed * Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if((isDetectingLedge || isDetectingWall) && !isPlayerInMaxAgroRange)
        {
            enemy.IdleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.IdleState);
        }
        else if(performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        else //if(isChargeTimeOver)
        {
            if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.PlayerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.MoveState);
            }
        }
        
       // else
        //{
          //  Movement?.SetVelocityX(entityData.chargeSpeed * Movement.FacingDirection);
        //}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
