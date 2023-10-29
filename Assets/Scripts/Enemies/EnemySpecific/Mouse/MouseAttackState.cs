using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAttackState : MeleeAttackState
{
    private Mouse mouse;

    public MouseAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Mouse mouse) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.mouse = mouse;
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
}
