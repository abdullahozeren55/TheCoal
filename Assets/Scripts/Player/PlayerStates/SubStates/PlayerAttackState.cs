using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        playerData.isAttacking = true;
    }

    public override void Exit()
    {
        base.Exit();

        playerData.isAttacking = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void AnimationActionTrigger()
    {
        
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        isAbilityDone = true;
    }

}
