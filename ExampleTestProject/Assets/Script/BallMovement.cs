using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private float stopThreshold = 0.1f;
    public bool kinematicOn;
    public bool isGrounded = false;
    public bool isConnected = false;

    private float nextTimeCheck;
    private float supportCheckFrequency = 1.0f; // Adjust the frequency as needed

    [SerializeField] private float timeRemaining;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

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
        //Debug.Log($"Ball {gameObject.name} initialized with kinematicOn: {kinematicOn}");
    }

    void Update()
    {
        timeRemaining = nextTimeCheck - Time.time;
        

        if (isGrounded && Time.time >= nextTimeCheck)
        {
            CheckAndHandleSupport();
            nextTimeCheck = Time.time + supportCheckFrequency;
            //Debug.Log("Checking support for floating balls");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isConnected = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            //Debug.Log($"{gameObject.name} grounded on {collision.gameObject.name}");
        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            if (!kinematicOn && rb.linearVelocity.magnitude < stopThreshold)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeAll;

                //Debug.Log($"{gameObject.name} stopped after colliding with {collision.gameObject.name}");
            }
            isConnected = true;
        }
    }

    private void CheckAndHandleSupport()
    {
        BallBehavior behavior = GetComponent<BallBehavior>();
        if (behavior != null)
        {
            List<GameObject> connectedBalls = behavior.GetConnectedBallsOfSameColor();
            bool isSupported = IsClusterSupported(connectedBalls);

            if (!isSupported)
            {
                Debug.Log($"{gameObject.name} cluster unsupported. Making cluster fall. Cluster size: {connectedBalls.Count}");
                MakeClusterFall(connectedBalls);
                isGrounded = false;
                isConnected = false;

                Debug.Log($"{gameObject.name} after falling: isConnected: {isConnected}, isGrounded: {isGrounded}");
            }
        }
    }

    private bool IsClusterSupported(List<GameObject> connectedBalls)
    {
        foreach (var ball in connectedBalls)
        {
            if (IsBallSupported(ball))
            {
                //Debug.Log($"Cluster is supported by {ball.name}");
                return true;
            }
        }
        //Debug.Log("Cluster is unsupported");
        return false;
    }

    private bool IsBallSupported(GameObject ball)
    {
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(ball.transform.position, 1f);
        foreach (var neighbor in neighbors)
        {
            if (neighbor.CompareTag("Ground"))
            {
                //Debug.Log($"Ball {ball.name} is supported by ground");
                return true;
            }
            else if (neighbor.CompareTag("Ball"))
            {
                BallBehavior neighborBehavior = neighbor.GetComponent<BallBehavior>();
                if (neighborBehavior != null && neighborBehavior.isPlayerBall)
                {
                    //Debug.Log($"Ball {ball.name} is supported by player ball: {neighbor.name}");
                    return true;
                }
            }
        }
        return false;
    }

    private void MakeClusterFall(List<GameObject> connectedBalls)
    {
        foreach (var ball in connectedBalls)
        {
            BallMovement ballMovement = ball.GetComponent<BallMovement>();
            if (ballMovement != null)
            {
                ballMovement.MakeDynamicAndFall();
                ballMovement.isGrounded = false;
                ballMovement.isConnected = false;
                Debug.Log($"{ball.name} is set to fall. isConnected: {ballMovement.isConnected}, isGrounded: {ballMovement.isGrounded}");
            }

        }

    }

    public void MakeDynamicAndFall()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        circleCollider.isTrigger = true;
        rb.gravityScale = 1f;

        Debug.Log($"Ball {gameObject.name} set to dynamic and falling");
    }

}
