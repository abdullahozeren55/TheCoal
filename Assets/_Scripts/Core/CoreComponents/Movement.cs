using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }

    public int FacingDirection { get; private set; }

    public bool CanSetVelocity { get; set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;

    [SerializeField] private GameObject AfterImagePool;
    [SerializeField] private GameObject cameraFollowObject;

    private CameraFollowObjectController cameraFollowObjectController;

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();
        FacingDirection = 1;
        CanSetVelocity = true;

        if(cameraFollowObject != null)
        {
            cameraFollowObjectController = cameraFollowObject.GetComponent<CameraFollowObjectController>();
        }
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    public void SetVelocityZero()
    {
        workspace = Vector2.zero;
        SetFinalVelocity();
    }
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        SetFinalVelocity();
    }

    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetFinalVelocity();
    }

    private void SetFinalVelocity()
    {
        if(CanSetVelocity)
        {
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }
    }

    public void Flip()
    {
        FacingDirection *= -1;
        RB.transform.localScale = new Vector3(RB.transform.localScale.x * -1f, 1f, 1f);
        if(AfterImagePool != null)
        {
            AfterImagePool.transform.localScale = new Vector3(RB.transform.localScale.x, 1f, 1f);
        }
        if(cameraFollowObjectController != null)
        {
            cameraFollowObjectController.CallTurn();
        }
        
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }
}
