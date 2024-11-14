using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherTrigger : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out BallBehaviorTest ball))
        {
            Destroy(other.gameObject);
        }
        else if (other.TryGetComponent(out PlayerBall playerBall))
        {
            ResetPlayerBall(playerBall);

            Destroy(other.gameObject);
        }

    }

    void ResetPlayerBall(PlayerBall playerBall)
    {
        // Reset Rigidbody2D properties
        Rigidbody2D rb = playerBall.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic; // Make sure it's dynamic
        rb.gravityScale = 1f; // Enable gravity
        rb.constraints = RigidbodyConstraints2D.None; // Unfreeze all constraints

        // Reset Collider2D properties
        CircleCollider2D collider = playerBall.GetComponent<CircleCollider2D>();
        collider.isTrigger = false;

        // Reset PlayerBall custom properties
        playerBall.isGrounded = false;
        playerBall.isConnected = false;
        playerBall.isShot = false;

    }



}
