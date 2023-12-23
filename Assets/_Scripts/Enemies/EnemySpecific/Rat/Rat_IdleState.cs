using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_IdleState : IdleState
{
    private Rat enemy;
    public Rat_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
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
        if(isIdleTimeOver)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
        else
        {
            Movement?.SetVelocityZero();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
