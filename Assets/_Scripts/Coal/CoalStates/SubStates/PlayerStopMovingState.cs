using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStopMovingState : PlayerGroundedState
{

    public bool stopScarfUp;
    public PlayerStopMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocityZero();

        stopScarfUp = true;
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
            Movement?.CheckIfShouldFlip(xInput);

        
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
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
        else if(xInput != 0 && (!isTouchingWall || xInput != Movement?.FacingDirection))
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
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else if(isAnimationFinished && xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            Movement?.SetVelocityZero();
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
    }
}
