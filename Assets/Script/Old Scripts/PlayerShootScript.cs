using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerShootScript : MonoBehaviour
{
    public List <GameObject> ballPrefabs;
    public bool blueBallOnly;
    public GameObject BlueBall;


    public float shootingForce;
    public Transform shootingPoint;

    private PlayerInput playerInput;
    private GameObject previewBallInstance = null;
    private BallMovementTest movementTest;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Attack"].performed += ctx => onAttack();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneratePreviewBall();

        movementTest = GetComponent<BallMovementTest>();

    }


    // Update is called once per frame
    void Update()
    {
        if (previewBallInstance != null)
        {
            previewBallInstance.transform.position = shootingPoint.position;
        }

    }

    
    void onAttack()
    {
        if (previewBallInstance != null)
        {
            //Debug.Log("Player is shooting!");

            ShootBall();

            GeneratePreviewBall();

        }
        movementTest.isShot = true;
    }


    void GeneratePreviewBall()
    {
        if (ballPrefabs.Count > 0)
        {
            if (previewBallInstance != null)
            {
                Destroy(previewBallInstance);

            }

            GameObject ballToShoot;
            if (blueBallOnly && BlueBall != null)
            {
                ballToShoot = BlueBall;
            }
            else
            {
                int randomIndex = Random.Range(0, ballPrefabs.Count);
                ballToShoot = ballPrefabs[randomIndex];

            }

            previewBallInstance = Instantiate(ballToShoot, shootingPoint.position, Quaternion.identity);
            previewBallInstance.tag = "PlayerBall";

            BallBehavior ballBehavior = previewBallInstance.GetComponent<BallBehavior>();
            if (ballBehavior != null)
            {
                ballBehavior.isPlayerBall = true;
            }
            
            Rigidbody2D rb = previewBallInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            }

        }
        else
        {
            Debug.Log("No ball obj assaigned to the list");
        }

    }

    void ShootBall()
    {
        if (previewBallInstance != null)
        {
            Rigidbody2D rb = previewBallInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.AddForce(shootingPoint.up * shootingForce);
            }
            previewBallInstance= null;

        }

    }
    
}