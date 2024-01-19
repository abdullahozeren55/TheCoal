using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_FreezeState : IdleState
{
    private Golem enemy;
    private bool isInstantiated;
    public Golem_FreezeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Golem enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        isInstantiated = false;

        if(enemy.startFacingDirection == -1)
        {
            Movement.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isInstantiated && isGrounded)
        {
            ParticleManager.StartParticles(enemy.groundedDustParticle, enemy.groundedDustPoint.position, Quaternion.identity);
            isInstantiated = true;
        }

        if(!enemy.shouldFreeze)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }
        
}

