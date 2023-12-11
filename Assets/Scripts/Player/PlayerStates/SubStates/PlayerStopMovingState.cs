using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStopMovingState : PlayerGroundedState
{

    public PlayerStopMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        core.Movement.SetVelocityX(0f);
        player.eyesAnim.SetBool("moveStopRunning", true);

    }

    public override void Exit()
    {
        base.Exit();
        playerData.stopMovingHalfFinished = false;
        playerData.stopMovingFinished = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (playerData.stopMovingFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (playerData.canMove && xInput != 0)
        {
            if (playerData.stopMovingHalfFinished)
            {
                stateMachine.ChangeState(player.StartMovingState);
            }
            else if (!playerData.stopMovingHalfFinished)
            {
                stateMachine.ChangeState(player.MoveState);
            }

        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
