using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private float elapsedTime;
    private float lerpedAmount;
    private float stateLength = 0.5f;
    private bool shouldAccel;
    private float startingAccelSpeed;
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
            startingAccelSpeed = playerData.flipMovementVelocity;

            if(Movement?.CurrentVelocity.x != playerData.flipMovementVelocity * xInput)
            {
                Movement?.SetVelocityX(playerData.flipMovementVelocity * xInput);
            }
        }
        else if(stateMachine.PreviousState == player.DashState)
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

            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
