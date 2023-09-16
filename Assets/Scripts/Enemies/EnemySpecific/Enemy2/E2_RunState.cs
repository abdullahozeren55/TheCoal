using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E2_RunState : RunState
{
    private Enemy2 enemy;


    public E2_RunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunState stateData, Enemy2 enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if(isRunOver)
        {
            entity.Flip();
            
            if(isPlayerInMaxAgroRange && performCloseRangeAction)
            {
                if(isDetectingLedgeRun && !isDetectingWallRun)
                {
                    stateMachine.ChangeState(enemy.runState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.rangedAttackState);
                }
                
            }
            else if(isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.rangedAttackState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
