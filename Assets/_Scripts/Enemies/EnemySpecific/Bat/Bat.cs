using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bat : Entity, IDamageable, IKnockbackable
{
    public bool shouldHalfAwake;
    public bool shouldWakeUp;

    public Bat_IdleState IdleState { get; private set; }
    public Bat_SleepState SleepState { get; private set; }

    public override void Awake()
    {
        base.Awake();

        shouldHalfAwake = false;
        shouldWakeUp = false;

        IdleState = new Bat_IdleState(this, stateMachine, "idle", entityData, this);
        SleepState = new Bat_SleepState(this, stateMachine, "sleepState", entityData, this);
    }

    private void Start()
    {
        stateMachine.Initialize(SleepState);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        throw new System.NotImplementedException();
    }
}
