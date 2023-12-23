using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRat_JumpAttackState : MeleeAttackState
{
    private NewRat enemy;
    private bool isFlipDone;
    public NewRat_JumpAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, NewRat enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        Movement?.SetVelocity(entityData.attackJumpVelocity, entityData.attackJumpAngle, Movement.FacingDirection);
        isFlipDone = false;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.IdleState.lastAttackTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGrounded && isAttackFinished)
        {
            if(!isPlayerInMaxAgroRange && !isFlipDone)
            {
                Movement?.Flip();
                isFlipDone = true;
            }
            
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

    public override void FinishAttack()
    {
        base.FinishAttack();
    }
}
