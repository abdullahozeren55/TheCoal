using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
	private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

	private Movement movement;
	private CollisionSenses collisionSenses;

    private ParticleManager particleManager;
    private ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();

    private int xInput;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;

    private bool isGrounded;
    private bool isTouchingWall;
    private bool isTouchingWallBack;
    private bool oldIsTouchingWall;
	private bool oldIsTouchingWallBack;
    private bool isTouchingLedge;
    private bool isTouchingLedgeBottom;
    private bool isOnSlope;
    private bool isOnLeftSlope;
    private bool isOnRightSlope;

    private bool isJumping;

    private bool canWallHold;

    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool wallHoldCoyoteTime;

    private float startWallJumpCoyoteTime;
    private float startWallHoldCoyoteTime;

    private bool canInstantiateAirJumpPrefab;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTouchingWall = isTouchingWall;
		oldIsTouchingWallBack = isTouchingWallBack;

        if (CollisionSenses)
        {
			isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingWallBack = CollisionSenses.WallBack;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingLedgeBottom = CollisionSenses.LedgeHorizontalBottom;
            isOnSlope = CollisionSenses.Slope;
            isOnLeftSlope = CollisionSenses.LeftSlope;
            isOnRightSlope = CollisionSenses.RightSlope;
        }

        if (isTouchingWall && !isTouchingLedge && !isGrounded)
        {
			player.LedgeClimbState.SetDetectedPosition(player.transform.position);
		}

        if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack && (oldIsTouchingWall || oldIsTouchingWallBack))
        {
			StartWallJumpCoyoteTime();
		}
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTouchingWall = false;
		oldIsTouchingWallBack = false;
        isTouchingWall = false;
		isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();
        CheckWallHoldCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            player.AttackState.SetAttackIsHeavy(false);
            stateMachine.ChangeState(player.AttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            player.AttackState.SetAttackIsHeavy(true);
            stateMachine.ChangeState(player.AttackState);
        }
        else if ((isGrounded || isOnSlope) && Movement?.CurrentVelocity.y < 0.01f)
        {
            if(isGrounded)
            {
                ParticleManager?.StartParticles(player.groundedDustParticle, player.feetParticlePoint.position, Quaternion.identity);
            }
            else if(isOnLeftSlope)
            {
                ParticleManager?.StartParticles(player.groundedDustParticle, player.feetParticlePoint.position, Quaternion.Euler(0f, 0f, -45f));
            }
            else if(isOnRightSlope)
            {
                ParticleManager?.StartParticles(player.groundedDustParticle, player.feetParticlePoint.position, Quaternion.Euler(0f, 0f, 45f));
            }
            
            if(xInput != 0 && !isTouchingWall)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
			    stateMachine.ChangeState(player.LandState);
            }
        }
        else if (!isGrounded && !isOnSlope && isTouchingLedgeBottom && !isTouchingLedge)
        {
			stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime) && (canWallHold || wallHoldCoyoteTime))
        {
			StopWallJumpCoyoteTime();
            StopWallHoldCoyoteTime();
			isTouchingWall = CollisionSenses.WallFront;
			player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
			stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            if(canInstantiateAirJumpPrefab)
            {
                ParticleManager?.StartParticles(player.airJumpParticle, player.feetParticlePoint.position, Quaternion.identity);
            }
            
            coyoteTime = false;
            canInstantiateAirJumpPrefab = true;
			stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0 && canWallHold)
        {
            stateMachine.ChangeState(player.WallHoldState);
        }
        else if (dashInput && player.SuperDashState.CheckIfCanSuperDash())
        {
			stateMachine.ChangeState(player.SuperDashState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash() && !player.SuperDashState.CheckIfCanSuperDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);
		    Movement?.SetVelocityX(playerData.movementVelocity * xInput);

		    player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		    player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
            player.EyesAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		    player.EyesAnim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		        player.CoalSwordAnim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
            }
            
        }


    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckJumpMultiplier()
    {
		if (isJumping)
        {
			if (jumpInputStop)
            {
				Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
				isJumping = false;
			}
            else if (Movement.CurrentVelocity.y <= 0f)
            {
				isJumping = false;
			}

		}
	}

    private void CheckCoyoteTime()
    {
		if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
			coyoteTime = false;
            canInstantiateAirJumpPrefab = true;
			player.JumpState.DecreaseAmountOfJumpsLeft();
		}
	}

    public void SetCanWallHold(bool value)
    {
        canWallHold = value;
    }

    private void CheckWallJumpCoyoteTime()
    {
		if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
        {
			wallJumpCoyoteTime = false;
		}
	}

    private void CheckWallHoldCoyoteTime()
    {
		if (wallHoldCoyoteTime && Time.time > startWallHoldCoyoteTime + playerData.coyoteTime)
        {
			wallHoldCoyoteTime = false;
		}
	}

    public void StartWallJumpCoyoteTime()
    {
		wallJumpCoyoteTime = true;
		startWallJumpCoyoteTime = Time.time;
	}

    public void StartWallHoldCoyoteTime()
    {
		wallHoldCoyoteTime = true;
		startWallHoldCoyoteTime = Time.time;
	}

	public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void StopWallHoldCoyoteTime() => wallHoldCoyoteTime = false;

    public void StartCoyoteTime()
    {
        coyoteTime = true;
        canInstantiateAirJumpPrefab = false;
    }

    public void SetIsJumping() => isJumping = true;
}
