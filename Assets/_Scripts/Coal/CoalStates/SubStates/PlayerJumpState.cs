using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{

    private int amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
        amountOfJumpsLeft = playerData.amountOfJumps;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        if(player.isInStatueLevel && (isGrounded || isOnSlope))
        {
            SoundFXManager.instance.PlaySoundFXClip(playerData.jumpOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
        }

        player.InputHandler.UseJumpInput();
        Movement?.SetVelocityY(playerData.jumpVelocity);
        isAbilityDone = true;
        amountOfJumpsLeft--;
        player.InAirState.SetIsJumping();
    }

    public override void Exit()
    {
        base.Exit();

        player.InAirState.shouldInstantiateAirJumpPrefab = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isAbilityDone)
        {
            if(isGrounded && Movement?.CurrentVelocity.y <= 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanJump()
    {
		if (amountOfJumpsLeft > 0)
        {
		    return true;
		}
        else
        {
			return false;
		}
	}

	public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

	public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
}
