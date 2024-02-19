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

	public Vector2 GroundCheckSize { get => groundCheckSize; set => groundCheckSize = value; }
    public Vector2 SlopeCheckRadius { get => slopeCheckSize; set => slopeCheckSize = value; }
    public Vector2 LedgeVerticalCheckSize { get => ledgeVerticalCheckSize; set => ledgeVerticalCheckSize = value; }
	public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

	public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }
    public LayerMask WhatIsSlope { get => whatIsSlope; set => whatIsSlope = value; }
    public LayerMask WhatIsLeftSlope { get => whatIsLeftSlope; set => whatIsLeftSlope = value; }
    public LayerMask WhatIsRightSlope { get => whatIsRightSlope; set => whatIsRightSlope = value; }
    public LayerMask WhatIsWall { get => whatIsWall; set => whatIsWall = value; }
    public LayerMask WhatIsLedge { get => whatIsLedge; set => whatIsLedge = value; }
    public LayerMask WhatIsCeiling { get => whatIsCeiling; set => whatIsCeiling = value; }

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Transform ledgeCheckHorizontal;

    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Vector2 slopeCheckSize;
    [SerializeField] private Vector2 ledgeVerticalCheckSize;
    [SerializeField] private float wallCheckDistance;
    


    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsSlope;
    [SerializeField] private LayerMask whatIsLeftSlope;
    [SerializeField] private LayerMask whatIsRightSlope;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsLedge;
    [SerializeField] private LayerMask whatIsCeiling;

    public bool Ground
    {
        get => Physics2D.OverlapBox(GroundCheck.position, groundCheckSize, 0f, whatIsGround);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * Movement.FacingDirection, wallCheckDistance, whatIsWall);
    }

    public bool LedgeVertical
    {
        get => Physics2D.OverlapBox(GroundCheck.position, ledgeVerticalCheckSize, 0f, whatIsLedge);
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
        get => Physics2D.OverlapBox(GroundCheck.position, slopeCheckSize, 0f, whatIsSlope);
    }

    public bool LeftSlope
    {
        get => Physics2D.OverlapBox(GroundCheck.position, slopeCheckSize, 0f, whatIsLeftSlope);
    }

    public bool RightSlope
    {
        get => Physics2D.OverlapBox(GroundCheck.position, slopeCheckSize, 0f, whatIsRightSlope);
    }

    public bool Ceiling
    {
		get => Physics2D.OverlapBox(CeilingCheck.position, groundCheckSize, 0f, whatIsCeiling);
	}

    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(GroundCheck.position, slopeCheckSize);
    }
}
