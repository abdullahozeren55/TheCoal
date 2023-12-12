using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_ChargeState : ChargeState
{

    private Rat enemy;
    public Rat_ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData, Rat enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
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
        
        if(performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if(isChargeTimeOver)
        {
            if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
        else if(isDetectingLedge || isDetectingWall)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
