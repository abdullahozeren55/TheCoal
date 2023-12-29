using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    public string attackName;
    public float movementSpeed;

    public Vector2 movementAngle;
    public float damageAmount;

    public float poiseDamageAmount;

    public float knockbackStrength;
    public Vector2 knockbackAngle;
}

[System.Serializable]
public struct WeaponScreenShakeDetails
{
    public string attackName;
    [Header("Impulse Source Settings")]
    public float impulseTime;
    public float impulseForce;
    public Vector3 defaultVelocity;

    [Header("Impulse Listener Settings")]
    public float listenerAmplitude;
    public float listenerFrequency;
    public float listenerDuration;
}

[System.Serializable]
public struct EnemyMeleeAttackDetails
{
    public string attackName;
    public float hpDamageAmount;
    public float poiseDamageAmount;
    public float attackRadius;
    public bool canKnockback;
    public float knockbackStrength;
    public Vector2 knockbackAngle;
    public LayerMask whatIsDamageable;
}