using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    private Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private Movement movement;

    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    private bool isGrounded;
    public PlayerHitState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
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
            if(isGrounded)
            {

                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
