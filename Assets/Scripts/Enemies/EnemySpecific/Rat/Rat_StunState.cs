using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_StunState : StunState
{
    private Rat enemy;
    public Rat_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData, Rat enemy) : base(entity, stateMachine, animBoolName, stateData)
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
        if(!enemy.isStunned)
        {
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.meleeAttackState);
            }
            else if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.chargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
