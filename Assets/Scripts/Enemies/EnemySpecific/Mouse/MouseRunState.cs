using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRunState : RunState
{
    private Mouse mouse;
    public MouseRunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunState stateData, Mouse mouse) : base(entity, stateMachine, animBoolName, stateData)
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
        entity.SetVelocity(stateData.runSpeed);
        if (isDetectingLedge || isDetectingWall)
        {
            mouse.Flip();
            //TODO:
        }
        if (entity.CheckPlayerInMaxAgroRange())
        {
            stateMachine.ChangeState(mouse.rangedAttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
