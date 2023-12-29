using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName ="newAggressiveWeaponData", menuName ="Data/Weapon Data/Aggressive Weapon Data")]
public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField] private int normalAttackCount;
    [SerializeField] private int heavyAttackCount;
    [SerializeField] private int moveAttackCount;
    [SerializeField] private int moveHeavyAttackCount;
    [SerializeField] private int airAttackCount;
    [SerializeField] private WeaponAttackDetails[] attackDetails;
    [SerializeField] private WeaponScreenShakeDetails[] screenShakeDetails; 

    public WeaponAttackDetails[] AttackDetails { get => attackDetails; private set => attackDetails = value; }
    public WeaponScreenShakeDetails[] ScreenShakeDetails { get => screenShakeDetails; private set => screenShakeDetails = value; }
    public GameObject hitPart;

    private void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        movementAngle = new Vector2[amountOfAttacks];

        shakeDetails = new WeaponScreenShakeDetails[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed;
            movementAngle[i] = attackDetails[i].movementAngle;

            shakeDetails[i] = screenShakeDetails[i];
        }

        amountOfNormalAttacks = normalAttackCount;
        amountOfHeavyAttacks = heavyAttackCount;
        amountOfMoveAttacks = moveAttackCount;
        amountOfMoveHeavyAttacks = moveHeavyAttackCount;
        amountOfAirAttacks = airAttackCount;
        hitParticle = hitPart;
    }
}
