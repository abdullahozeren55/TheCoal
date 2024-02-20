using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartMovingState : PlayerGroundedState
{

    public bool startScarfUp;

    private float elapsedTime;
    private float lerpedAmount;
    private float stateLength = 0.857f;
    public PlayerStartMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(Movement?.CurrentVelocity.x != playerData.startMovementVelocity * xInput)
        {
            Movement?.SetVelocityX(playerData.startMovementVelocity * xInput);
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
        else if(xInput == 0 || isTouchingWall)
        {
            if(startScarfUp)
            {
                stateMachine.ChangeState(player.StopMovingState);
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
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
        else if(isAnimationFinished && xInput == Movement?.FacingDirection && !isTouchingWall)
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else
        {

            if(elapsedTime <= stateLength)
            {
                elapsedTime += Time.deltaTime;
                lerpedAmount = Mathf.Lerp(playerData.startMovementVelocity, playerData.movementVelocity, (elapsedTime/stateLength));
                Movement?.SetVelocityX(lerpedAmount * xInput);
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
        startScarfUp = true;
        player.StopMovingState.stopScarfUp = true;
    }
}
