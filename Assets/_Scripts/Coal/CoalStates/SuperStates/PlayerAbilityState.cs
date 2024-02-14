using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
	private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

	private Movement movement;
	private CollisionSenses collisionSenses;

    protected bool isAbilityDone;
    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isOnSlope;

    protected int xInput;
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
        if (CollisionSenses)
        {
			isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
            isOnSlope = CollisionSenses.Slope;
		}
    }

    public override void Enter()
    {
        base.Enter();

        isAbilityDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        xInput = player.InputHandler.NormInputX;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
