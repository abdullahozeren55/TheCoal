using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{

    private Weapon weapon;

    public float lastAttackTime;

    public bool hitEnemy;

    private float velocityToSet;
    private bool setVelocity;
    private bool shouldCheckFlip;
    private bool attackIsHeavy;

    private bool isMoving;

    public ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();
    private ParticleManager particleManager;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        setVelocity = false;
        hitEnemy = false;

        player.eyeLights.SetActive(false);

        if(isGrounded || isOnSlope)
        {
            if(!isMoving)
            {
                if(attackIsHeavy)
                {
                    weapon.EnterWeaponHeavy();
                }
                else
                {
                    weapon.EnterWeapon();
                }  
            }
            else
            {
                if(attackIsHeavy)
                {
                    weapon.EnterWeaponMoveHeavy();
                }
                else
                {
                    weapon.EnterWeaponMove();
                }    
            }
        }
        else
        {
            weapon.EnterWeaponInAir();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.eyeLights.SetActive(true);

        if(hitEnemy)
        {
            lastAttackTime = Time.time;
        }


        weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if(xInput == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

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
                if(xInput == 0)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.MoveState);
                }
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

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    
}
