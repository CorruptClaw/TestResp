using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public string ballColor;
    public bool isPlayerBall;
    private Rigidbody2D rb;

    private BallMovement ballMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballMovement = GetComponent<BallMovement>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        //Debug.Log($"Ball initialized with color: {ballColor}, isPlayerBall: {isPlayerBall}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CompareTag("Ball") && !CompareTag("PlayerBall")) return;

        //Debug.Log($"Collision detected with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("PlayerBall"))
        {
            isPlayerBall = true;
        }

        BallBehavior otherBall = collision.gameObject.GetComponent<BallBehavior>();
        if (otherBall != null && otherBall.isPlayerBall && otherBall.ballColor == this.ballColor)
        {
            //Debug.Log($"Player ball with matching color detected: {otherBall.ballColor}");

            List<GameObject> connectedBalls = GetConnectedBallsOfSameColor();
            //Debug.Log($"Connected balls found: {connectedBalls.Count}");

            if (connectedBalls.Count >= 3)
            {
                MakeBallsTriggers(connectedBalls);
                CircleCollider2D playerCollider = otherBall.GetComponent<CircleCollider2D>();
                if (playerCollider != null)
                {
                    StartCoroutine(MakePlayerBallTriggerWithDelay(playerCollider));
                }

                //Debug.Log($"Set player ball and connected balls to triggers: {otherBall.gameObject.name}");
            }
        }
    }

    public List<GameObject> GetConnectedBallsOfSameColor()
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

            //Debug.Log($"Checking ball: {currentBall.name}");

            Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBall.transform.position, 1f);
            foreach (var neighbor in neighbors)
            {
                BallBehavior neighborBall = neighbor.GetComponent<BallBehavior>();

                if (neighborBall != null && neighborBall.ballColor == this.ballColor && !visited.Contains(neighbor.gameObject))
                {
                    queue.Enqueue(neighbor.gameObject);
                    visited.Add(neighbor.gameObject);

                    //Debug.Log($"Found connected ball: {neighborBall.gameObject.name}");
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
                //Debug.Log($"Set ball to trigger: {ball.name}");
            }

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            BallMovement isOn = ball.GetComponent<BallMovement>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.constraints = RigidbodyConstraints2D.None;

                isOn.isConnected = false;
                isOn.isGrounded = false;
                isOn.isOnBall = false;

                //Debug.Log($"Unfreezing ball: {ball.name}");
            }
        }
    }

    private IEnumerator MakePlayerBallTriggerWithDelay(CircleCollider2D playerCollider)
    {
        yield return new WaitForFixedUpdate();
        playerCollider.isTrigger = true;
    }

}
