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
        BallBehaviorTest ball = other.GetComponent<BallBehaviorTest>();
        if (ball != null)
        {
            Destroy(other.gameObject);
        }

        PlayerBall playerBall = other.GetComponent<PlayerBall>();
        if (playerBall != null)
        {
            ResetPlayerBall(playerBall);

            Destroy(other.gameObject);
        }

    }

    void ResetPlayerBall(PlayerBall playerBall)
    {
        // Reset Rigidbody2D properties
        Rigidbody2D rb = playerBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Stop any movement from previous instances
            rb.angularVelocity = 0f; // Stop any rotation from previous instances
            rb.bodyType = RigidbodyType2D.Dynamic; // Make sure it's dynamic
            rb.gravityScale = 1f; // Enable gravity
            rb.constraints = RigidbodyConstraints2D.None; // Unfreeze all constraints
        }

        // Reset Collider2D properties
        CircleCollider2D collider = playerBall.GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.isTrigger = false; // Make sure the collider is not a trigger initially
        }

        // Reset PlayerBall custom properties
        PlayerBall playerBallScript = playerBall.GetComponent<PlayerBall>();
        if (playerBallScript != null)
        {
            playerBallScript.isGrounded = false;
            playerBallScript.isConnected = false;
            playerBallScript.isShot = false;
        }

    }



}
