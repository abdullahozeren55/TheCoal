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

    public Core Core { get; private set; }
    public Animator Anim { get; private set; }

    public Animator eyesAnim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public SpriteRenderer SR { get; private set; }

    public PlayerInventory Inventory { get; private set; }

    public GameObject eyes;

    #endregion

    #region Prefabs

    [SerializeField] private GameObject shockWavePrefab;

    [SerializeField] private GameObject coalAfterImagePool;

    #endregion

    #region Unity Callback Functions

    private void Awake()
    {

        Core = GetComponentInChildren<Core>();

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
        PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
        SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        eyesAnim = eyes.GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        Inventory = GetComponent<PlayerInventory>();

        PrimaryAttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);

        StateMachine.Initialize(IdleState);

        playerData.canDash = true;
        playerData.lastDashTime = 0f;
        playerData.canMove = true;
        playerData.isJumping = false;
        playerData.canJump = true;
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion
    #region Other Functions

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public void InstantiateShockWave()
    {
        Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
    }

    public void SetAllEyeBoolsFalse()
    {
        foreach (AnimatorControllerParameter parameter in eyesAnim.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                eyesAnim.SetBool(parameter.name, false);
            }
        }
    }

    public void StartMovingFinishTrigger() => playerData.startMovingFinished = true;

    public void StartMovingHalfFinishTrigger() => playerData.startMovingHalfFinished = true;

    public void StopMovingFinishTrigger() => playerData.stopMovingFinished = true;

    public void StopMovingHalfFinishTrigger() => playerData.stopMovingHalfFinished = true;


    #endregion
}
