using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallHoldState : PlayerState
{
    private bool isTouchingWall;
    private bool isGrounded;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
	private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

	private Movement movement;
	private CollisionSenses collisionSenses;

    private int xInput;
    private bool jumpInput;
    public PlayerWallHoldState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)
        {
            isTouchingWall = CollisionSenses.WallFront;
            isGrounded = CollisionSenses.Ground;
        }
    }

    public override void Enter()
    {
        base.Enter();

        if(player.isInStatueLevel)
        {
            SoundFXManager.instance.PlaySoundFXClip(playerData.landOnGrassSoundFX, player.transform, 0.5f, 0.8f, 1.2f);
        }

        player.InAirState.shouldInstantiateAirJumpPrefab = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;

        if (jumpInput)
        {
			stateMachine.ChangeState(player.WallJumpState);
        }
        else if(isGrounded)
        {
            if(xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
            {
                stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.MoveState);
            }
            
        }
        else if(isAnimationFinished && isTouchingWall)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        else if(xInput != Movement?.FacingDirection || !isTouchingWall)
        {
            player.InAirState.SetCanWallHold(false);
            player.InAirState.StartWallHoldCoyoteTime();
            stateMachine.ChangeState(player.InAirState);
        }
        else
        {
            Movement?.SetVelocityY(-playerData.wallHoldVelocity);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
