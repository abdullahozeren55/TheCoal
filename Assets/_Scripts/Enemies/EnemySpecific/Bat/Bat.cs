using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UIElements;

public class Bat : Entity, IDamageable, IKnockbackable
{
    public bool shouldHalfAwake;
    public bool shouldWakeUp;

    public Seeker seeker;

    public float nextWayPointDistance = 3f;
    public float dashCooldown = 4f;

    public Transform playerHead;
    public Transform playerFeet;

    [SerializeField] private Transform meleeAttackPosition;

    public Bat_FollowPlayerState FollowPlayerState { get; private set; }
    public Bat_SleepState SleepState { get; private set; }
    public Bat_GettingReadyToDashState GettingReadyToDashState { get; private set; }
    public Bat_DashState DashState { get; private set; }
    public Bat_FinishingDashState FinishingDashState { get; private set; }
    public Bat_HitState HitState { get; private set; }

    public override void Awake()
    {
        base.Awake();

        seeker = GetComponent<Seeker>();

        shouldHalfAwake = false;
        shouldWakeUp = false;

        FollowPlayerState = new Bat_FollowPlayerState(this, stateMachine, "idle", entityData, this);
        SleepState = new Bat_SleepState(this, stateMachine, "sleepState", entityData, this);
        GettingReadyToDashState = new Bat_GettingReadyToDashState(this, stateMachine, "gettingReadyToDash", entityData, this);
        DashState = new Bat_DashState(this, stateMachine, "dash", entityData, meleeAttackPosition, this);
        FinishingDashState = new Bat_FinishingDashState(this, stateMachine, "finishingDash", entityData, this);
        HitState = new Bat_HitState(this, stateMachine, "hit", entityData, this);
    }

    private void Start()
    {
        stateMachine.Initialize(SleepState);
    }

    public void Damage(float amount)
    {
        Stats?.DecreaseHealth(amount);
        //ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength * entityData.knockBackMultiplier, new Vector2(angle.x, -angle.y), direction);

        if(stateMachine.currentState != HitState)
        {
            stateMachine.ChangeState(HitState);
        }
    }

    public void CheckFlip()
    {
        if((transform.position.x < player.transform.position.x && Movement.FacingDirection == -1) || (transform.position.x >= player.transform.position.x && Movement.FacingDirection == 1))
        {
            Movement.Flip();
        }
    }
}
