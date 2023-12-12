using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{

    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        playerData.isJumping = true;
        Movement?.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        DecreaseAmountOfJumpsLeft();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CheckIfCanJump()
    {
        return (playerData.amountOfJumpsLeft > 0 && playerData.canJump && !playerData.isJumping);
    }

    public void ResetAmountOfJumpsLeft() => playerData.amountOfJumpsLeft = playerData.maxAmountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => playerData.amountOfJumpsLeft--;

    public void IncreaseAmountOfJumpsLeft() => playerData.amountOfJumpsLeft++;
}
