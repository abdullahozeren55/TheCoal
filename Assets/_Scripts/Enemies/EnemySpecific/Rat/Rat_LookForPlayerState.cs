using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_LookForPlayerState : LookForPlayerState
{
    private Rat enemy;
    public Rat_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isPlayerInCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.MeleeAttackState);
        }
        if(isPlayerInMaxAgroRange)
        {
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if(isAllTurnsTimeDone)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
        else
        {
            Movement?.SetVelocityX(0f);
        }
    }
}
