using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField] protected SO_WeaponData weaponData;
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

        if(attackCounter >= weaponData.amountOfAttacks - 5)
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

        attackCounter = weaponData.amountOfAttacks - 1;

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponHeavy()
    {
        gameObject.SetActive(true);

        attackCounter = weaponData.amountOfAttacks - 2;

        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void EnterWeaponMove()
    {
        gameObject.SetActive(true);

        if((attackCounter >= weaponData.amountOfAttacks - 2) || (attackCounter <= 2))
        {
            attackCounter = 3;
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