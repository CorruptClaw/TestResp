using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    private Rigidbody2D rb;
    public string ballColor;
    public bool isPlayerBall;
    public CircleCollider2D circleCollider;
    //private float stopThreshold = 0.2f;
    //public LayerMask groundLayer;
    //public LayerMask ballLayer;
    public bool isGrounded = false;
    public bool isOnBall = false;
    public bool isConnected = false;
    public bool isShot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        float rotationDir = Random.Range(-1f, 1f);
        rb.AddTorque(rotationDir * 10);

    }

    void Update()
    {
        /*
        // Periodically check for support
        if (!isGrounded || !isOnBall)
        {
            CheckAndHandleSupport();
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isShot) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isConnected = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Debug.Log($"{gameObject.name} grounded on {collision.gameObject.name}");
        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            isConnected = true;
            isOnBall = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Debug.Log($"{gameObject.name} stopped after colliding with {collision.gameObject.name}");
            //isConnected = true;
        }
    }

    public void MakePlayerBallFall()
    {
        circleCollider.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Dynamic;  // Change to dynamic so it can move and fall
        rb.gravityScale = 1f;                   // Apply gravity
        rb.constraints = RigidbodyConstraints2D.None; // Remove constraints
        Debug.Log("Player ball set to fall (Collider is trigger, Rigidbody unfreezed).");
    }

}
