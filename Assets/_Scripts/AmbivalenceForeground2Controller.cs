using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbivalenceForeground2Controller : MonoBehaviour
{
    private ParallaxLayer parallaxLayer;
    private ParallaxBackground parallaxBackground;

    private Animator anim;
    private bool shouldJump;

    void Start()
    {
        parallaxLayer = GetComponent<ParallaxLayer>();
        parallaxBackground = GetComponentInParent<ParallaxBackground>();
        anim = GetComponent<Animator>();

        shouldJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldJump)
        {
            anim.Play("JumpUp");
        }
    }

    public void SetJumpOn()
    {
        shouldJump = true;
    }

    void DestroyGameObject()
    {
        parallaxBackground.RemoveLayer(parallaxLayer);
        Destroy(gameObject, 0.1f);
    }
}
