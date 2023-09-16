using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    public bool[] AttackInputs { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;


    private void Start()
    {
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if(context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            JumpInput = true;
            jumpInputStartTime = Time.time;
            JumpInputStop = false;
        }

        if(context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            DashInput = true;
            dashInputStartTime = Time.time;
            DashInputStop = false;
        }

        if(context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    private void CheckDashInputHoldTime()
    {
        if(Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
