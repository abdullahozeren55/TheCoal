using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

public class Entity : MonoBehaviour
{
    protected Movement Movement { get => movement ??= Core.GetCoreComponent<Movement>(); }
    private Movement movement;

    protected ParticleManager ParticleManager => particleManager ??= Core.GetCoreComponent<ParticleManager>(); 
    private ParticleManager particleManager;

    protected CollisionSenses CollisionSenses => collisionSenses ??= Core.GetCoreComponent<CollisionSenses>();
    private CollisionSenses collisionSenses;

    protected Stats Stats => stats ??= Core.GetCoreComponent<Stats>();
    private Stats stats;

    protected CinemachineImpulseSource impulseSource;

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;
    public Animator anim { get; private set; }
    public GameObject eyes;
    public Animator EyesAnim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    public Core Core { get; private set; }
    [SerializeField] private Transform playerCheck;
    [SerializeField] protected SO_ScreenShakeProfile screenShakeProfile;

    public GameObject damageParticles;
    public GameObject hitParticles;
    public GameObject stunStars;
    public GameObject player;

    [HideInInspector] public bool gotBackAttacked;

    private Vector2 velocityWorkspace;

    public virtual void Awake()
    {

        Core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();
        EyesAnim = eyes.GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        //return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.maxAgroDistance, entityData.whatIsPlayer);
        RaycastHit2D hit = Physics2D.Linecast(playerCheck.transform.position, player.transform.position, entityData.whatIsLineCastCanHit);

        if(hit.collider != null && hit.collider.CompareTag("Player") && hit.distance <= entityData.maxAgroDistance)
        {
            if((Movement.FacingDirection == 1 && player.transform.position.x >= playerCheck.transform.position.x) || (Movement.FacingDirection == -1 && player.transform.position.x <= playerCheck.transform.position.x))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right * Movement.FacingDirection, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

}
