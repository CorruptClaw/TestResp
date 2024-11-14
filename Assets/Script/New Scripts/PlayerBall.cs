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

    // A static dictionary to keep track of connected player balls by color
    private static Dictionary<string, List<PlayerBall>> colorGroups = new Dictionary<string, List<PlayerBall>>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        rb.AddTorque(Random.Range(-1f, 1f) * 10);

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

        //if (!isShot) return;

        // Check if the collision is with another player ball of the same color
        PlayerBall otherPlayerBall = collision.gameObject.GetComponent<PlayerBall>();
        if (otherPlayerBall != null && otherPlayerBall.ballColor == ballColor)
        {
            // Add both this ball and the other ball to the color group
            RegisterInColorGroup();

            // Check if there are 3 or more balls in the group now
            if (colorGroups[ballColor].Count >= 3)
            {
                TriggerFallForGroup(ballColor);
            }
        }
    }

    // Register this ball in the color group
    private void RegisterInColorGroup()
    {
        if (!colorGroups.ContainsKey(ballColor))
        {
            colorGroups[ballColor] = new List<PlayerBall>();
        }

        // Only add if it’s not already in the group
        if (!colorGroups[ballColor].Contains(this))
        {
            colorGroups[ballColor].Add(this);
        }
    }

    // Make all player balls in this color group fall
    private static void TriggerFallForGroup(string color)
    {
        if (!colorGroups.ContainsKey(color)) return;

        foreach (PlayerBall ball in colorGroups[color])
        {
            ball.MakePlayerBallFall();
        }

        // Clear the group after triggering fall
        colorGroups[color].Clear();
    }

    public void MakePlayerBallFall()
    {
        circleCollider.isTrigger = true;
        if (circleCollider.isTrigger)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;  // Change to dynamic so it can move and fall
            rb.gravityScale = 1f;                   // Apply gravity
            rb.constraints = RigidbodyConstraints2D.None; // Remove constraints
        }
        Debug.Log("Player ball set to fall (Collider is trigger, Rigidbody unfreezed).");
    }

    private void OnDestroy()
    {
        // Ensure this ball is removed from the group upon destruction
        if (colorGroups.ContainsKey(ballColor))
        {
            colorGroups[ballColor].Remove(this);
        }
    }



}
