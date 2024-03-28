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
    private bool isTouchingLedge;
    private bool isTouchingLedgeBottom;
    private bool isOnSlope;
    private bool isOnLeftSlope;
    private bool isOnRightSlope;

    private bool isJumping;

    private bool canWallHold;

    private bool coyoteTime;
    private bool wallHoldCoyoteTime;
    public bool shouldInstantiateAirJumpPrefab;
    private float startWallHoldCoyoteTime;

    private float elapsedTime;
    private float lerpedAmount;
    private float stateLength = 0.2f;
    private bool shouldAccel;
    private float startingAccelSpeed;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
			isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isTouchingLedge = CollisionSenses.LedgeHorizontal;
            isTouchingLedgeBottom = CollisionSenses.LedgeHorizontalBottom;
            isOnSlope = CollisionSenses.Slope;
            isOnLeftSlope = CollisionSenses.LeftSlope;
            isOnRightSlope = CollisionSenses.RightSlope;

            if (isTouchingLedgeBottom && !isTouchingLedge)
            {
			    player.LedgeClimbState.SetDetectedPosition(player.transform.position);
		    }
        }

        
    }

    public override void Enter()
    {
        base.Enter();

        if((stateMachine.PreviousState == player.IdleState || stateMachine.PreviousState == player.MoveState || stateMachine.PreviousState == player.StartMovingState || stateMachine.PreviousState == player.StopMovingState || stateMachine.PreviousState == player.LandState || stateMachine.PreviousState == player.FlipState))
        {
            StartCoyoteTime();
        }


        if(stateMachine.PreviousState == player.DashState)
        {
            lerpedAmount = 0f;
            elapsedTime = 0f;
            shouldAccel = true;
            startingAccelSpeed = playerData.dashVelocity/1.5f;

            if(Movement?.CurrentVelocity.x != playerData.dashVelocity/1.5f * xInput)
            {
                Movement?.SetVelocityX(playerData.dashVelocity/1.5f * xInput);
            }
        }
        else
        {
            shouldAccel = false;
            if(Movement?.CurrentVelocity.x != playerData.movementVelocity * xInput)
            {
                Movement?.SetVelocityX(playerData.movementVelocity * xInput);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        //isTouchingWall = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
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
        else if (isTouchingLedgeBottom && !isTouchingLedge)
        {
			stateMachine.ChangeState(player.LedgeClimbState);
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
                playerData.walkOnGrassSoundFXTimer = playerData.walkOnGrassSoundFXCooldown + 0.1f;

                if(player.isInStatueLevel)
                {
                    SoundFXManager.instance.PlaySoundFXClip(playerData.landOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
                }

                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
			    stateMachine.ChangeState(player.LandState);
            }
        }
        else if (jumpInput && isTouchingWall && (canWallHold || wallHoldCoyoteTime) && (xInput == Movement?.FacingDirection || player.WallJumpState.inWallJumpCombo))
        {
            StopWallHoldCoyoteTime();

            if(player.WallJumpState.inWallJumpCombo)
            {
                if(player.isInStatueLevel)
                {
                    SoundFXManager.instance.PlaySoundFXClip(playerData.landOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
                }
            }

			stateMachine.ChangeState(player.WallJumpState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            if(shouldInstantiateAirJumpPrefab)
            {
                ParticleManager?.StartParticles(player.airJumpParticle, player.feetParticlePoint.position, Quaternion.identity);
            }
            
            if(coyoteTime)
            {
                if(player.isInStatueLevel)
                {
                    SoundFXManager.instance.PlaySoundFXClip(playerData.jumpOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
                }
                coyoteTime = false;
            }
            shouldInstantiateAirJumpPrefab = true;
			stateMachine.ChangeState(player.JumpState);
        }
        else if (isTouchingWall && xInput == Movement?.FacingDirection && Movement?.CurrentVelocity.y <= 0 && canWallHold)
        {
            stateMachine.ChangeState(player.WallHoldState);
        }
        else if (playerData.superDashUnlocked && dashInput && player.SuperDashState.CheckIfCanSuperDash())
        {
			stateMachine.ChangeState(player.SuperDashState);
        }
        else if(playerData.dashUnlocked && dashInput && player.DashState.CheckIfCanDash() && (!playerData.superDashUnlocked || !player.SuperDashState.CheckIfCanSuperDash()))
        {
            stateMachine.ChangeState(player.DashState);
        }
        else
        {
            Movement?.CheckIfShouldFlip(xInput);

            if(shouldAccel)
            {
                if(elapsedTime <= stateLength)
                {
                    elapsedTime += Time.deltaTime;
                    lerpedAmount = Mathf.Lerp(startingAccelSpeed, playerData.movementVelocity, (elapsedTime/stateLength));
                    Movement?.SetVelocityX(lerpedAmount * xInput);
                }
                else
                {
                    if(Movement?.CurrentVelocity.x != playerData.movementVelocity * xInput)
                    {
                        Movement?.SetVelocityX(playerData.movementVelocity * xInput);
                    }
                }
            }
            else
            {
		        Movement?.SetVelocityX(playerData.movementVelocity * xInput);
            }

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
            shouldInstantiateAirJumpPrefab = true;
            if((stateMachine.PreviousState == player.IdleState || stateMachine.PreviousState == player.MoveState || stateMachine.PreviousState == player.StartMovingState || stateMachine.PreviousState == player.StopMovingState || stateMachine.PreviousState == player.LandState || stateMachine.PreviousState == player.FlipState))
            {
			    player.JumpState.DecreaseAmountOfJumpsLeft();
            }
		}
	}

    public void SetCanWallHold(bool value)
    {
        canWallHold = value;
    }

    private void CheckWallHoldCoyoteTime()
    {
		if (wallHoldCoyoteTime && Time.time > startWallHoldCoyoteTime + playerData.coyoteTime)
        {
			wallHoldCoyoteTime = false;
		}
	}

    public void StartWallHoldCoyoteTime()
    {
		wallHoldCoyoteTime = true;
		startWallHoldCoyoteTime = Time.time;
	}

    public void StopWallHoldCoyoteTime() => wallHoldCoyoteTime = false;

    private void StartCoyoteTime()
    {
        coyoteTime = true;
    }

    public void SetIsJumping() => isJumping = true;
}
