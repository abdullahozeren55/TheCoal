using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLeftRight : MonoBehaviour
{
    public Transform button;
    public Transform door;
    public float buttonPressedOffset = 0.1f;
    public float doorOpenOffset = 1.0f;
    public float doorOpenTime = 3.0f;
    public float buttonMovementSpeed = 1.0f;
    public float doorMovementSpeed = 1.0f;

    private bool isButtonPressed = false;
    private bool isDoorOpen = false;
    private float timer = 0.0f;

    private Vector3 initialButtonPosition;
    private Vector3 initialDoorPosition;
    private Vector3 targetButtonPosition;
    private Vector3 targetDoorPosition;

    private void Start()
    {
        initialButtonPosition = button.position;
        initialDoorPosition = door.position;
        targetButtonPosition = initialButtonPosition;
        targetDoorPosition = initialDoorPosition;
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            timer += Time.deltaTime;

            if (timer >= doorOpenTime)
            {
                CloseDoor();
            }
        }

        if (button.position != targetButtonPosition)
        {
            button.position = Vector3.MoveTowards(button.position, targetButtonPosition, buttonMovementSpeed * Time.deltaTime);
        }

        if (isDoorOpen)
        {
            door.position = Vector3.MoveTowards(door.position, targetDoorPosition, doorMovementSpeed * Time.deltaTime);

            if (door.position == targetDoorPosition && !isButtonPressed)
            {
                isDoorOpen = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isButtonPressed)
            {
                PressButton();
            }
            else if (isDoorOpen)
            {
                ResetTimer();
            }
        }
    }

    private void PressButton()
    {
        isButtonPressed = true;
        targetButtonPosition = initialButtonPosition - new Vector3(buttonPressedOffset, 0, 0);
        OpenDoor();
    }

    private void ResetButton()
    {
        targetButtonPosition = initialButtonPosition;
    }

    private void ResetTimer()
    {
        timer = 0.0f;
    }

    private void OpenDoor()
    {
        isDoorOpen = true;
        targetDoorPosition = initialDoorPosition + new Vector3(0, doorOpenOffset, 0);
    }

    private void CloseDoor()
    {
        isButtonPressed = false;
        timer = 0.0f;
        targetDoorPosition = initialDoorPosition;
        ResetButton();
    }
}
