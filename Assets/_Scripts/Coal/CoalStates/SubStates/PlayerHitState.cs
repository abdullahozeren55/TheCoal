using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    private Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private Movement movement;
    public PlayerHitState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement.CanSetVelocity = false;
    }

    public override void Exit()
    {
        base.Exit();

        Movement.CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
