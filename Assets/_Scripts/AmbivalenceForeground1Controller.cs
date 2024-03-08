using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbivalenceForeground1Controller : MonoBehaviour
{
    public Transform[] targetPositions; // Hedef pozisyon
    public float moveSpeed = 5f;
    public float idleStateTime = 3f;

    private ParallaxLayer parallaxLayer;
    private ParallaxBackground parallaxBackground;
    private Animator anim;
    private float idleStartTime;

    private bool isMoveDone;
    private int targetCount;

    void Start()
    {
        parallaxLayer = GetComponent<ParallaxLayer>();
        parallaxBackground = GetComponentInParent<ParallaxBackground>();
        anim = GetComponent<Animator>();
        
        targetCount = 0;
        isMoveDone = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(!isMoveDone)
        {
            if (transform.position == targetPositions[targetCount].position)
            {
                if(targetCount < targetPositions.Length - 1)
                {
                    targetCount++;
                }
                else
                {
                    anim.Play("Idle");
                    idleStartTime = Time.time;
                    isMoveDone = true;
                }
                
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPositions[targetCount].position, moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            if(Time.time >= idleStartTime + idleStateTime)
            {
                anim.Play("JumpUp");
            }
        }
        
        
    }

    void DestroyGameObject()
    {
        parallaxBackground.RemoveLayer(parallaxLayer);
        Destroy(gameObject, 0.1f);
    }
}
