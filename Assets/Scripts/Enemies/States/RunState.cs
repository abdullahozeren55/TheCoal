using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : State
{
    protected D_RunState stateData;

    protected bool isDetectingWall;
    protected bool isDetectingLedge;
    protected bool isRunOver;
    protected bool isPlayerInMinAgroRange;

    protected bool isDetectingLedgeRun;
    protected bool isDetectingWallRun;
    protected bool isPlayerInMaxAgroRangeRun;

    protected bool performCloseRangeAction;
    protected bool isPlayerInMaxAgroRange;

    public RunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_RunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        
        isDetectingLedgeRun = entity.CheckLedgeRun();
        isDetectingWallRun = entity.CheckWallRun();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();

        isPlayerInMaxAgroRangeRun = entity.CheckPlayerInMaxAgroRangeRun();
    }

    public override void Enter()
    {
        base.Enter();
        entity.Flip();
        isRunOver = false;
        entity.SetVelocity(stateData.runSpeed);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isDetectingWall || !isDetectingLedge || !isPlayerInMaxAgroRangeRun)
        {
            isRunOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
