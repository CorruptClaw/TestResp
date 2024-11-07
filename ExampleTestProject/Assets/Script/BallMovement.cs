using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private float stopThreshold = 0.1f;
    public bool kinematicOn;
    public LayerMask groundLayer;
    public bool isGrounded = false;
    public bool isOnBall = false;
    public bool isConnected = false;

    public float frontLineLength = 1f;
    private float supportCheckFrequency = 1.0f; // Frequency of support checks in seconds
    private float nextTimeCheck;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground", "Ball");

        if (!kinematicOn)
        {
            float rotationDir = Random.Range(-1f, 1f);
            rb.AddTorque(rotationDir * 10);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        nextTimeCheck = Time.time + supportCheckFrequency;
    }

    void Update()
    {
        // Periodically check for support
        if (isGrounded || isOnBall)
        {
            if (Time.time >= nextTimeCheck)
            {
                CheckAndHandleSupport();
                nextTimeCheck = Time.time + supportCheckFrequency;
            }
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
            if (!kinematicOn && rb.linearVelocity.magnitude < stopThreshold)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                Debug.Log($"{gameObject.name} stopped after colliding with {collision.gameObject.name}");
            }
            isOnBall = true;
            isConnected = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            Debug.Log($"{gameObject.name} no longer colliding with {collision.gameObject.name}");
            isOnBall = false;
            isConnected = false;
        }
    }

    private void CheckAndHandleSupport()
    {
        // Perform a raycast downwards to check for support below
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, frontLineLength, groundLayer);

        // If there's no collider below, make this ball fall
        if (hit.collider == null)
        {
            Debug.Log($"{gameObject.name} has no support below. Enabling trigger and making it fall.");
            MakeDynamicAndFall();
        }
        else if (hit.collider.CompareTag("Ball"))
        {
            // Check if the supporting ball has its trigger enabled (indicating it's falling)
            BallMovement supportingBall = hit.collider.GetComponent<BallMovement>();
            if (supportingBall != null && supportingBall.circleCollider.isTrigger)
            {
                Debug.Log($"{gameObject.name} is no longer supported by {hit.collider.gameObject.name} (trigger enabled). Falling.");

                // Set isOnBall and isConnected to false, then trigger falling
                isOnBall = false;
                isConnected = false;
                MakeDynamicAndFall();
            }
        }
    }

    public void MakeDynamicAndFall()
    {
        Debug.Log($"{gameObject.name} isTrigger status before falling: {circleCollider.isTrigger}");
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        circleCollider.isTrigger = true;
        rb.gravityScale = 1f;
        Debug.Log($"Ball {gameObject.name} set to dynamic and falling.");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = transform.position;
        Vector3 pointDir = Vector2.down * frontLineLength;
        Gizmos.DrawLine(startPoint, startPoint + pointDir);
    }

}
