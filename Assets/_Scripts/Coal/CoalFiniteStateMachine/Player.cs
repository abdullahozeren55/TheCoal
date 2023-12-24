using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable, IKnockbackable
{

    protected Movement Movement { get => movement ??= Core.GetCoreComponent<Movement>(); }
    private Movement movement;

    protected ParticleManager ParticleManager => particleManager ??= Core.GetCoreComponent<ParticleManager>(); 
    private ParticleManager particleManager;

    protected CollisionSenses CollisionSenses => collisionSenses ??= Core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected Stats Stats => stats ??= Core.GetCoreComponent<Stats>();
    private Stats stats;

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerStartMovingState StartMovingState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerStopMovingState StopMovingState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerWallHoldState WallHoldState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerLedgeClimbState LedgeClimbState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerSuperDashState SuperDashState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    public Animator Anim { get; private set; }
    public Animator EyesAnim { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }

    public Core Core { get; private set; }

    public Transform DashDirectionIndicator { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public PlayerInventory Inventory { get; private set; }
    
    [SerializeField] private GameObject eyes;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private GameObject damageParticles;
    public GameObject groundedDustParticle;
    public Transform feetParticlePoint;
    public Transform sideParticlePointFacingDirection;
    public Transform sideParticlePointBack;

    private void Awake()
    {
        Core = GetComponentInChildren<Core>();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        StartMovingState = new PlayerStartMovingState(this, StateMachine, playerData, "startMoving");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        StopMovingState = new PlayerStopMovingState(this, StateMachine, playerData, "stopMoving");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        WallHoldState = new PlayerWallHoldState(this, StateMachine, playerData, "wallHold");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
        WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
        LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
        SuperDashState = new PlayerSuperDashState(this, StateMachine, playerData, "inAir");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        EyesAnim = eyes.GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("SuperDash_Direction_Indicator");
        Inventory = GetComponent<PlayerInventory>();

        AttackState.SetWeapon(Inventory.weapons[(int)CombatInputs.primary]);

        

        StateMachine.Initialize(IdleState);
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

    public void Damage(float amount)
    {
        Stats?.DecreaseHealth(amount);
        ParticleManager?.StartParticlesWithRandomRotation(damageParticles);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocity(strength, angle, direction);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}
