using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipState : PlayerGroundedState
{
    private float elapsedTime;
    private float lerpedAmount;
    private float startingAccelSpeed;
    public PlayerFlipState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.Flip();

        startingAccelSpeed = Mathf.Abs(Movement.CurrentVelocity.x/2f);

        if(Movement?.CurrentVelocity.x != startingAccelSpeed * -Movement.FacingDirection)
        {
            Movement?.SetVelocityX(startingAccelSpeed * -Movement.FacingDirection);
        }

        lerpedAmount = 0f;
        elapsedTime = 0f;
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
        else if(!isGrounded && !isOnSlope)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(xInput != 0 && xInput != Movement?.FacingDirection)
        {
            stateMachine.ChangeState(player.FlipState);
        }
        else if(Time.time >= startTime + playerData.flipTime && xInput == Movement.FacingDirection && !isTouchingWall)
        {
            if(player.StartMovingState.startScarfUp)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.StartMovingState);
            }
        }
        else if(Time.time >= startTime + playerData.flipTime && (xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection)))
        {
            if(player.StopMovingState.stopScarfUp)
            {
                stateMachine.ChangeState(player.StopMovingState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
            
        }
        else
        {
            if(elapsedTime <= playerData.flipTime)
                {
                    elapsedTime += Time.deltaTime;
                    lerpedAmount = Mathf.Lerp(startingAccelSpeed * -Movement.FacingDirection, playerData.flipMovementVelocity * Movement.FacingDirection, (elapsedTime/playerData.flipTime));
                    Movement?.SetVelocityX(lerpedAmount);
                }
        }
    }
}
