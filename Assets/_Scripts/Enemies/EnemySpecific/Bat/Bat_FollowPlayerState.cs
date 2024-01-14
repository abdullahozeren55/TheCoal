using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;

public class Bat_FollowPlayerState : MoveState
{
    private Bat enemy;

    private float lastPathUpdatedTime;

    private Pathfinding.Path path;
    private int currentWayPoint;
    private bool reachedEndOfPath;
    public Bat_FollowPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData, Bat enemy) : base(entity, stateMachine, animBoolName, entityData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        currentWayPoint = 0;
        reachedEndOfPath = false;

        UpdatePath();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        enemy.CheckFlip();

        if(path != null)
        {
            if(currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
            }
            else
            {
                reachedEndOfPath = false;
            }

            if(isPlayerInMinAgroRange ||reachedEndOfPath)
            {
                if(Time.time >= enemy.DashState.lastDashTime + enemy.dashCooldown)
                {
                    stateMachine.ChangeState(enemy.GettingReadyToDashState);
                }
                else
                {
                    Movement?.SetVelocityZero();
                }
                
            }
            else
            {
                Vector2 direction = (path.vectorPath[currentWayPoint] - enemy.transform.position).normalized;

                Movement?.SetVelocity(entityData.movementSpeed, direction);

                float distance = Vector2.Distance(enemy.transform.position, path.vectorPath[currentWayPoint]);

                if(distance < enemy.nextWayPointDistance)
                {
                    currentWayPoint++;
                }
            }

            
        }

        if(Time.time >= lastPathUpdatedTime + 0.5f)
        {
            UpdatePath();
        }

        
    }

    private void OnPathComplete(Pathfinding.Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void UpdatePath()
    {
        if(enemy.seeker.IsDone())
        {
            if(enemy.transform.position.y > enemy.player.transform.position.y)
            {
                enemy.seeker.StartPath(enemy.playerCheck.transform.position, enemy.playerFeet.position, OnPathComplete);
            }
            else
            {
                enemy.seeker.StartPath(enemy.playerCheck.transform.position, enemy.playerHead.position, OnPathComplete);
            }
            
            lastPathUpdatedTime = Time.time;
        }
    }
}
