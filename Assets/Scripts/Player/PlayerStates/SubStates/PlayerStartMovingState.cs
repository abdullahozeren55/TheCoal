using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartMovingState : PlayerGroundedState
{

    public PlayerStartMovingState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        playerData.startMovingFinished = false;
        playerData.startMovingHalfFinished = false;
        player.eyesAnim.SetBool("moveStartRunning", true);

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (xInput == 0)
        {
            if (!playerData.startMovingHalfFinished)
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (playerData.startMovingHalfFinished)
            {
                stateMachine.ChangeState(player.StopMovingState);
            }

        }
        else if (playerData.canMove && !playerData.startMovingFinished)
        {
            
            core.Movement.CheckIfShouldFlip(xInput);
            core.Movement.SetVelocityX(playerData.movementVelocity * xInput);

            //float targetSpeed = xInput * playerData.movementVelocity;
            //float speedDif = targetSpeed - player.RB.velocity.x;
            //float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? playerData.acceleration : playerData.decceleration;
            //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, playerData.velPower) * Mathf.Sign(speedDif);
            //core.Movement.SetVelocityXWithAcceleration(movement);
        }
        else if (playerData.startMovingFinished )
        {
            stateMachine.ChangeState(player.MoveState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
