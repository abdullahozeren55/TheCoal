using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle State Data")]
public class D_IdleState : ScriptableObject
{
    public float minIdleTime = 0.5f;
    public float maxIdleTime = 2f;
}
