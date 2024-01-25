using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    private float minAmountToMove;

    void Start()
    {
        minAmountToMove = parallaxFactor/100f;
    }

    public void Move(float delta)
    {
        if(Mathf.Abs(delta) >= minAmountToMove)
        {
            Vector3 newPos = transform.localPosition;
            newPos.x -= delta * parallaxFactor;

            transform.localPosition = newPos;
        }
        
    }

}