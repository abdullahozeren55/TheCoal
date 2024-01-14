using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_GettingReadyToDashState : IdleState
{
    private Bat enemy;
    public Bat_GettingReadyToDashState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Bat enemy) : base(entity, stateMachine, animBoolName, entityData)
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
        
        if(isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.DashState);
        }
        else
        {
            Movement?.SetVelocityX(0f);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
