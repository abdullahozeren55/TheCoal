using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_PlayerDetectedState : PlayerDetectedState
{

    private Rat enemy;
    public Rat_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetectedState stateData, Rat enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        if(isGrounded)
        {
            enemy.SetVelocityY(6f);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if(performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if(!isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
