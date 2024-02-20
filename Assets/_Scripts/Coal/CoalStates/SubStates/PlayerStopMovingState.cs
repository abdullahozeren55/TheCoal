using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStopMovingState : PlayerGroundedState
{

    public bool stopScarfUp;

    private float elapsedTime;
    private float lerpedAmount;
    private float stateLength = 0.2f;
    private float startingAccelSpeed;
    public PlayerStopMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        startingAccelSpeed = Mathf.Abs(Movement.CurrentVelocity.x/2f);

        if(Movement?.CurrentVelocity.x != startingAccelSpeed * Movement.FacingDirection)
        {
            Movement?.SetVelocityX(startingAccelSpeed * Movement.FacingDirection);
        }

        lerpedAmount = 0f;
        elapsedTime = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.shouldFreeze)
        {
            if(isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else
        {   
            if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
            {
                player.AttackState.SetAttackIsHeavy(false);
                player.AttackState.SetIsMoving(false);
                stateMachine.ChangeState(player.AttackState);
            }
            else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
            {
                player.AttackState.SetAttackIsHeavy(true);
                player.AttackState.SetIsMoving(false);
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
            else if(xInput == Movement?.FacingDirection && !isTouchingWall)
            {
                if(stopScarfUp)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.StartMovingState);
                }
            
            }
            else if(!isGrounded && !isOnSlope)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else if(xInput != 0 && xInput != Movement?.FacingDirection)
            {
                stateMachine.ChangeState(player.FlipState);
            }
            else if(isAnimationFinished && (xInput == 0 || isTouchingWall ))
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                if(elapsedTime <= stateLength)
                {
                    elapsedTime += Time.deltaTime;
                    lerpedAmount = Mathf.Lerp(startingAccelSpeed, 0f, (elapsedTime/stateLength));
                    Movement?.SetVelocityX(lerpedAmount * Movement.FacingDirection);
                }
            }
        }
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        stopScarfUp = false;
        player.StartMovingState.startScarfUp = false;
    }
}
