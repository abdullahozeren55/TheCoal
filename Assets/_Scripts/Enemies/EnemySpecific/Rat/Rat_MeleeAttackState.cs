using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_MeleeAttackState : MeleeAttackState
{
    private Rat enemy;
    public Rat_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttackState stateData, Rat enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Movement?.SetVelocityX(5f * Movement.FacingDirection);
        Movement?.SetVelocityY(10f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAnimationFinished)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
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
