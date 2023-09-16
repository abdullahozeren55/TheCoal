using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{

    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(xInput != 0 && playerData.canMove)
        {
            player.CheckIfShouldFlip(xInput);

            player.SetVelocityX(playerData.movementVelocity * xInput);
        }
        else if(xInput == 0 && !isExitingState)
        {
            stateMachine.ChangeState(player.StopMovingState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
