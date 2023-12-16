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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(jumpInput && player.JumpState.CanJump())
        {
        stateMachine.ChangeState(player.JumpState);
        }
        else if (xInput != 0 && (!isTouchingWall || xInput != Movement?.FacingDirection))
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(!isGrounded)
        {
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
