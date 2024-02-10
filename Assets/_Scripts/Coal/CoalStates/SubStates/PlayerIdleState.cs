using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(player.shouldFreeze)
        {
            Movement?.SetVelocityZero();
        } 
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
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
            stateMachine.ChangeState(player.StartMovingState);
        }
        else if(!isGrounded && !isOnSlope)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else
        {
            Movement?.SetVelocityX(0f);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
