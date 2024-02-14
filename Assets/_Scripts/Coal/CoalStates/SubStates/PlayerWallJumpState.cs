using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{

    public bool inWallJumpCombo;
    public float lastWallJumpedTime;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        
        player.InAirState.SetCanWallHold(false);
        player.Movement?.Flip();
        player.InputHandler.UseJumpInput();
        player.JumpState.DecreaseAmountOfJumpsLeft();
		Movement?.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, Movement.FacingDirection);
        player.InAirState.SetCanWallHold(true);

        inWallJumpCombo = true;
    }

    public override void Exit()
    {
        base.Exit();
        
        player.InAirState.shouldInstantiateAirJumpPrefab = true;
        lastWallJumpedTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        player.EyesAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.EyesAnim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if (Time.time >= startTime + playerData.wallJumpTime)
        {
			stateMachine.ChangeState(player.InAirState);
		}
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
