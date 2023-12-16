using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuperDashState : PlayerAbilityState
{
    public bool CanSuperDash { get; private set; }
	private bool isHolding;
	private bool dashInputStop;

	private float lastDashTime;

	private Vector2 dashDirection;
	private Vector2 dashDirectionInput;
	private Vector2 lastAIPos;
    public PlayerSuperDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

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
    }

    public override void Exit()
    {
        base.Exit();

        player.InAirState.SetCanWallHold(true);

        if (Movement?.CurrentVelocity.y > 0)
        {
			Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.superDashEndYMultiplier);
		}
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
		player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));

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
			Movement?.SetVelocity(playerData.superDashVelocity, dashDirection);
			CheckIfShouldPlaceAfterImage();

			if ((Time.time >= startTime + playerData.superDashTime) || isTouchingWall || (isGrounded && Movement?.CurrentVelocity.y < 0.01f && Mathf.Abs(Movement.CurrentVelocity.x) <= 0))
            {
				player.RB.drag = 0f;
				isAbilityDone = true;
				lastDashTime = Time.time;
			}
		}

        if(isAbilityDone)
        {
            if(isGrounded)
            {
                stateMachine.ChangeState(player.LandState);
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
		if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages) {
			PlaceAfterImage();
		}
	}

	private void PlaceAfterImage()
    {
		CoalAfterImagePool.Instance.GetFromPool();
		lastAIPos = player.transform.position;
	}

	public bool CheckIfCanSuperDash()
    {
		return CanSuperDash && Time.time >= lastDashTime + playerData.superDashCooldown;
	}

	public void ResetCanSuperDash() => CanSuperDash = true;
}
