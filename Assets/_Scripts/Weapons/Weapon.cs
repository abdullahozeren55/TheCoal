using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Weapon : MonoBehaviour
{

    [SerializeField] protected SO_WeaponData weaponData;
    [SerializeField] protected CinemachineImpulseSource impulseSource;
    protected Animator baseAnimator;
    protected Animator weaponAnimator;

    protected PlayerAttackState state;

    protected Movement movement;

    protected int attackCounter;

    protected virtual void Awake()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if(attackCounter >= weaponData.amountOfNormalAttacks)
        {
            attackCounter = 0;
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponInAir()
    {
        gameObject.SetActive(true);

        if(attackCounter < weaponData.amountOfAttacks - weaponData.amountOfAirAttacks || attackCounter >= weaponData.amountOfAttacks)
        {
            attackCounter = weaponData.amountOfAttacks - weaponData.amountOfAirAttacks;
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponHeavy()
    {
        gameObject.SetActive(true);

        if(attackCounter < weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks + weaponData.amountOfMoveAttacks + weaponData.amountOfHeavyAttacks) || attackCounter >= weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks + weaponData.amountOfMoveAttacks))
        {
            attackCounter = weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks + weaponData.amountOfMoveAttacks + weaponData.amountOfHeavyAttacks);
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponMove()
    {
        gameObject.SetActive(true);

        if(attackCounter < weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks + weaponData.amountOfMoveAttacks) || attackCounter >= weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks))
        {
            attackCounter = weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks + weaponData.amountOfMoveAttacks);
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponMoveHeavy()
    {
        gameObject.SetActive(true);

        if(attackCounter < weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks) || attackCounter >= weaponData.amountOfAttacks - weaponData.amountOfAirAttacks)
        {
            attackCounter = weaponData.amountOfAttacks - (weaponData.amountOfAirAttacks + weaponData.amountOfMoveHeavyAttacks);
        }

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void ExitWeapon()
    {
        baseAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);

        attackCounter++;

        gameObject.SetActive(false);
    }

    public void InitializeWeapon(PlayerAttackState state, Movement movement)
    {
        this.state = state;
        this.movement = movement;
    }

    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter], weaponData.movementAngle[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f, Vector2.zero);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }

    
}
