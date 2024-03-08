using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public bool landScarfUp;
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        if(player.isInStatueLevel)
        {
            SoundFXManager.instance.PlaySoundFXClip(playerData.landOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
        }

        if(Movement?.CurrentVelocity.x != 0f)
        {
            Movement?.SetVelocityX(0f);
        }
        landScarfUp = true;
    }

    public override void Exit()
    {
        base.Exit();

        if(landScarfUp)
        {
            landScarfUp = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(player.shouldLandFreeze)
        {
            if(isAnimationFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
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
        else if (xInput == Movement?.FacingDirection && !isTouchingWall)
        {
            playerData.walkOnGrassSoundFXTimer = playerData.walkOnGrassSoundFXCooldown + 0.1f;

            if(landScarfUp)
            {
                stateMachine.ChangeState(player.MoveState);
            }
            else
            {
                stateMachine.ChangeState(player.StartMovingState);
            }
        }
        else if(xInput != 0 && xInput != Movement?.FacingDirection)
        {
            stateMachine.ChangeState(player.FlipState);
        }
        else if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if(!isGrounded && !isOnSlope)
        {
            stateMachine.ChangeState(player.InAirState);
        }      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        landScarfUp = false;
    }
}
