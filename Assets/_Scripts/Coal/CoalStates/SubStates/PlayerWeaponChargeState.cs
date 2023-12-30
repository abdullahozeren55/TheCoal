using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponChargeState : PlayerState
{
    public bool weaponCharged;
    public bool shouldTurnBackToNormalWeapon;

    private bool enteredStateWithPrimaryAttackButton;
    private bool enteredStateWithSecondaryAttackButton;
    public bool shouldStartCharging;
    private bool shouldFinishCharging;

    private float elapsedTime;
    private float newAlpha;
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

        elapsedTime = 0f;

        shouldStartCharging = false;
        shouldFinishCharging = false;

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

        shouldStartCharging = false;

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
        else if(shouldStartCharging)
        {
            CoroutineRunner.instance.StartCoroutine(DecreaseNormalAlpha());
        }

        if ((enteredStateWithPrimaryAttackButton && player.InputHandler.AttackInputsStop[(int)CombatInputs.primary]) || (enteredStateWithSecondaryAttackButton && player.InputHandler.AttackInputsStop[(int)CombatInputs.secondary]))
        {
            player.Anim.SetBool("chargeStateExiting", true);
            player.EyesAnim.SetBool("chargeStateExiting", true);
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool("chargeStateExiting", true);
            }
            if(player.coalSwordGlow.activeSelf)
            {
                player.CoalSwordGlowAnim.SetBool("chargeStateExiting", true);
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

    private IEnumerator DecreaseNormalAlpha()
    {
        Color currentColor = player.coalSwordSR.color;
        newAlpha = player.coalSwordSR.color.a;
        float startingAlpha = newAlpha;
        
        while(newAlpha > 0)
        {
            elapsedTime += Time.deltaTime;

            newAlpha = Mathf.Lerp(startingAlpha, 0f, elapsedTime/(playerData.timeForFirstCharge * 40f));
            player.coalSwordSR.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        if(player.currentWeapon == 0)
        {
            player.AttackState.SetWeapon(player.Inventory.weapons[1]);
            player.currentWeapon = 1;
                
            weaponCharged = true;
            shouldTurnBackToNormalWeapon = false;
            player.AttackState.lastAttackTime = Time.time;
        } 
        
    }

    public void SetWeaponAlpha(float alpha)
    {
        player.coalSwordSR.color = new Color(255f, 255f, 255f, alpha);
    }
}
