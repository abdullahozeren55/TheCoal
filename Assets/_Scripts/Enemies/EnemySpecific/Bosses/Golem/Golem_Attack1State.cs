using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Attack1State : MeleeAttackState
{
    private Golem enemy;
    public Golem_Attack1State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, Golem enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAttackFinished)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, entityData.attackDetails[1].attackRadius, entityData.attackDetails[1].whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.Damage(entityData.attackDetails[1].hpDamageAmount);
            }

            if(entityData.attackDetails[1].canKnockback)
            {
                IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            

                if(knockbackable != null)
                {
                    knockbackable.Knockback(entityData.attackDetails[1].knockbackStrength, entityData.attackDetails[1].knockbackAngle, Movement.FacingDirection);
                }
            }

            
        }
    }
}
