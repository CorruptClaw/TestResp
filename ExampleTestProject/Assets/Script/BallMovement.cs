using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;

    private float stopThreshold = 0.1f;

    public bool kinematicOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();

        if (kinematicOn == false)
        {
            float rotationDir = Random.Range(-1f, 1f);
            rb.AddTorque(rotationDir * 10);

        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log(gameObject.name + " touching " + collision.gameObject.name);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            //Debug.Log(gameObject.name + " touching " + collision.gameObject.name);
            if (kinematicOn == false)
            {
                if (rb.linearVelocity.magnitude < stopThreshold)
                {
                    rb.constraints = RigidbodyConstraints2D.FreezeAll;

                }

            }
            else
            {
                return;
            }

        }

    }

}
