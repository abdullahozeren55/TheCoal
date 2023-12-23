using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_IdleState : IdleState
{
    private NewRat enemy;

    public float lastAttackTime;

    private bool shouldLookForPlayer;
    public NewRat_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();
        shouldLookForPlayer = false;
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
            shouldLookForPlayer = true;
            if(Time.time >= lastAttackTime + entityData.attackNormalCooldown)
            {
                stateMachine.ChangeState(enemy.NormalAttackState);
            }            
        }
        else if(isPlayerInMaxAgroRange && !isDetectingLedge && !isDetectingWall)
        {
            SetFlipAfterIdle(false);
            stateMachine.ChangeState(enemy.ChargeState);
        }
        else if(!isPlayerInMaxAgroRange && shouldLookForPlayer)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if(isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
        else
        {
            Movement?.SetVelocityZero();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
