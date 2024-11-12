using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviorTest : MonoBehaviour
{
    public string ballColor;
    public bool isPlayerBall;
    private Rigidbody2D rb;
    public LayerMask groundLayer;
    public float supportCheckFrequency = 1.0f; // How often to check support in seconds
    private float nextCheckTime;
    public bool isGrounded = false; // Track if the ball has initially collided with the ground or other grounded balls

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        nextCheckTime = Time.time + supportCheckFrequency;
    }

    void Update()
    {
        // Only start support checks if the ball is grounded (initially settled on the ground)
        if (isGrounded && Time.time >= nextCheckTime)
        {
            CheckAndHandleSupport();
            nextCheckTime = Time.time + supportCheckFrequency;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CompareTag("Ball") && !CompareTag("PlayerBall")) return;

        // If the ball collides with the ground or other grounded balls, mark it as grounded
        if (collision.gameObject.CompareTag("Ground") || (collision.gameObject.CompareTag("Ball") && collision.gameObject.GetComponent<BallBehaviorTest>().isGrounded))
        {
            isGrounded = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll; // Freeze the ball after initial grounding
        }

        // If it collides with a player ball with the same color, perform color-matching logic
        if (collision.gameObject.CompareTag("PlayerBall"))
        {
            isPlayerBall = true;
        }

        BallBehaviorTest otherBall = collision.gameObject.GetComponent<BallBehaviorTest>();

        if (otherBall != null && otherBall.isPlayerBall && otherBall.ballColor == this.ballColor)
        {
            List<GameObject> connectedBalls = GetConnectedBallsOfSameColor();
            if (connectedBalls.Count >= 3)
            {
                MakeBallsTriggers(connectedBalls);
                StartCoroutine(MakePlayerBallTriggerWithDelay(otherBall.GetComponent<CircleCollider2D>()));
            }
        }
    }

    private List<GameObject> GetConnectedBallsOfSameColor()
    {
        List<GameObject> connectedBalls = new List<GameObject>();
        Queue<GameObject> queue = new Queue<GameObject>();
        HashSet<GameObject> visited = new HashSet<GameObject>();

        queue.Enqueue(this.gameObject);
        visited.Add(this.gameObject);

        while (queue.Count > 0)
        {
            GameObject currentBall = queue.Dequeue();
            connectedBalls.Add(currentBall);

            Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBall.transform.position, 1f);
            foreach (var neighbor in neighbors)
            {
                BallBehaviorTest neighborBall = neighbor.GetComponent<BallBehaviorTest>();
                if (neighborBall != null && neighborBall.ballColor == this.ballColor && !visited.Contains(neighbor.gameObject))
                {
                    queue.Enqueue(neighbor.gameObject);
                    visited.Add(neighbor.gameObject);
                }
            }
        }
        return connectedBalls;
    }

    private void MakeBallsTriggers(List<GameObject> connectedBalls)
    {
        foreach (var ball in connectedBalls)
        {
            CircleCollider2D collider = ball.GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private IEnumerator MakePlayerBallTriggerWithDelay(CircleCollider2D playerCollider)
    {
        yield return new WaitForFixedUpdate();
        playerCollider.isTrigger = true;
    }

    private void CheckAndHandleSupport()
    {
        List<GameObject> connectedBalls = GetConnectedBallsOfSameColor();
        bool isSupported = IsClusterSupported(connectedBalls);

        if (!isSupported)
        {
            MakeBallsTriggers(connectedBalls); // Make unsupported balls fall
        }
    }

    private bool IsClusterSupported(List<GameObject> connectedBalls)
    {
        foreach (var ball in connectedBalls)
        {
            if (IsBallSupported(ball))
            {
                return true; // If any ball in the cluster is supported, the cluster is supported
            }
        }
        return false;
    }

    private bool IsBallSupported(GameObject ball)
    {
        // Check if the ball is supported by the ground (below it)
        return Physics2D.Raycast(ball.transform.position, Vector2.down, 0.5f, groundLayer);
    }
}

