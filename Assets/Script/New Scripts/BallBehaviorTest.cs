using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviorTest : MonoBehaviour
{
    private Rigidbody2D rb;
    //public int points; // Points awarded for destroying this bubble
    public string ballColor;
    public Vector2Int position; // Position in the grid (e.g., (0, 1))
    private Collider2D ballCollider;

    public bool kinematicOn;
    public bool isFalling; // Indicates if this ball is falling as part of a chain reaction



    public void Initialize(string color, Vector2Int position)
    {
        this.ballColor = color;
        this.position = position;
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();
        if (kinematicOn)
        {
            position = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

    }

    /*public void TriggerFall()
    {
        isFalling = true;
        ballCollider.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1.0f;
        rb.constraints = RigidbodyConstraints2D.None;
        OnFall();
        Debug.Log($"Ball at {position} is falling due to chain reaction.");
    }*/


    private void OnFall()
    {
        // Add points to the score when the bubble is destroyed
        /*if (ScoreManager2.instance != null && isFalling)
        {
            ScoreManager2.instance.Addscore(points);
        }*/
        if (ScoreManager.instance != null && isFalling)
        {
            ScoreManager.instance.AddPoint();
        }

    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        // Example of bubble destruction logic (adjust as needed)
        if (collision.gameObject.CompareTag("PlayerBall"))
        {
            OnDestroy();
            Destroy(gameObject);
        }

    }*/


    private void OnCollisionEnter2D(Collision2D otherCollision)
    {
        if (otherCollision.gameObject.CompareTag("Ball"))
        {
            BallManager ballManager = Object.FindFirstObjectByType<BallManager>();
            PlayerBall playerBallScript = otherCollision.gameObject.GetComponent<PlayerBall>();

            if (ballManager != null && playerBallScript != null && playerBallScript.ballColor == ballColor)
            {
                ballManager.OnPlayerBallHit(this);
                Debug.Log($"Player ball collided with ball at {position}. Triggering group response in BallManager.");

                playerBallScript.MakePlayerBallFall();
            }
            else
            {
                Debug.LogWarning("BallManager not found. Unable to trigger group response.");
            }
        }

    }


    public void SetColliderTrigger(bool isTrigger)
    {
        Collider2D ballCollider = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (ballCollider != null)
        {
            ballCollider.isTrigger = isTrigger;

            if (isTrigger && rb != null) // Only change Rigidbody properties if we’re setting it to trigger
            {
                isFalling = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.constraints = RigidbodyConstraints2D.None;

                OnFall();

                Debug.Log($"Collider for Ball at {position} set to trigger, Rigidbody set to dynamic with gravity.");
            }

        }
        else
        {
            Debug.LogWarning($"No collider found for Ball at {position}");
        }

    }



}

