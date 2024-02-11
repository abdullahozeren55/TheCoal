using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        if(player.transform.parent == null)
        {
            Movement?.SetVelocityX(0f);
        }
        else
        {
            Movement?.SetVelocityXY(player.transform.parent.GetComponent<Rigidbody2D>().velocityX, player.transform.parent.GetComponent<Rigidbody2D>().velocityY);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(player.shouldLandFreeze)
        {
            if(isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if (xInput != 0 && (!isTouchingWall || xInput != Movement?.FacingDirection))
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(!isGrounded && !isOnSlope)
        {
            stateMachine.ChangeState(player.InAirState);
        }      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
