using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_LookForPlayerState : LookForPlayerState
{
    private NewRat enemy;
    private float animationTime;
    public NewRat_LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
        if(isPlayerInCloseRangeAction)
        {
            enemy.IdleState.SetFlipAfterIdle(false);
            stateMachine.ChangeState(enemy.IdleState);
        }
        else if(isPlayerInMaxAgroRange)
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
