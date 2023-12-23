using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopMovingState : PlayerGroundedState
{

    public bool stopScarfUp;
    public PlayerStopMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        Movement?.CheckIfShouldFlip(xInput);

        
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
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
