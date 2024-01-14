using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_HitState : HitState
{
    private Bat enemy;
    public Bat_HitState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Bat enemy) : base(entity, stateMachine, animBoolName, entityData)
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

        if(isAnimationFinished)
        {       
            stateMachine.ChangeState(enemy.FollowPlayerState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
