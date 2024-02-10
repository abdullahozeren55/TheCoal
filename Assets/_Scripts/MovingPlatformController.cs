using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int startingPoint = 0;
    [SerializeField] private Transform[] points;

    private int i;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, points[i].position) <= 0.2f)
        {
            i++;

            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player") || other.collider.CompareTag("DashingPlayer"))
        {
            other.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.CompareTag("Player") || other.collider.CompareTag("DashingPlayer"))
        {
            other.transform.SetParent(null);
        }
    }
}
