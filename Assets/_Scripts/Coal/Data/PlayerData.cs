using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerData", menuName ="Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int amountOfJumps = 1;

    [Header("Wall Jump State")]
    public float wallJumpVelocity = 20;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);

    [Header("Ledge Climb State")]
    public Vector2 startOffset;
    public Vector2 stopOffset;

    [Header("In Air State")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("Wall Slide/Hold State")]
    public float wallSlideVelocity = 2f;
    public float wallSlideTime = 0.6f;
    public float wallHoldVelocity = 0f;

    [Header("Super Dash State")]
    public float superDashCooldown = 0.5f;
    public float maxHoldTime = 1f;
    public float holdTimeScale = 0.25f;
    public float superDashTime = 0.2f;
    public float superDashVelocity = 30f;
    public float drag = 10f;
    public float superDashEndYMultiplier = 0.2f;
    public float superDashDistBetweenAfterImages = 0.5f;

    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float dashTime = 0.2f;
    public float dashVelocity = 30f;
    public float dashDistBetweenAfterImages = 0.5f;
    public float dashFlipCoyoteTime = 0.2f;

    [Header("Weapon Charge State")]
    public float weaponCoolOffTime = 5f;
    public float alphaChangeCooldown = 0.1f;
    public float alphaChangeAmount = 0.05f;
    public float maxDecreasedAlphaForCompleteTheCharge = 0.2f;

    public float[] coalSwordGlowLightsChangeAmounts;

    [Header("Attack Stuff")]

    public float maxHoldTimeForAttack = 0.2f;
}
