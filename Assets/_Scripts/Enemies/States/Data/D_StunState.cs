using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State Data")]
public class D_StunState : ScriptableObject
{

    public float stunKnockbackTime = 0.2f;
    public float stunKnockbackSpeed = 20f;

    public Vector2 stunKnockbackAngle;



}
