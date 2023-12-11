using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    private Vector2 lastAIPos;
    private bool isTouchingWall;
    protected int xInput;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isTouchingWall = core.CollisionSenses.WallFront;
    }

    public override void Enter()
    {
        base.Enter();
        PlaceAfterImage();
        core.Movement.SetVelocityX(playerData.dashVelocity * core.Movement.FacingDirection);
        playerData.canDash = false;
        player.InputHandler.UseDashInput();
        playerData.canMove = false;
        player.eyesAnim.SetBool("moveRun", true);
        //player.InstantiateShockWave();
    }

    public override void Exit()
    {
        base.Exit();
        ResetCanDash();

        if (core.Movement.CurrentVelocity.y > 0)
        {
            core.Movement.SetVelocityY(core.Movement.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
        playerData.canMove = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if (!isAbilityDone)
        {
            TryPlaceAfterImage();
        }

        if ((Time.time >= startTime + playerData.dashTime) || isTouchingWall)
        {
            player.RB.drag = 0.0f;
            isAbilityDone = true;
            playerData.lastDashTime = Time.time;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void PlaceAfterImage()
    {
        lastAIPos = player.transform.position;
        CoalAfterImagePool.Instance.GetFromPool();
    }

    private void TryPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distanceBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    public bool CheckIfCanDash()
    {
        return (playerData.canDash && Time.time >= playerData.lastDashTime + playerData.dashCooldown);

    }

    public void ResetCanDash() => playerData.canDash = true;
}
