using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float stopThreshold = 0.1f;
    public bool kinematicOn;
    public LayerMask groundLayer;
    public bool isGrounded = false;
    public bool isOnBall = false;
    public bool isConnected = false;

    public float frontLineLength = 1f;
    private bool hasSupportLogged = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            //Debug.Log($"{gameObject.name} grounded on {collision.gameObject.name}");
        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            if (!kinematicOn && rb.linearVelocity.magnitude < stopThreshold)
            {
                isOnBall = true;
                isConnected = true;
                //Debug.Log($"{gameObject.name} stopped after colliding with {collision.gameObject.name}");
            }
            isOnBall = true;
            isConnected = true;
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
        // Perform a raycast downwards to check for support below
        RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(transform.position.x - 0.2f, transform.position.y), Vector2.down, frontLineLength, groundLayer);
        RaycastHit2D hitDown2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.2f, transform.position.y), Vector2.down, frontLineLength, groundLayer);

        RaycastHit2D hitUp = Physics2D.Raycast(new Vector2(transform.position.x - 0.2f, transform.position.y), Vector2.up, frontLineLength, groundLayer);
        RaycastHit2D hitUp2 = Physics2D.Raycast(new Vector2(transform.position.x + 0.2f, transform.position.y), Vector2.up, frontLineLength, groundLayer);

        bool hasSupportBelow = hitDown.collider != null || hitDown2.collider != null;
        bool hasSupportAbove = hitUp.collider != null || hitUp2.collider != null;
        BallMovement isOn = gameObject.GetComponent<BallMovement>();

        if (hasSupportBelow && !hasSupportLogged && isGrounded && (!CompareTag("PlayerBall")))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log($"{gameObject.name} has support from below.");
            hasSupportLogged = true;
        }
        else if(!hasSupportBelow && (!CompareTag("PlayerBall")))
        {
            //Debug.Log($"{gameObject.name} has no support below. Enabling trigger and making it fall.");
            hasSupportLogged = false;
        }

        /*
        if (!isOn.isGrounded && !isOn.isOnBall && !isOn.isConnected)
        {
            Debug.Log($"{gameObject.name} has no support. Enabling trigger and making it fall.");
        }
        */
        /*
        // If there's no collider below, make this ball fall
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Ball"))
        {
            Debug.Log($"{gameObject.name} has no support below. Enabling trigger and making it fall.");
            //MakeDynamicAndFall();
        }
        */
    }

    public void MakeDynamicAndFall()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        Debug.Log($"Ball {gameObject.name} set to dynamic and falling.");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = new Vector2(transform.position.x - 0.2f, transform.position.y);
        Vector3 pointDir = Vector2.down * frontLineLength;
        Gizmos.DrawLine(startPoint, startPoint + pointDir);

        Gizmos.color = Color.red;
        Vector3 startPoint2 = new Vector2(transform.position.x + 0.2f, transform.position.y);
        Vector3 pointDir2 = Vector2.down * frontLineLength;
        Gizmos.DrawLine(startPoint2, startPoint2 + pointDir2);

        Gizmos.color = Color.magenta;
        Vector3 startPoint3 = new Vector2(transform.position.x - 0.2f, transform.position.y);
        Vector3 pointDir3 = Vector2.up * frontLineLength;
        Gizmos.DrawLine(startPoint3, startPoint3 + pointDir3);

        Gizmos.color = Color.magenta;
        Vector3 startPoint4 = new Vector2(transform.position.x + 0.2f, transform.position.y);
        Vector3 pointDir4 = Vector2.up * frontLineLength;
        Gizmos.DrawLine(startPoint4, startPoint4 + pointDir4);
    }

}
