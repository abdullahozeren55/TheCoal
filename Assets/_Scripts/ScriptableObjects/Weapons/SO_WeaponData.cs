using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon Data")]
public class SO_WeaponData : ScriptableObject
{
    public int amountOfNormalAttacks { get; protected set; }
    public int amountOfHeavyAttacks { get; protected set; }
    public int amountOfMoveAttacks { get; protected set; }
    public int amountOfMoveHeavyAttacks { get; protected set; }
    public int amountOfAirAttacks { get; protected set; }
    public int amountOfAttacks { get; protected set; }
    public float[] movementSpeed { get; protected set; }
    public Vector2[] movementAngle { get; protected set; }

    public GameObject hitParticle { get; protected set; }


    public WeaponScreenShakeDetails[] shakeDetails { get; protected set; }
    public AudioClip[] weaponNormalSoundFX;
    public AudioClip weaponInAirSoundFX;
    public AudioClip weaponHeavySoundFX;
    public AudioClip[] weaponMoveSoundFX;
    public AudioClip weaponMoveHeavySoundFX;
}
