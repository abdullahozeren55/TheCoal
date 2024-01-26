using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartMovingState : PlayerGroundedState
{

    public bool startScarfUp;
    public PlayerStartMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityX(playerData.movementVelocity * xInput);

        startScarfUp = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.CheckIfShouldFlip(xInput);

        Movement?.SetVelocityX(playerData.movementVelocity * xInput);

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
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if(xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
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
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isAnimationFinished && xInput != 0 && (!isTouchingWall || xInput != Movement?.FacingDirection))
        {
            stateMachine.ChangeState(player.MoveState);
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
    }
}
