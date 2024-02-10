using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected bool jumpInput;
    protected bool dashInput;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    private Movement movement;
    private CollisionSenses collisionSenses;


    protected bool isGrounded;
    protected bool isTouchingWall;
    protected bool isOnSlope;

    

    
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
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

        player.JumpState.ResetAmountOfJumpsLeft();
        player.InAirState.SetCanWallHold(true);
        player.InAirState.shouldInstantiateAirJumpPrefab = false;
        player.SuperDashState.SetCanSuperDash(true);
        player.DashState.SetCanDash(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.shouldCheckInputs)
        {
            xInput = player.InputHandler.NormInputX;
            jumpInput = player.InputHandler.JumpInput;
            dashInput = player.InputHandler.DashInput;
        }
        else
        {
            xInput = 0;
            jumpInput = false;
            dashInput = false;
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
