using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_ChargeState : ChargeState
{
    private NewRat enemy;
    public NewRat_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
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

        if(isDetectingLedge || isDetectingWall)
        {
            Movement?.Flip();
            stateMachine.ChangeState(enemy.MoveState);
        }
        else if(performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.JumpAttackState);
        }
        else if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else
        {
            Movement?.SetVelocityX(entityData.chargeSpeed * Movement.FacingDirection);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
