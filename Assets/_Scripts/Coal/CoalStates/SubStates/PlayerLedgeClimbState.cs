using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    private Movement movement;
    private CollisionSenses collisionSenses;

    private Vector2 detectedPos;
    private Vector2 cornerPos;
    private Vector2 startPos;
	private Vector2 stopPos;
	private Vector2 workspace;

    private bool isHanging;
	private bool isClimbing;

    private int xInput;
    private int yInput;
    private bool jumpInput;

    private bool isTouchingWall;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("climbLedge", false);
        player.EyesAnim.SetBool("climbLedge", false);
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
        {
            isTouchingWall = CollisionSenses.WallFront;
        }
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();
        player.transform.position = detectedPos;
        cornerPos = DetermineCornerPosition();

        startPos.Set(cornerPos.x - (Movement.FacingDirection * playerData.startOffset.x), cornerPos.y - playerData.startOffset.y);
		stopPos.Set(cornerPos.x + (Movement.FacingDirection * playerData.stopOffset.x), cornerPos.y + playerData.stopOffset.y);

		player.transform.position = startPos;

        player.SuperDashState.SetCanSuperDash(true);
        player.InAirState.SetCanWallHold(true);
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;

		if (isClimbing)
        {
			player.transform.position = stopPos;
			isClimbing = false;
		}

        player.Anim.SetBool("climbLedge", false);
        player.Anim.SetBool("ledgeTurn", false);
        player.EyesAnim.SetBool("climbLedge", false);
        player.EyesAnim.SetBool("ledgeTurn", false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if(xInput != 0 && (!isTouchingWall || xInput != Movement?.FacingDirection))
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
			    stateMachine.ChangeState(player.IdleState);
            }
		}
        else
        {
			xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
			jumpInput = player.InputHandler.JumpInput;

			Movement?.SetVelocityZero();
			player.transform.position = startPos;

            if (jumpInput && !isClimbing && xInput == -Movement.FacingDirection)
            {
				player.WallJumpState.DetermineWallJumpDirection(true);
				stateMachine.ChangeState(player.WallJumpState);
			}
			else if (jumpInput || (jumpInput && xInput == Movement.FacingDirection && isHanging && !isClimbing))
            {
				isClimbing = true;
				player.Anim.SetBool("climbLedge", true);
                player.EyesAnim.SetBool("climbLedge", true);
			}
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                player.InAirState.SetCanWallHold(true);
				stateMachine.ChangeState(player.InAirState);
			}

            if(xInput == -Movement.FacingDirection)
            {
                player.Anim.SetBool("ledgeTurn", true);
                player.EyesAnim.SetBool("ledgeTurn", true);
            }
            else
            {
                player.Anim.SetBool("ledgeTurn", false);
                player.EyesAnim.SetBool("ledgeTurn", false);
            }
		}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private Vector2 DetermineCornerPosition()
    {
		RaycastHit2D xHit = Physics2D.Raycast(CollisionSenses.WallCheck.position, Vector2.right * Movement.FacingDirection, CollisionSenses.WallCheckDistance, CollisionSenses.WhatIsLedge);
		float xDist = xHit.distance;
		workspace.Set((xDist + 0.015f) * Movement.FacingDirection, 0f);
		RaycastHit2D yHit = Physics2D.Raycast(CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(workspace), Vector2.down, CollisionSenses.LedgeCheckHorizontal.position.y - CollisionSenses.WallCheck.position.y + 0.015f, CollisionSenses.WhatIsLedge);
		float yDist = yHit.distance;

		workspace.Set(CollisionSenses.WallCheck.position.x + (xDist * Movement.FacingDirection), CollisionSenses.LedgeCheckHorizontal.position.y - yDist);
		return workspace;
	}

    public void SetDetectedPosition(Vector2 pos) => detectedPos = pos;
}
