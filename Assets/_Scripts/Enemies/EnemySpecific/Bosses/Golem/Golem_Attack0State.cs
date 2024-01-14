using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Attack0State : MeleeAttackState
{
    private Golem enemy;
    public Golem_Attack0State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, Golem enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
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

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, entityData.attackDetails[0].attackRadius, entityData.attackDetails[0].whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.Damage(entityData.attackDetails[0].hpDamageAmount);
            }

            if(entityData.attackDetails[0].canKnockback)
            {
                IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            

                if(knockbackable != null)
                {
                    knockbackable.Knockback(entityData.attackDetails[0].knockbackStrength, entityData.attackDetails[0].knockbackAngle, Movement.FacingDirection);
                }
            }

            
        }
    }
}
