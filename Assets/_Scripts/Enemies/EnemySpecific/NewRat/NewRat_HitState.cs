using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_HitState : HitState
{
    private NewRat enemy;
    public NewRat_HitState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Exit()
    {
        base.Exit();
        isAnimationFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished && (isGrounded || isOnSlope || isDetectingLedge))
        {
            
            if(performCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.NormalAttackState);
            }
            else if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.ChargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
