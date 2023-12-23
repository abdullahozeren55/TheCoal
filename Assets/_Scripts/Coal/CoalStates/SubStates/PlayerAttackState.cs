using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{

    private Weapon weapon;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    private bool attackIsHeavy;

    private int xInput;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;

        if(!isGrounded && !isOnSlope)
        {
            weapon.EnterWeaponInAir();
        }
        else if(attackIsHeavy)
        {
            weapon.EnterWeaponHeavy();
        }
        else if((isGrounded || isOnSlope) && xInput != 0)
        {
            weapon.EnterWeaponMove();
        }
        else
        {
            weapon.EnterWeapon();
        }
        
    }

    public override void Exit()
    {
        base.Exit();

        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if(shouldCheckFlip)
        {
            Movement?.CheckIfShouldFlip(xInput);
        }

        if(setVelocity)
        {
            Movement?.SetVelocityX(velocityToSet * Movement.FacingDirection);
        }

        if(isAbilityDone)
        {
            if(isGrounded || isOnSlope)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
            
        }
    }


    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
        weapon.InitializeWeapon(this, Movement);
    }

    public void SetPlayerVelocity(float velocity, Vector2 angle)
    {
        Movement?.SetVelocity(velocity, angle, Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    public void SetAttackIsHeavy(bool value)
    {
        attackIsHeavy = value;
    }

    
}
