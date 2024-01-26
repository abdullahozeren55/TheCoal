using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{

    private bool isGrounded;
    private bool isTouchingWall;
    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
	private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

	private Movement movement;
	private CollisionSenses collisionSenses;

    private int xInput;
    private bool jumpInput;
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)
        {
			isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
        }
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

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput)
        {
			player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
			stateMachine.ChangeState(player.WallJumpState);
        }
        else if(isGrounded)
        {
            if(xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.MoveState);
            }
            
        }
        else if(Time.time >= startTime + playerData.wallSlideTime || xInput != Movement?.FacingDirection || !isTouchingWall)
        {
            player.InAirState.SetCanWallHold(false);
            player.InAirState.StartWallHoldCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else
        {
            Movement?.SetVelocityY(-playerData.wallSlideVelocity);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
