using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_Attack0State : MeleeAttackState
{
    private Golem enemy;

    private int spikeNumber;
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

    private void SetRandomSpikeNumber()
    {
        spikeNumber = Random.Range(1, 6);
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        enemy.attack0SpikesParent.transform.position = new Vector2(enemy.player.transform.position.x, enemy.attack0SpikesParent.transform.position.y);
        enemy.attack0SpikesParent.SetActive(true);
        foreach (Animator anim in enemy.attack0SpikesAnims)
        {
            SetRandomSpikeNumber();
            switch (spikeNumber)
            {
                case 1:
                    anim.Play("Spikes1");
                    break;
                case 2:
                    anim.Play("Spikes2");
                    break;
                case 3:
                    anim.Play("Spikes3");
                    break;
                case 4:
                    anim.Play("Spikes4");
                    break;
                case 5:
                    anim.Play("Spikes5");
                    break;
            
                default:
                    anim.Play("Spikes1");
                    break;
            }
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
