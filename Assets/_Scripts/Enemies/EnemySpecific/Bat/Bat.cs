using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Entity, IDamageable, IKnockbackable
{
    public void Knockback(float strength, Vector2 angle, int direction)
    {
        throw new System.NotImplementedException();
    }
}
