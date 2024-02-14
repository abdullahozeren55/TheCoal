using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Sprite buttonNotPressedImage;
    [SerializeField] private Sprite buttonPressedImage;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
        {
            if(spriteRenderer.sprite != buttonPressedImage)
            {
                spriteRenderer.sprite = buttonPressedImage;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("DashingPlayer"))
        {
            if(spriteRenderer.sprite != buttonNotPressedImage)
            {
                spriteRenderer.sprite = buttonNotPressedImage;
            }
        }
    }
}
