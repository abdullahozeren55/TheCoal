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

    public FiniteStateMachine stateMachine;

    public D_Entity entityData;

    private SpriteRenderer SR;
    private SpriteRenderer eyesSR;
    public Animator anim { get; private set; }
    public GameObject eyes;
    public Animator EyesAnim { get; private set; }
    public AnimationToStateMachine atsm { get; private set; }

    public Core Core { get; private set; }
    public Transform playerCheck;

    public GameObject damageParticles;
    public GameObject stunStars;
    public GameObject player;
    public bool shouldCheckSortingLayerWithPlayerInMaxAgroRange;

    [HideInInspector] public bool gotBackAttacked;

    private Vector2 velocityWorkspace;

    public virtual void Awake()
    {

        Core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();
        atsm = GetComponent<AnimationToStateMachine>();
        SR = GetComponent<SpriteRenderer>();

        if(eyes != null)
        {
            EyesAnim = eyes.GetComponent<Animator>();
            eyesSR = eyes.GetComponent<SpriteRenderer>();
        }

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        CheckIfInFrontOfPlayerOrBehind();

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

    private void CheckIfInFrontOfPlayerOrBehind()
    {
        if(shouldCheckSortingLayerWithPlayerInMaxAgroRange)
        {
            if(CheckPlayerInMaxAgroRange())
            {
                if(SR.sortingLayerName != "Enemy Behind Player")
                {
                    SR.sortingLayerName = "Enemy Behind Player";
                    SR.sortingOrder = GameManager.instance.enemyBehindPlayerSortingOrder;

                    GameManager.instance.enemyBehindPlayerSortingOrder++;

                    if(eyesSR != null)
                    {
                        eyesSR.sortingLayerName = "Enemy Behind Player";
                        eyesSR.sortingOrder = GameManager.instance.enemyBehindPlayerSortingOrder;

                        GameManager.instance.enemyBehindPlayerSortingOrder++;
                    }
                }
            }
            else
            {
                if(SR.sortingLayerName != "Enemy in Front Of Player")
                {
                    SR.sortingLayerName = "Enemy in Front Of Player";
                    SR.sortingOrder = GameManager.instance.enemyInFrontOfPlayerSortingOrder;

                    GameManager.instance.enemyInFrontOfPlayerSortingOrder++;

                    if(eyesSR != null)
                    {
                        eyesSR.sortingLayerName = "Enemy in Front Of Player";
                        eyesSR.sortingOrder = GameManager.instance.enemyInFrontOfPlayerSortingOrder;

                        GameManager.instance.enemyInFrontOfPlayerSortingOrder++;

                    }
                }
            }
        }
        else
        {
            if(transform.position.x >= player.transform.position.x)
            {
                if(SR.sortingLayerName != "Enemy in Front Of Player")
                {
                    SR.sortingLayerName = "Enemy in Front Of Player";
                    SR.sortingOrder = GameManager.instance.enemyInFrontOfPlayerSortingOrder;

                    GameManager.instance.enemyInFrontOfPlayerSortingOrder++;

                    if(eyesSR != null)
                    {
                        eyesSR.sortingLayerName = "Enemy in Front Of Player";
                        eyesSR.sortingOrder = GameManager.instance.enemyInFrontOfPlayerSortingOrder;

                        GameManager.instance.enemyInFrontOfPlayerSortingOrder++;

                    }
                }
            
            }
            else
            {

                if(SR.sortingLayerName != "Enemy Behind Player")
                {
                    SR.sortingLayerName = "Enemy Behind Player";
                    SR.sortingOrder = GameManager.instance.enemyBehindPlayerSortingOrder;

                    GameManager.instance.enemyBehindPlayerSortingOrder++;

                    if(eyesSR != null)
                    {
                        eyesSR.sortingLayerName = "Enemy Behind Player";
                        eyesSR.sortingOrder = GameManager.instance.enemyBehindPlayerSortingOrder;

                        GameManager.instance.enemyBehindPlayerSortingOrder++;
                    }
                }
            
            }
        }
        
        
    }

    private void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    private void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

}
