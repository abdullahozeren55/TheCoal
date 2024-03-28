using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondPrisonController : MonoBehaviour
{
    [SerializeField] private float dissolveTime = 0.75f;

    [SerializeField] private SpriteRenderer bodySpriteRenderer;
    
    private Material material;
    private bool enteredCollider;

    private int dissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        material = bodySpriteRenderer.material;
        enteredCollider = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("DashingPlayer"))
        {
            if(!enteredCollider)
            {
                anim.Play("LookUp");
                enteredCollider = true;
            }
        }
    }

    private void FinishAnim()
    {
        StartCoroutine("Vanish");
    }

    private IEnumerator Vanish()
    {
        float elapsedTime = 0f;
        while(elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            
            float lerpedDissolve = Mathf.Lerp(0f, 1.1f, elapsedTime/dissolveTime);
            material.SetFloat(dissolveAmount, lerpedDissolve);

            yield return null;
        }

        Destroy(gameObject);
    }
}
