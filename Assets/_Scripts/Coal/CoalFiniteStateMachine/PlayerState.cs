using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Core core;
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected bool isAnimationFinished;
    protected bool isExitingState;

    protected float startTime;

    private string animBoolName;
    private int normalMapMaterialForPlayer;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
        this.normalMapMaterialForPlayer = normalMapMaterialForPlayer;
        core = player.Core;
    }

    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        if(!player.isInStatueLevel)
        {
            player.SR.material = player.stateNormalMapMaterialsForPlayer[normalMapMaterialForPlayer];
        }
        else
        {
            player.SR.material = player.statueLevelMaterialForPlayer;
        }
        player.EyesAnim.SetBool(animBoolName, true);
        if(stateMachine.CurrentState != player.LedgeClimbState)
        {
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool(animBoolName, true);
            }
        }
        startTime = Time.time;
        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        player.EyesAnim.SetBool(animBoolName, false);
        if(stateMachine.CurrentState != player.LedgeClimbState)
        {
            if(player.coalSword.activeSelf)
            {
                player.CoalSwordAnim.SetBool(animBoolName, false);
            }
        }
    }

    public virtual void LogicUpdate(){}

    public virtual void PhysicsUpdate(){ DoChecks(); }

    public virtual void DoChecks(){}

    public virtual void AnimationTrigger(){}

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
