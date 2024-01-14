using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{

    [Header("Detect Player Stuff")]
    public float minAgroDistance = 1f;
    public float maxAgroDistance = 4f;
    public float closeRangeActionDistance = 1f;
    public float shockedTime = 1f;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsLineCastCanHit;

    [Header("Idle State")]
    public float minIdleTime = 0.5f;
    public float maxIdleTime = 2f;

    [Header("Move State")]
    public float movementSpeed = 3f;
    public float ledgeDetectionCooldown = 1f;

    [Header("Player Detected State")]
    public float longRangeActionTime = 1f;

    [Header("Charge State")]
    public float chargeSpeed = 10f;
    public float chargeTime = 2f;

    [Header("Look For Player State")]
    public int minAmountOfTurns = 1;
    public int maxAmountOfTurns = 4;

    public float timeBetweenTurns = 0.5f;

    [Header ("Melee Attack State")]
    public EnemyMeleeAttackDetails[] attackDetails;
    public float attackJumpVelocity = 10f;
    public Vector2 attackJumpAngle;
    public float attackNormalVelocity = 3f;
    public Vector2 attackNormalAngle;
    public float attackNormalCooldown = 1f;

    [Header("Stun State")]
    public float poiseRegCooldown = 0.3f;
    public float poiseRegStartTimeAfterDamaged = 2f;
    public float minPoiseRegAmount = 1f;
    public float maxPoiseRegAmount = 30f;

    public float dragTime = 0.5f;

    [Header("Hit State")]
    public float knockBackMultiplier = 1f;
}
