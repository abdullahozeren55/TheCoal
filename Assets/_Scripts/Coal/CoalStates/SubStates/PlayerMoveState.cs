using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
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

        if(player.transform.parent == null)
        {
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);
        }
        else
        {
            Movement?.SetVelocityXY(playerData.movementVelocity * xInput + player.transform.parent.GetComponent<Rigidbody2D>().velocityX, player.transform.parent.GetComponent<Rigidbody2D>().velocityY);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        Movement?.CheckIfShouldFlip(xInput);

		if(player.transform.parent == null)
        {
            Movement?.SetVelocityX(playerData.movementVelocity * xInput);
        }
        else
        {
            Movement?.SetVelocityXY(playerData.movementVelocity * xInput + player.transform.parent.GetComponent<Rigidbody2D>().velocityX, player.transform.parent.GetComponent<Rigidbody2D>().velocityY);
        }

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
            stateMachine.ChangeState(player.StopMovingState);
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
}
