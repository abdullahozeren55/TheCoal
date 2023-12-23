using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected ParticleManager ParticleManager => particleManager ??= core.GetCoreComponent<ParticleManager>();
    private ParticleManager particleManager;
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
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

    public override void TriggerAttack()
    {
        base.TriggerAttack();

    }
    
    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
