using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float elapsedTime;
    private float lerpedAmount;
    private float stateLength = 0.5f;
    private bool shouldAccel;
    private bool shouldAccelForSoundFX;
    private float startingAccelSpeed;
    private float walkOnGrassSoundFXCooldownForState;
    private float startingSoundFXCooldown;

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        if(stateMachine.PreviousState == player.FlipState || stateMachine.PreviousState == player.LandState)
        {
            lerpedAmount = 0f;
            elapsedTime = 0f;
            shouldAccel = true;
            shouldAccelForSoundFX = false;
            startingAccelSpeed = playerData.flipMovementVelocity;
            walkOnGrassSoundFXCooldownForState = playerData.walkOnGrassSoundFXCooldown;
        }
        else if(stateMachine.PreviousState == player.DashState)
        {
            lerpedAmount = 0f;
            elapsedTime = 0f;
            shouldAccel = true;
            shouldAccelForSoundFX = true;
            startingAccelSpeed = playerData.dashVelocity/2f;
            walkOnGrassSoundFXCooldownForState = playerData.walkOnGrassSoundFXCooldown/3f;
            startingSoundFXCooldown = walkOnGrassSoundFXCooldownForState;
        }
        else
        {
            shouldAccel = false;
            shouldAccelForSoundFX = false;
            walkOnGrassSoundFXCooldownForState = playerData.walkOnGrassSoundFXCooldown;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            player.AttackState.SetAttackIsHeavy(false);
            player.AttackState.SetIsMoving(true);
            stateMachine.ChangeState(player.AttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            player.AttackState.SetAttackIsHeavy(true);
            player.AttackState.SetIsMoving(true);
            stateMachine.ChangeState(player.AttackState);
        }
        else if (isTouchingLedgeBottom && !isTouchingLedge)
        {
			stateMachine.ChangeState(player.LedgeClimbState);
        }
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if(xInput == 0 || isTouchingWall)
        {
            stateMachine.ChangeState(player.StopMovingState);
        }
        else if(!isGrounded && !isOnSlope)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(xInput != 0 && xInput != Movement?.FacingDirection)
        {
            stateMachine.ChangeState(player.FlipState);
        }
        else
        {
            if(shouldAccel)
            {
                if(elapsedTime <= stateLength)
                {
                    elapsedTime += Time.deltaTime;
                    lerpedAmount = Mathf.Lerp(startingAccelSpeed, playerData.movementVelocity, (elapsedTime/stateLength));
                    Movement?.SetVelocityX(lerpedAmount * xInput);

                    if(shouldAccelForSoundFX)
                    {
                        walkOnGrassSoundFXCooldownForState = Mathf.Lerp(startingSoundFXCooldown, playerData.walkOnGrassSoundFXCooldown, (elapsedTime/stateLength));
                    }
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
                if(Movement?.CurrentVelocity.x != playerData.movementVelocity * xInput)
                {
                    Movement?.SetVelocityX(playerData.movementVelocity * xInput);
                }
            }

            if(player.isInStatueLevel)
            {
                if(playerData.walkOnGrassSoundFXTimer >= walkOnGrassSoundFXCooldownForState)
                {
                    SoundFXManager.instance.PlaySoundFXClip(playerData.walkOnGrassSoundFX, player.transform, 0.6f, 0.8f, 1.2f);
                    playerData.walkOnGrassSoundFXTimer = 0f;
                }
                else
                {
                    playerData.walkOnGrassSoundFXTimer += Time.deltaTime;
                }
            }
            

            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
