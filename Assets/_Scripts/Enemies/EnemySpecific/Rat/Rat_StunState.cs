using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_StunState : StunState
{
    private Rat enemy;
    public Rat_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Rat enemy) : base(entity, stateMachine, animBoolName, entityData)
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

        
        if (!isStunned)
        {
            stateMachine.ChangeState(enemy.LookForPlayerState);
        }
        else if(isGrounded && Time.time >= enemy.lastKnockbackedTime + entityData.dragTime)
        {
            Movement.SetVelocityX(0f);
        }
    }
}
