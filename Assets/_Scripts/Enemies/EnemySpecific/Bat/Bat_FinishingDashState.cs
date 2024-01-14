using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_FinishingDashState : IdleState
{
    private Bat enemy;
    public Bat_FinishingDashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Bat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
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
