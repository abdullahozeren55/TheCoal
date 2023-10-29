using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : Entity
{
    public Transform attackPosition;

    public MouseRunState runState { get; private set; }
    public MouseAttackState attackState { get; private set; }

    public MouseRangedAttackState rangedAttackState { get; private set; }

    [SerializeField]
    private D_RunState runStateData;
    [SerializeField]
    private D_MeleeAttack attackStateData;
    [SerializeField]
    private D_RangedAttackState rangedAttackStateData;


    public override void Start()
    {
        base.Start();

        runState = new MouseRunState(this, stateMachine, "run", runStateData, this);
        attackState = new MouseAttackState(this, stateMachine, "attack", attackPosition, attackStateData, this);
        rangedAttackState = new MouseRangedAttackState(this, stateMachine, "rangedAttack", attackPosition, rangedAttackStateData, this);


        stateMachine.Initialize(runState);
    }
}
