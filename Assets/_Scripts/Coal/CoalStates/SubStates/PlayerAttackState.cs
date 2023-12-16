using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{

    private Weapon weapon;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;

    private int xInput;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;

        weapon.EnterWeapon();
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
            stateMachine.ChangeState(player.IdleState);
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

    public void SetPlayerVelocity(float velocity)
    {
        Movement?.SetVelocityX(velocity * Movement.FacingDirection);

        velocityToSet = velocity;
        setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        shouldCheckFlip = value;
    }

    
}
