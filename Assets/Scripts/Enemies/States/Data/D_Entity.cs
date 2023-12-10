using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float maxHealth = 30f;

    public float maxStunResistance = 100f;

    public float minStunTime = 0.2f;

    public float timeBeforeStunResStartsRegain = 1f;

    public float stunResRegCooldown = 0.2f;
    public float minStunResistanceRegain = 3f;
    public float maxStunResistanceRegain = 15f;

    public float damageHopVelocity = 3f;

    public Vector2 damageHopAngle;

    public float wallCheckDistance = 0.3f;
    public float groundCheckDistance = 0.3f;
    public float minAgroDistance = 1f;
    public float maxAgroDistance = 4f;
    public float closeRangeActionDistance = 1f;

    public GameObject hitParticle;

    public LayerMask whatIsWall;
    public LayerMask whatIsLedge;
    public LayerMask whatIsPlayer;

    public LayerMask whatIsGround;
}
