using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;
    //TODO: ADD TUTORIAL STUFF P31-32-33
    //TODO: ADD LEDGECHECK FOR PLAYER IF YOU WANT CLIMB
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float wallCheckDistance;


    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsLedge;

    public bool Ground
    {
        //get => Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0f, whatIsGround);
        get => Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsWall);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsWall);
    }

    public bool LedgeVertical
    {
        get => Physics2D.Raycast(groundCheck.position, Vector2.down, wallCheckDistance, whatIsLedge);
    }
}
