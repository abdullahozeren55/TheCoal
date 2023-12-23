using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat_MeleeAttackState : MeleeAttackState
{
    private Rat enemy;
    public Rat_MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, Rat enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
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
        Movement?.SetVelocity(entityData.attackJumpVelocity, entityData.attackJumpAngle, Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAttackFinished)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    /*public override void TriggerAttack()
    {
        base.TriggerAttack();

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackPosition.position, entityData.attackDetails.attackRadius, entityData.attackDetails.whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.Damage(entityData.attackDetails.hpDamageAmount);
            }

            if(entityData.attackDetails.canKnockback)
            {
                IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            

                if(knockbackable != null)
                {
                    knockbackable.Knockback(entityData.attackDetails.knockbackStrength, entityData.attackDetails.knockbackAngle, Movement.FacingDirection);
                }
            }

            
        }
    }*/
    
    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
