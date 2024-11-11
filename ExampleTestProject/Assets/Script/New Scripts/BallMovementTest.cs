using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementTest : MonoBehaviour
{

    private Rigidbody2D rb;
    //private float stopThreshold = 0.2f;
    public bool kinematicOn;
    public LayerMask groundLayer;
    public LayerMask ballLayer;
    public bool isGrounded = false;
    public bool isOnBall = false;
    public bool isConnected = false;
    public bool isShot = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!kinematicOn)
        {
            float rotationDir = Random.Range(-1f, 1f);
            rb.AddTorque(rotationDir * 10);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Update()
    {
        // Periodically check for support
        if (!isGrounded || !isOnBall)
        {
            CheckAndHandleSupport();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isConnected = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log($"{gameObject.name} grounded on {collision.gameObject.name}");
        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            isConnected = true;
            isOnBall = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log($"{gameObject.name} stopped after colliding with {collision.gameObject.name}");
            //isConnected = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball") || !collision.gameObject.CompareTag("PlayerBall"))
        {
            //Debug.Log($"{gameObject.name} no longer colliding with {collision.gameObject.name}");
            isOnBall = false;
            isConnected = false;
        }
    }

    private void CheckAndHandleSupport()
    {

    }

    public void MakeDynamicAndFall()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        Debug.Log($"Ball {gameObject.name} set to dynamic and falling.");
    }



}
