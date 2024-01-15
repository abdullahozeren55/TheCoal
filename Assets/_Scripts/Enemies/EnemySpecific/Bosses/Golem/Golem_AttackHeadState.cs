using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_AttackHeadState : MeleeAttackState
{
    private Golem enemy;

    private int spikeNumber;
    public Golem_AttackHeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Transform attackPosition, Golem enemy) : base(entity, stateMachine, animBoolName, entityData, attackPosition)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        SetRandomSpikeNumber();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAttackFinished)
        {
            stateMachine.ChangeState(enemy.IdleState);
        }
    }

    private void SetRandomSpikeNumber()
    {
        spikeNumber = Random.Range(1, 6);
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        enemy.headSpike.SetActive(true);
        
        switch (spikeNumber)
            {
                case 1:
                    enemy.headSpikeAnim.Play("Spikes1");
                    break;
                case 2:
                    enemy.headSpikeAnim.Play("Spikes2");
                    break;
                case 3:
                    enemy.headSpikeAnim.Play("Spikes3");
                    break;
                case 4:
                    enemy.headSpikeAnim.Play("Spikes4");
                    break;
                case 5:
                    enemy.headSpikeAnim.Play("Spikes5");
                    break;
            
                default:
                    enemy.headSpikeAnim.Play("Spikes1");
                    break;
            }

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
