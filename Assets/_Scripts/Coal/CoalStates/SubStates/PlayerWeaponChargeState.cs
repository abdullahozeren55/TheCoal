using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponChargeState : PlayerState
{
    private Color[] oldColors1;
    private Color[] oldColors2;

    private Color[] newColors1;
    private Color[] newColors2;
    public PlayerWeaponChargeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        oldColors1 = new Color[player.coalSwordGlowMats.Length];
        oldColors2 = new Color[player.coalSwordGlowMats.Length];

        newColors1 = new Color[player.coalSwordGlowMats.Length];
        newColors2 = new Color[player.coalSwordGlowMats.Length];
    }

    public override void Enter()
    {
        base.Enter();

        for (int i = 0; i < player.coalSwordGlowMats.Length; i++)
        {
            oldColors1[i] = player.coalSwordGlowMats[i].GetColor("_GlowColor");
            oldColors2[i] = player.coalSwordGlowMats[i].GetColor("_GlowColor2");

            newColors1[i] = oldColors1[i] * 10f;
            newColors2[i] = oldColors2[i] * 5f;

            player.coalSwordGlowMats[i].SetColor("_GlowColor", newColors1[i]);
            player.coalSwordGlowMats[i].SetColor("_GlowColor2", newColors2[i]);
        }
    }
}
