using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem_IdleState : IdleState
{
    private Golem enemy;
    public int actionNumber;

    private float lastNotSeeingPlayerTime;
    private bool lastNotSeeingPlayerTimeIsSet;
    private bool shouldFlipBeforeExit;
    public Golem_IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Golem enemy) : base(entity, stateMachine, animBoolName, entityData)
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

        if(shouldFlipBeforeExit)
        {
            Movement?.Flip();
            shouldFlipBeforeExit = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(Time.time >= startTime + idleTime)
        {
            if(enemy.isPlayerOnHead)
            {
                stateMachine.ChangeState(enemy.AttackHeadState);
            }
            else
            {
                SetRandomAction();
                
                switch (actionNumber)
                {
                    case 0:
                        stateMachine.ChangeState(enemy.Attack0State);
                        break;
                    case 1:
                        stateMachine.ChangeState(enemy.Attack1State);
                        break;
                    case 2:
                        stateMachine.ChangeState(enemy.Attack2State);
                        break;
                    default:
                        stateMachine.ChangeState(enemy.Attack0State);
                        break;
                }
            }
            
        }

        if(isPlayerInMaxAgroRange)
        {
            lastNotSeeingPlayerTimeIsSet = false;
            shouldFlipBeforeExit = false;
        }
        else
        {
            if(!lastNotSeeingPlayerTimeIsSet)
            {
                lastNotSeeingPlayerTime = Time.time;
                lastNotSeeingPlayerTimeIsSet = true;
                shouldFlipBeforeExit = true;
            }
            if(Time.time >= lastNotSeeingPlayerTime + enemy.flipCooldown && !enemy.attack0SpikesParent.activeSelf)
            {
                Movement?.Flip();
                shouldFlipBeforeExit = false;
            }
        }
    }

    public void SetRandomAction()
    {
        actionNumber = Random.Range(0, 3);
    }
}
