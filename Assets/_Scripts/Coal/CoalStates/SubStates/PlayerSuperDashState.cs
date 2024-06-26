using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperDashState : PlayerAbilityState
{
    public bool CanSuperDash { get; private set; }
	private bool isHolding;
	private bool dashInputStop;
    private bool dashInput;

	private float lastDashTime;

	private Vector2 dashDirection;
	private Vector2 dashDirectionInput;
	private Vector2 lastAIPos;
    public PlayerSuperDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, int normalMapMaterialForPlayer) : base(player, stateMachine, playerData, animBoolName, normalMapMaterialForPlayer)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.gameObject.layer = LayerMask.NameToLayer("DashingPlayer");
        player.gameObject.tag = "DashingPlayer";

        CanSuperDash = false;
		player.InputHandler.UseDashInput();

        if(Movement.FacingDirection == -1)
        {
            player.DashDirectionIndicator.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            player.DashDirectionIndicator.GetComponent<SpriteRenderer>().flipX = false;
        }

		isHolding = true;
		dashDirection = Vector2.right * Movement.FacingDirection;

		Time.timeScale = playerData.holdTimeScale;
		startTime = Time.unscaledTime;

		player.DashDirectionIndicator.gameObject.SetActive(true);
        player.DashDirectionIndicator.GetComponent<Animator>().Play("Coal_SuperDash_Direction_Indicator", -1, 0f);
    }

    public override void Exit()
    {
        base.Exit();

        player.InAirState.SetCanWallHold(true);

        player.gameObject.layer = LayerMask.NameToLayer("Player");
        player.lastUncollidableTime = Time.time;

        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        if (Movement?.CurrentVelocity.y > 0)
        {
			Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.superDashEndYMultiplier);
		}
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        dashInput = player.InputHandler.DashInput;

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));
        player.EyesAnim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.EyesAnim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

        if(Movement.FacingDirection == -1)
        {
            player.DashDirectionIndicator.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            player.DashDirectionIndicator.GetComponent<SpriteRenderer>().flipX = false;
        }


		if (isHolding)
        {
			dashDirectionInput = player.InputHandler.DashDirectionInput;
			dashInputStop = player.InputHandler.DashInputStop;
            player.DashState.SetCanDash(false);

			if (dashDirectionInput != Vector2.zero)
            {
				dashDirection = dashDirectionInput;
				dashDirection.Normalize();
			}

			float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
			player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

			if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
            {
				isHolding = false;
				Time.timeScale = 1f;
				startTime = Time.time;
				Movement?.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
				player.RB.drag = playerData.drag;
				Movement?.SetVelocity(playerData.superDashVelocity, dashDirection);
				player.DashDirectionIndicator.gameObject.SetActive(false);
				PlaceAfterImage();
			}
		}
        else
        {
            player.DashState.SetCanDash(true);
			Movement?.SetVelocity(playerData.superDashVelocity, dashDirection);
			CheckIfShouldPlaceAfterImage();

			if ((Time.time >= startTime + playerData.superDashTime) || isTouchingWall || ((isGrounded || isOnSlope) && Movement?.CurrentVelocity.y < 0.01f && Mathf.Abs(Movement.CurrentVelocity.x) <= 0))
            {
				player.RB.drag = 0f;
				isAbilityDone = true;
				lastDashTime = Time.time;
			}
		}

        if(dashInput && player.DashState.CheckIfCanDash())
        {
            player.RB.drag = 0f;
			lastDashTime = Time.time;
            stateMachine.ChangeState(player.DashState);
        }

        else if(isAbilityDone)
        {
            if(isGrounded || isOnSlope)
            {
                if(xInput == 0 || (isTouchingWall && xInput == Movement?.FacingDirection))
                {
                    stateMachine.ChangeState(player.StopMovingState);
                }
                else
                {
                    stateMachine.ChangeState(player.MoveState);
                }
            }
            else
            {
                stateMachine.ChangeState(player.InAirState);
            }
            
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckIfShouldPlaceAfterImage()
    {
		if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.superDashDistBetweenAfterImages) {
			PlaceAfterImage();
		}
	}

	private void PlaceAfterImage()
    {
		CoalAfterImagePool.Instance.GetFromPool(player.transform.localScale);
		lastAIPos = player.transform.position;
	}

	public bool CheckIfCanSuperDash()
    {
		return CanSuperDash && Time.time >= lastDashTime + playerData.superDashCooldown;
	}

    public void SetCanSuperDash(bool value)
    {
        CanSuperDash = value;
    }
}
