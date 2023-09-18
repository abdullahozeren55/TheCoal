using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{

    private int xInput;
    private bool isGrounded;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool dashInput;
    private bool isTouchingWall;
    private bool isTouchingWallBack;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingWall = player.CheckIfTouchingWall();
        isTouchingWallBack = player.CheckIfTouchingWallBack();
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        dashInput = player.InputHandler.DashInput;

        CheckJumpMultiplier();

        if (player.CurrentVelocity.y > 0.05f)
        {
            playerData.isJumping = true;
        }
        else if (player.CurrentVelocity.y <= 0.05f || isGrounded || jumpInputStop)
        {
            playerData.isJumping = false;
        }
        if (player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (isGrounded && player.CurrentVelocity.y < 0.01f)
        {
            playerData.canStickWall = true;
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && (xInput == player.FacingDirection || playerData.wallJumpCombo) && playerData.canStickWall && (player.CurrentVelocity.y <= 0f || playerData.wallJumpCombo))
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if (jumpInput && player.JumpState.CheckIfCanJump() && !playerData.isJumping)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if (dashInput && player.DashState.CheckIfCanDash())
        {
            playerData.canStickWall = true;
            stateMachine.ChangeState(player.DashState);
        }

        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));

        }
    }

    private void CheckJumpMultiplier()
    {
        if (playerData.isJumping)
        {
            if (jumpInputStop)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
