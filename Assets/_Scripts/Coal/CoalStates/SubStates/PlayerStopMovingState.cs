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

        if(Movement?.CurrentVelocity.x != 0f)
        {
            Movement?.SetVelocityX(0f);
        }

        //stopScarfUp = true;
        //player.StartMovingState.startScarfUp = true;
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
                if(Movement?.CurrentVelocity.x != 0f)
                {
                    Movement?.SetVelocityX(0f);
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
