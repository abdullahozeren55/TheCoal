using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRangedAttackState : RangedAttackState
{
    Mouse mouse;
    public MouseRangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_RangedAttackState stateData, Mouse mouse) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.mouse = mouse;
    }

    public override void Enter()
    {
        base.Enter();
        mouse.SetVelocityY(mouse.entityData.jumpVelocity);
        stateMachine.ChangeState(mouse.runState);
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
