using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponChargeState : PlayerState
{

    private bool enteredStateWithPrimaryAttackButton;
    private bool enteredStateWithSecondaryAttackButton;
    public bool isWeaponCharged;
    private bool isCharging;
    public bool shouldStartCharging;
    public bool chargeFailed;

    private float lastAlphaDecreaseTime;
    private float lastAlphaIncreaseTime;
    private float currentAlpha;
    //private Color[] oldColors1;
    //private Color[] oldColors2;

    //private Color[] newColors1;
    //private Color[] newColors2;
    public PlayerWeaponChargeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        //oldColors1 = new Color[player.coalSwordGlowMats.Length];
        //oldColors2 = new Color[player.coalSwordGlowMats.Length];

        //newColors1 = new Color[player.coalSwordGlowMats.Length];
        //newColors2 = new Color[player.coalSwordGlowMats.Length];
    }

    public override void Enter()
    {
        base.Enter();

        shouldStartCharging = false;

        if(player.currentWeapon == 0 || player.currentWeapon == 1)
        {
            player.Anim.SetBool("chargeStartWithSword", true);
            player.EyesAnim.SetBool("chargeStartWithSword", true);
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool("chargeStartWithSword", true);
            }

            if(player.coalSwordGlow.activeSelf)
            {
                player.CoalSwordGlowAnim.SetBool("chargeStartWithSword", true);
            }
        }
        else
        {
            player.Anim.SetBool("chargeStartWithSword", false);
            player.EyesAnim.SetBool("chargeStartWithSword", false);
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool("chargeStartWithSword", false);
            }

            if(player.coalSwordGlow.activeSelf)
            {
                player.CoalSwordGlowAnim.SetBool("chargeStartWithSword", false);
            }
        }

        /*for (int i = 0; i < player.coalSwordGlowMats.Length; i++)
        {
            oldColors1[i] = player.coalSwordGlowMats[i].GetColor("_GlowColor");
            oldColors2[i] = player.coalSwordGlowMats[i].GetColor("_GlowColor2");

            newColors1[i] = oldColors1[i] * 10f;
            newColors2[i] = oldColors2[i] * 5f;

            player.coalSwordGlowMats[i].SetColor("_GlowColor", newColors1[i]);
            player.coalSwordGlowMats[i].SetColor("_GlowColor2", newColors2[i]);
        }*/
    }

    public override void Exit()
    {
        base.Exit();

        enteredStateWithPrimaryAttackButton = false;
        enteredStateWithSecondaryAttackButton = false;

        player.Anim.SetBool("chargeStateExiting", false);
        player.EyesAnim.SetBool("chargeStateExiting", false);
        if(player.coalSword.activeSelf)
        {
            player.CoalSwordAnim.SetBool("chargeStateExiting", false);
        }

        if(player.coalSwordGlow.activeSelf)
        {
            player.CoalSwordGlowAnim.SetBool("chargeStateExiting", false);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        if(isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        
        if(shouldStartCharging)
        {
            SetWeaponOnFire(playerData.alphaChangeCooldown, playerData.alphaChangeAmount);
        }

        if(isCharging && player.coalSwordSR.color.a <= 0f)
        {
            lastAlphaDecreaseTime = Time.time;
        }

        if ((enteredStateWithPrimaryAttackButton && player.InputHandler.AttackInputsStop[(int)CombatInputs.primary]) || (enteredStateWithSecondaryAttackButton && player.InputHandler.AttackInputsStop[(int)CombatInputs.secondary]))
        {
            player.Anim.SetBool("chargeStateExiting", true);
            player.EyesAnim.SetBool("chargeStateExiting", true);
            isCharging = false;

            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool("chargeStateExiting", true);
            }
            if(player.coalSwordGlow.activeSelf)
            {
                player.CoalSwordGlowAnim.SetBool("chargeStateExiting", true);
            }

            if(player.coalSwordSR.color.a > playerData.maxDecreasedAlphaForCompleteTheCharge)
            {
                shouldStartCharging = false;
                chargeFailed = true;
            }
        }
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        shouldStartCharging = true;
    }

    public void EnteredWithPrimaryAttackButton()
    {
        enteredStateWithPrimaryAttackButton = true;
    }

    public void EnteredWithSecondaryAttackButton()
    {
        enteredStateWithSecondaryAttackButton = true;
    }

    public void CheckWeaponCoolOff()
    {
        if(!isCharging && Time.time >= lastAlphaDecreaseTime + playerData.weaponCoolOffTime && Time.time >= player.AttackState.lastAttackTime + playerData.weaponCoolOffTime)
        {
            CoolWeaponOff(playerData.alphaChangeCooldown, playerData.alphaChangeAmount);
        }
    }

    public void CoolWeaponOff(float cooldown, float increaseAmount)
    {
        currentAlpha = player.coalSwordSR.color.a;
        if(Time.time >= lastAlphaIncreaseTime + cooldown)
        {
            currentAlpha += increaseAmount;
            player.coalSwordSR.color = new Color(255f, 255f, 255f, currentAlpha);
            lastAlphaIncreaseTime = Time.time;

            if(currentAlpha >= 1f && stateMachine.CurrentState != player.AttackState)
            {
                player.AttackState.SetWeapon(player.Inventory.weapons[0]);
                player.currentWeapon = 0;
                chargeFailed = false;
                isWeaponCharged = false;
            }
        }
    }

    public void SetWeaponOnFire(float cooldown, float decreaseAmount)
    {
        if(stateMachine.CurrentState == player.WeaponChargeState)
        {
            isCharging = true;
        }
        currentAlpha = player.coalSwordSR.color.a;
        chargeFailed = false;

        if(Time.time >= lastAlphaDecreaseTime + cooldown)
        {
            currentAlpha -= decreaseAmount;
            player.coalSwordSR.color = new Color(255f, 255f, 255f, currentAlpha);
            lastAlphaDecreaseTime = Time.time;

            if(currentAlpha <= 0f)
            {
                player.AttackState.SetWeapon(player.Inventory.weapons[1]);
                player.currentWeapon = 1;
                shouldStartCharging = false;
                isWeaponCharged = true;
            }
        }
    }

}
