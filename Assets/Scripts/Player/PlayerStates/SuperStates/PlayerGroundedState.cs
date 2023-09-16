using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{

    protected int xInput;

    protected bool jumpInput;
    private bool isGrounded;
    protected bool dashInput;

    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        playerData.isJumping = false;
        playerData.wallJumpCombo = false;

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
        dashInput = player.InputHandler.DashInput;

        if(player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.PrimaryAttackState);
        }
        else if(player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.SecondaryAttackState);
        }
        else if (jumpInput && player.JumpState.CheckIfCanJump())
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else if(!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }
        else if(dashInput && player.DashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


}
