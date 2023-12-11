using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{

    [Header("Move State")]
    public float movementVelocity = 10f;
    public bool canMove;
    public bool startMovingFinished;

    public bool startMovingHalfFinished;

    public bool stopMovingFinished;

    public bool stopMovingHalfFinished;

    //public float acceleration = 1f;

    //public float decceleration = -1f;

    //public float velPower = 1.1f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;
    public int maxAmountOfJumps = 1;
    public int amountOfJumpsLeft = 1;
    public float variableJumpHeightMultiplier = 0.5f;
    public bool canJump;
    public bool isJumping;

    [Header("Dash State")]
    public float dashCooldown = 0.5f;
    public float dashTime = 0.3f;
    public float dashVelocity = 30f;
    public float dashEndYMultiplier = 0.2f;

    public float distanceBetweenAfterImages = 0.5f;
    public bool canDash;
    public float lastDashTime = 0f;

    [Header("Attack State")]
    public bool isAttacking;
    public Transform attackTransform;
    public float attackRadius;
    public LayerMask whatIsDamageable;

    [Header("Wall Slide/Jump State")]
    public LayerMask whatIsWall;
    public float stickingWallTime = 0.5f;
    public float wallSlideVelocity = 0.5f;
    public bool canStickWall;
    public float wallJumpVelocity = 20f;
    public float wallJumpTime = 0.4f;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public bool wallJumpCombo;

}
