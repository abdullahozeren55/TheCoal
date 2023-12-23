using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_NormalAttackState : MeleeAttackState
{
    private NewRat enemy;
    public NewRat_NormalAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocity(entityData.attackNormalVelocity, entityData.attackNormalAngle, Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.IdleState.lastAttackTime = Time.time;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAttackFinished)
        {
            if(isPlayerInCloseRangeAction)
            {
                stateMachine.ChangeState(enemy.IdleState);
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
