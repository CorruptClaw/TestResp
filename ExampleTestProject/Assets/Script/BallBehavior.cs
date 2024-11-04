using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public string ballColor;
    public bool isPlayerBall = false;
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        Debug.Log($"Ball initialized with color: {ballColor}, isPlayerBall: {isPlayerBall}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"Collision detected with: {collision.gameObject.name}");
        BallBehavior otherbBall = collision.gameObject.GetComponent<BallBehavior>();
        if (otherbBall != null && otherbBall.isPlayerBall && otherbBall.ballColor == this.ballColor)
        {
            List<GameObject> connectedBalls = GetConnectedBallsOfSameColor();
            Debug.Log($"Connected balls found: {connectedBalls.Count}");
            if (connectedBalls.Count >= 3)
            {
                MakeBallsFall(connectedBalls);
                Destroy(otherbBall.gameObject);
                Debug.Log($"Destroy player ball: {otherbBall.gameObject.name}");
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
            Debug.Log($"Checking ball: {currentBall.name}");

            Collider2D[] neighbors = Physics2D.OverlapCircleAll(currentBall.transform.position, 1f);
            foreach (var neighbor in neighbors)
            {
                BallBehavior neighborBall = neighbor.GetComponent<BallBehavior>();
                if (neighborBall != null && neighborBall.ballColor == this.ballColor && !visited.Contains(neighbor.gameObject))
                {
                    queue.Enqueue(neighbor.gameObject);
                    visited.Add(neighbor.gameObject);
                    Debug.Log($"Found connected ball: {neighborBall.gameObject.name}");

                }

            }

        }
        return connectedBalls;

    }

    private void MakeBallsFall(List<GameObject> connectedBalls)
    {
        foreach (var ball in connectedBalls)
        {
            BallBehavior ballBehavior = ball.GetComponent<BallBehavior>();
            if (ballBehavior != null)
            {
                ballBehavior.EnableGravity();
                //Debug.Log($"Enabled gravity for ball: {ball.name}");
            }

        }

    }

    public void EnableGravity()
    {
        rb.gravityScale = 1f;
        //Debug.Log($"Gravity enabled for ball: {gameObject.name}");
    }



}
