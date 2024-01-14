using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour, IDamageable, IKnockbackable
{

    public Movement Movement { get => movement ??= Core.GetCoreComponent<Movement>(); }
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
    public PlayerHitState HitState { get; private set; }

    public Animator Anim { get; private set; }
    public Animator EyesAnim { get; private set; }
    public Animator CoalSwordAnim { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }

    public Core Core { get; private set; }

    public Transform DashDirectionIndicator { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public PlayerInventory Inventory { get; private set; }
    public int currentWeapon;

    public GameObject eyeLights;
    
    [SerializeField] private GameObject eyes;
    public GameObject coalSword;

    [HideInInspector] public SpriteRenderer coalSwordSR;

    [SerializeField] private PlayerData playerData;

    [SerializeField] private GameObject damageParticles;
    public GameObject groundedDustParticle;
    public GameObject airJumpParticle;
    public Transform feetParticlePoint;
    public Transform sideParticlePointFacingDirection;
    public Transform sideParticlePointBack;

    private float fallSpeedYDampingChangeThreshold;

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
        HitState = new PlayerHitState(this, StateMachine, playerData, "hit");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        EyesAnim = eyes.GetComponent<Animator>();
        CoalSwordAnim = coalSword.GetComponent<Animator>();

        coalSwordSR = coalSword.GetComponent<SpriteRenderer>();

        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();
        DashDirectionIndicator = transform.Find("SuperDash_Direction_Indicator");
        Inventory = GetComponent<PlayerInventory>();

        AttackState.SetWeapon(Inventory.weapons[0]);
        currentWeapon = 0;

        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;

        

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        Core.LogicUpdate();
        StateMachine.CurrentState.LogicUpdate();

        //if we are falling past a certain speed threshold
        if(Movement.RB.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }

        //if we are standing still or moving up
        if(Movement.RB.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping && CameraManager.instance.LerpedFromPlayerFalling)
        {
            //reset so it can be called again
            CameraManager.instance.LerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
        
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
        StateMachine.ChangeState(HitState);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
}
