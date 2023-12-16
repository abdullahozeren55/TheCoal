using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int xInput;
    protected bool jumpInput;

    protected Movement Movement => movement ??= core.GetCoreComponent<Movement>();
    private CollisionSenses CollisionSenses => collisionSenses ??= core.GetCoreComponent<CollisionSenses>();

    private Movement movement;
    private CollisionSenses collisionSenses;


    protected bool isGrounded;
    protected bool isTouchingWall;

    

    
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        if (CollisionSenses)
        {
            isGrounded = CollisionSenses.Ground;
            isTouchingWall = CollisionSenses.WallFront;
        }
    }

    public override void Enter()
    {
        base.Enter();

        player.JumpState.ResetAmountOfJumpsLeft();
        player.InAirState.SetCanWallHold(true);
        player.SuperDashState.ResetCanSuperDash();
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
