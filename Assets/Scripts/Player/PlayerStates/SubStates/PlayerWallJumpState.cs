using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{

    private int wallJumpDirection;

    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
        player.CheckIfShouldFlip(wallJumpDirection);
        player.JumpState.DecreaseAmountOfJumpsLeft();
        playerData.wallJumpCombo = true;
        
    }

    public override void Exit()
    {
        base.Exit();
        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
        player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

        if(Time.time >= startTime + playerData.wallJumpTime)
        {
            isAbilityDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
        if(isTouchingWall)
        {
            wallJumpDirection = -player.FacingDirection;
        }
        else
        {
            wallJumpDirection = player.FacingDirection;
        }
    }
}
