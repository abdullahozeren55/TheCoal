using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{

    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.eyesAnim.SetBool("land", true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (jumpInput && player.JumpState.CheckIfCanJump() && !playerData.isJumping)
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (xInput != 0)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
        player.eyesAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
