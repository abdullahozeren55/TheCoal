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

        isGrounded = core.CollisionSenses.Ground;
        isTouchingWall = core.CollisionSenses.WallFront;
        isTouchingWallBack = core.CollisionSenses.WallBack;
    }

    public override void Enter()
    {
        base.Enter();
        player.eyesAnim.SetBool("inAir", true);

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

        if (core.Movement.CurrentVelocity.y > 0.05f)
        {
            playerData.isJumping = true;
        }
        else if (core.Movement.CurrentVelocity.y <= 0.05f || isGrounded || jumpInputStop)
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
        else if (isGrounded && core.Movement.CurrentVelocity.y < 0.01f)
        {
            playerData.canStickWall = true;
            stateMachine.ChangeState(player.LandState);
        }
        else if (isTouchingWall && (xInput == core.Movement.FacingDirection || playerData.wallJumpCombo) && playerData.canStickWall && (core.Movement.CurrentVelocity.y <= 0f || playerData.wallJumpCombo))
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
            core.Movement.CheckIfShouldFlip(xInput);
            core.Movement.SetVelocityX(playerData.movementVelocity * xInput);
            player.Anim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
            player.eyesAnim.SetFloat("yVelocity", core.Movement.CurrentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(xInput));
            player.eyesAnim.SetFloat("xVelocity", Mathf.Abs(xInput));

        }
    }

    private void CheckJumpMultiplier()
    {
        if (playerData.isJumping)
        {
            if (jumpInputStop)
            {
                core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
