using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObjectController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float flipYRotationTime = 0.5f;

    private float startRotation;
    private float endRotationAmount;

    private float yRotation;
 
    private float elapsedTime;
    private Coroutine turnCoroutine;
    private bool isFacingRight;
    private bool canFlip;

    private void Awake()
    {
        isFacingRight = true;
    }

    private void Update()
    {
        transform.position = playerTransform.position;
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
