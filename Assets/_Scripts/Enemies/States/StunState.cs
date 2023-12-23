using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{

    protected Stats Stats => stats ??= core.GetCoreComponent<Stats>();
    private Stats stats;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();

    private Movement movement;

    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();
    private ParticleManager particleManager;
    protected bool isStunned;
    protected bool isGrounded;
    protected bool isOnSlope;
    protected bool isDetectingLedge;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInCloseRangeAction;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData) : base(entity, stateMachine, animBoolName, entityData)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isOnSlope = CollisionSenses.Slope;
            isDetectingLedge = CollisionSenses.LedgeVertical;
        }

        isPlayerInCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        isStunned = Stats.Stun();
    }

}
