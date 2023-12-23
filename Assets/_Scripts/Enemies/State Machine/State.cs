using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    protected FiniteStateMachine stateMachine;
    protected Entity entity;

    protected D_Entity entityData;

    protected Core core;

    protected float startTime;

    protected string animBoolName;

    public bool isAnimationFinished { get; protected set; }

    public State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_Entity entityData)
    {
        this.entity = entity;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
        this.entityData = entityData;
        core = entity.Core;
    }

    public virtual void DoChecks()
    {

    }

    public virtual void Enter()
    {
        startTime = Time.time;
        entity.anim.SetBool(animBoolName, true);
        DoChecks();
    }

    public virtual void Exit()
    {
        entity.anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void AnimationTrigger(){}

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
