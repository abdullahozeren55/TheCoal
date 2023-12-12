using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newLookForPlayerStateData", menuName = "Data/State Data/Look For Player State Data")]
public class D_LookForPlayerState : ScriptableObject
{
    public int minAmountOfTurns = 1;
    public int maxAmountOfTurns = 4;

    public float timeBetweenTurns = 0.5f;
}
