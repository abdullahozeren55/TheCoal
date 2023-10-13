using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerStartMovingState StartMovingState { get; private set; }

    public PlayerStopMovingState StopMovingState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerAttackState PrimaryAttackState { get; private set; }
    public PlayerAttackState SecondaryAttackState { get; private set; }

    [SerializeField]
    private PlayerData playerData;

    #endregion

    #region Components

    public Animator Anim { get; private set; }

    public Animator eyesAnim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public GameObject eyes;

    public GameObject EnvGround;
    public PhysicsMaterial2D frictionMaterial;

    #endregion

    #region Check Transforms

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;

    #endregion

    #region Other Variables

    public Vector2 CurrentVelocity { get; private set; }
    public int FacingDirection { get; private set; }


    private Vector2 workspace;

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        StartMovingState = new PlayerStartMovingState(this, StateMachine, playerData, "startMoving");
        StopMovingState = new PlayerStopMovingState(this, StateMachine, playerData, "stopMoving");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        DashState = new PlayerDashState(this, StateMachine, playerData, "move");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack1");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack2");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        eyesAnim = eyes.GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();

        FacingDirection = 1;

        StateMachine.Initialize(IdleState);

        playerData.canDash = true;
        playerData.lastDashTime = 0f;
        playerData.canMove = true;
        playerData.isJumping = false;
        playerData.canJump = true;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set Functions

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    #endregion

    #region Check Functions

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(playerData.groundCheckWidth, playerData.groundCheckHeight), 0f, playerData.whatIsGround);
        //return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance, playerData.whatIsWall);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance, playerData.whatIsWall);
    }


    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    #endregion

    #region Other Functions

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Attack1AnimationActionTrigger() => PrimaryAttackState.AnimationActionTrigger();

    private void Attack2AnimationActionTrigger() => SecondaryAttackState.AnimationActionTrigger();

    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void SetAllEyeBoolsFalse()
    {
        foreach (AnimatorControllerParameter parameter in eyesAnim.parameters)
        {
            eyesAnim.SetBool(parameter.name, false);
        }
    }

    public void StartMovingFinishTrigger() => playerData.startMovingFinished = true;

    public void StartMovingHalfFinishTrigger() => playerData.startMovingHalfFinished = true;

    public void StopMovingFinishTrigger() => playerData.stopMovingFinished = true;

    public void StopMovingHalfFinishTrigger() => playerData.stopMovingHalfFinished = true;


    #endregion
}
