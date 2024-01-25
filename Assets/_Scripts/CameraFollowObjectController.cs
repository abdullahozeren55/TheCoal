using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObjectController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 0.5f;
    [SerializeField] private float ledgeClimbDuration = 0.3f;
    [SerializeField] private float ledgeHoldCameraSlideTime = 0.2f;

    [SerializeField] private Movement movement;

    private Player playerScript;

    private float startRotation;
    private float endRotationAmount;

    private float yRotation;
 
    private float elapsedTime;
    private float elapsedTimeForLedgeClimb;
    private float elapsedTimeForLedgeHold;
    private Coroutine turnCoroutine;
    private bool isFacingRight;
    private bool canFlip;
    private bool isTempPosSet;
    private Vector2 tempPos;

    private void Awake()
    {
        isFacingRight = true;
        elapsedTimeForLedgeClimb = 0f;
        elapsedTimeForLedgeHold = 0f;
        isTempPosSet = false;

        playerScript = playerTransform.GetComponent<Player>();
    }

    private void Update()
    {

        if(playerScript.StateMachine.CurrentState == playerScript.LedgeClimbState && playerScript.LedgeClimbState.positionsSet)
        {
            if(playerScript.LedgeClimbState.isClimbing)
            {
                elapsedTimeForLedgeHold = 0f;

                if(elapsedTimeForLedgeClimb < ledgeClimbDuration)
                {
                    elapsedTimeForLedgeClimb += Time.deltaTime;

                    float t = elapsedTimeForLedgeClimb/ledgeClimbDuration;
                    
                    transform.position = Vector3.Lerp(playerScript.LedgeClimbState.startPos, playerScript.LedgeClimbState.stopPos, t);
                }
            }
            else
            {
                elapsedTimeForLedgeClimb = 0f;

                if(!isTempPosSet)
                {
                    tempPos = transform.position;
                    isTempPosSet = true;
                }

                if(elapsedTimeForLedgeHold < ledgeHoldCameraSlideTime)
                {
                    elapsedTimeForLedgeHold += Time.deltaTime;

                    float t = elapsedTimeForLedgeHold/ledgeHoldCameraSlideTime;

                    transform.position = Vector3.Lerp(tempPos, playerScript.LedgeClimbState.startPos, t);
                }
            }
        }
        else
        {
            isTempPosSet = false;
            transform.position = playerTransform.position;
        }



        if((isFacingRight && movement.FacingDirection == -1) || (!isFacingRight && movement.FacingDirection == 1))
        {
            CallTurn();
        }
    }

    public void CallTurn()
    {
        canFlip = false;
        startRotation = transform.localEulerAngles.y;
        endRotationAmount = DetermineEndRotation();
        yRotation = 0f;
        canFlip = true;
        turnCoroutine = StartCoroutine(FlipYLerp());    
    }

    private IEnumerator FlipYLerp()
    {
        elapsedTime = 0f;

        while(elapsedTime < flipYRotationTime && canFlip)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }

    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if(isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }

}
