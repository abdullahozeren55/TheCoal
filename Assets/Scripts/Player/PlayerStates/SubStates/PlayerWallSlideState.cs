using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        player.SetVelocityY(-playerData.wallSlideVelocity);
        
        if(jumpInput && (isTouchingWall || isTouchingWallBack) && player.JumpState.CheckIfCanJump())
        {
            
            player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        else if((Time.time >= startTime + playerData.stickingWallTime))
        {
            playerData.canStickWall = false;
            playerData.wallJumpCombo = false;
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
