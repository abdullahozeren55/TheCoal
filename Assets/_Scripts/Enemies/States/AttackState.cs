using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{

    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;

    protected CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;
    protected Transform attackPosition;
    protected bool isPlayerInMinAgroRange;
    protected bool isPlayerInMaxAgroRange;
    protected bool isPlayerInCloseRangeAction;

    protected bool isAttackFinished;

    protected bool isGrounded;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.attackPosition = attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        isPlayerInCloseRangeAction = entity.CheckPlayerInCloseRangeAction();

        if(CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();

        entity.atsm.attackState = this;

        isAttackFinished = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {

    }
    
    public virtual void FinishAttack()
    {
        isAttackFinished = true;
    }
}
