using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_HitState : HitState
{
    private Rat enemy;
    public Rat_HitState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
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
        Movement.CanSetVelocity = true;
        isAnimationFinished = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            if(isGrounded || isOnSlope || isDetectingLedge)
            {
                if(performCloseRangeAction)
                {
                    stateMachine.ChangeState(enemy.MeleeAttackState);
                }
                else if(isPlayerInMaxAgroRange)
                {
                    stateMachine.ChangeState(enemy.ChargeState);
                }
                else
                {
                    stateMachine.ChangeState(enemy.MoveState);
                }
            }
            else
            {
                stateMachine.ChangeState(enemy.MoveState);
            }  
            
        }
    }
}
