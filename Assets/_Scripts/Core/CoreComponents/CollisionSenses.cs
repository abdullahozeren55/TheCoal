using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    protected Movement Movement { get => movement ??= core.GetCoreComponent<Movement>(); }
    private Movement movement;

    public Transform GroundCheck {
		get => GenericNotImplementedError<Transform>.TryGet(groundCheck, core.transform.parent.name);
		private set => groundCheck = value;
	}
	public Transform WallCheck {
		get => GenericNotImplementedError<Transform>.TryGet(wallCheck, core.transform.parent.name);
		private set => wallCheck = value;
	}
    public Transform LedgeCheckHorizontal {
		get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, core.transform.parent.name);
		private set => ledgeCheckHorizontal = value;
	}
	public Transform CeilingCheck {
		get => GenericNotImplementedError<Transform>.TryGet(ceilingCheck, core.transform.parent.name);
		private set => ceilingCheck = value;
	}

	public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public float SlopeCheckRadius { get => slopeCheckRadius; set => slopeCheckRadius = value; }
	public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

	public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    public LayerMask WhatIsSlope { get => whatIsSlope; set => whatIsSlope = value; }
    public LayerMask WhatIsWall { get => whatIsWall; set => whatIsWall = value; }
    public LayerMask WhatIsLedge { get => whatIsLedge; set => whatIsLedge = value; }
    public LayerMask WhatIsCeiling { get => whatIsCeiling; set => whatIsCeiling = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;

    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float slopeCheckRadius;
    [SerializeField] private float wallCheckDistance;


    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSlope;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsLedge;
    [SerializeField] private LayerMask whatIsCeiling;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsWall);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -Movement.FacingDirection, wallCheckDistance, whatIsWall);
    }

    public bool LedgeVertical
    {
        get => Physics2D.Raycast(GroundCheck.position, Vector2.down, wallCheckDistance, whatIsLedge);
    }

    public bool LedgeHorizontal
    {
		get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsLedge);
	}

    public bool LedgeHorizontalBottom
    {
		get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsLedge);
	}

    public bool Slope
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, slopeCheckRadius, whatIsSlope);
    }

    public bool Ceiling {
		get => Physics2D.OverlapCircle(CeilingCheck.position, groundCheckRadius, whatIsCeiling);
	}
}
