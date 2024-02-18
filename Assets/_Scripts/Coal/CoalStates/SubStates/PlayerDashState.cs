using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool CanDash { get; private set; }

    private float lastDashTime;

    private Vector2 lastAIPos;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
        player.gameObject.tag = "DashingPlayer";

        CanDash = false;
        player.InputHandler.UseDashInput();

        Movement?.SetVelocityX(playerData.dashVelocity * Movement.FacingDirection);
        if(isOnSlope)
        {
            Movement?.SetVelocityY(0.5f);
        }
        else
        {
            Movement?.SetVelocityY(0f);
        }
        CheckIfShouldPlaceAfterImage();
        Movement?.CheckIfShouldFlip(xInput);
    }

    public override void Exit()
    {
        base.Exit();

        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.lastUncollidableTime = Time.time;

        player.InAirState.SetCanWallHold(true);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if (Time.time >= startTime + playerData.dashTime || isTouchingWall)
        {
            isAbilityDone = true;
			lastDashTime = Time.time;
        }
        else if(Time.time < startTime + playerData.dashFlipCoyoteTime)
        {
            Movement?.CheckIfShouldFlip(xInput);
        }

        if(isAbilityDone)
        {
            if(isGrounded || isOnSlope)
            {
                if(xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
                {
                    stateMachine.ChangeState(player.StopMovingState);
                }
                else
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
            else
            {
                if(isTouchingWall)
                {
                    stateMachine.ChangeState(player.WallHoldState);
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }
        else
        {
            Movement?.SetVelocityX(playerData.dashVelocity * Movement.FacingDirection);
            if(isOnSlope)
            {
                Movement?.SetVelocityY(0.5f);
            }
            else
            {
                Movement?.SetVelocityY(0f);
            }
            CheckIfShouldPlaceAfterImage();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position, lastAIPos) >= playerData.dashDistBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        CoalAfterImagePool.Instance.GetFromPool(player.transform.localScale);
        lastAIPos = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
    }

    public void SetCanDash(bool value)
    {
        CanDash = value;
    }
}