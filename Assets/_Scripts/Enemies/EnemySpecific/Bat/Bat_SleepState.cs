using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_SleepState : IdleState
{
    private Bat enemy;
    public Bat_SleepState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Bat enemy) : base(entity, stateMachine, animBoolName, entityData)
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

        enemy.anim.SetBool("wakeUp", false);
        enemy.anim.SetBool("halfAwake", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(enemy.shouldWakeUp)
        {
            enemy.anim.SetBool("wakeUp", true);
            enemy.anim.SetBool("halfAwake", false);
        }
        else if(enemy.shouldHalfAwake)
        {
            enemy.anim.SetBool("halfAwake", true);
            enemy.anim.SetBool("wakeUp", false);
        }
        else
        {
            enemy.anim.SetBool("halfAwake", false);
            enemy.anim.SetBool("wakeUp", false);
        }

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
