using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{

    private int wallJumpDirection;

    public float lastWallJumpedTime;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.InputHandler.UseJumpInput();
		player.JumpState.ResetAmountOfJumpsLeft();
		Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
		Movement?.CheckIfShouldFlip(wallJumpDirection);
		player.JumpState.DecreaseAmountOfJumpsLeft();
        player.InAirState.SetCanWallHold(true);
        player.wallJumpCombo = true;
    }

    public override void Exit()
    {
        base.Exit();

        lastWallJumpedTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        player.EyesAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.EyesAnim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if (Time.time >= startTime + playerData.wallJumpTime) {
			isAbilityDone = true;
		}

        if(isAbilityDone)
        {
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void DetermineWallJumpDirection(bool isTouchingWall)
    {
		if (isTouchingWall)
        {
			wallJumpDirection = -Movement.FacingDirection;
		} else
        {
			wallJumpDirection = Movement.FacingDirection;
		}
	}
}
