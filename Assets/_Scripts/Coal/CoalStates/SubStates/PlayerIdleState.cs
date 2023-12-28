using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    private bool canPrimaryAttack;
    private bool canSecondaryAttack;
    private bool canChargeWeapon;
    private bool lastAttackPressedTimeIsSet;

    private float lastPrimaryAttackPressedTime;
    private float lastSecondaryAttackPressedTime;
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityZero();

        canChargeWeapon = false;
        lastAttackPressedTimeIsSet = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.InputHandler.AttackInputs[(int)CombatInputs.primary])
        {
            if(!lastAttackPressedTimeIsSet)
            {
                lastPrimaryAttackPressedTime = Time.time;
                lastAttackPressedTimeIsSet = true;
            }
            
            if(Time.time < lastPrimaryAttackPressedTime + playerData.maxHoldTimeForAttack)
            {
                canPrimaryAttack = true;
            }
            else
            {
                canPrimaryAttack = false;
                canChargeWeapon = true;
            } 
        }
        else if(player.InputHandler.AttackInputs[(int)CombatInputs.secondary])
        {
            if(!lastAttackPressedTimeIsSet)
            {
                lastSecondaryAttackPressedTime = Time.time;
                lastAttackPressedTimeIsSet = true;
            }

            if(Time.time < lastSecondaryAttackPressedTime + playerData.maxHoldTimeForAttack)
            {
                canSecondaryAttack = true;
            }
            else
            {
                canSecondaryAttack = false;
                canChargeWeapon = true;
            } 
        }
        if (player.InputHandler.AttackInputsStop[(int)CombatInputs.primary] && canPrimaryAttack)
        {
            player.AttackState.SetAttackIsHeavy(false);
            stateMachine.ChangeState(player.AttackState);
            canPrimaryAttack = false;
        }
        else if (player.InputHandler.AttackInputsStop[(int)CombatInputs.secondary] && canSecondaryAttack)
        {
            player.AttackState.SetAttackIsHeavy(true);
            stateMachine.ChangeState(player.AttackState);
            canSecondaryAttack = false;
        }
        /*else if((player.InputHandler.AttackInputs[(int)CombatInputs.secondary] || player.InputHandler.AttackInputs[(int)CombatInputs.primary]) && canChargeWeapon)
        {
            stateMachine.ChangeState(player.WeaponChargeState);
        }*/
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
            player.InAirState.StartCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else
        {
            Movement?.SetVelocityZero();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
