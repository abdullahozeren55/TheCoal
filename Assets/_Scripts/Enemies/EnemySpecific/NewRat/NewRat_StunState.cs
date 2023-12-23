using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_StunState : StunState
{
    private NewRat enemy;
    public NewRat_StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Movement.SetVelocityX(0f);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isStunned)
        {
            if(isPlayerInCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.NormalAttackState);
            }
            else if(isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.ChargeState);
            }
            else
            {
                stateMachine.ChangeState(enemy.LookForPlayerState);
            }
        }
        else if(isGrounded && Time.time >= enemy.lastKnockbackedTime + entityData.dragTime)
        {
            Movement.SetVelocityX(0f);
        }
    }
}
