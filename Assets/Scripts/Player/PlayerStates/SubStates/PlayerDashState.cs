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
        isTouchingWall = player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        PlaceAfterImage();
        player.SetVelocityX(playerData.dashVelocity * player.FacingDirection);
        playerData.canDash = false;
        player.InputHandler.UseDashInput();
        playerData.canMove = false;
        player.eyesAnim.SetBool("moveRun", true);
    }

    public override void Exit()
    {
        base.Exit();
        ResetCanDash();

        if (player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
        playerData.canMove = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;

        if (!isAbilityDone)
        {
            CheckIfShouldPlaceAfterImage();
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

    private void CheckIfShouldPlaceAfterImage()
    {
        if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distanceBetweenAfterImages)
        {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage()
    {
        var image = PlayerAfterImagePool.Instance.GetFromPool();
        image.transform.position = lastAIPos; //TODO: tüdüdüdü
        lastAIPos = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return (playerData.canDash && Time.time >= playerData.lastDashTime + playerData.dashCooldown);

    }

    public void ResetCanDash() => playerData.canDash = true;
}
