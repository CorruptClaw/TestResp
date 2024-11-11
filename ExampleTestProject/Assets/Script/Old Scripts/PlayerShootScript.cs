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


    private void Awake() // This method is called when the script instance is being loaded.
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Attack"].performed += ctx => onAttack();

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneratePreviewBall();

    }


    // Update is called once per frame
    void Update()
    {
        if (previewBallInstance != null)
        {
            previewBallInstance.transform.position = shootingPoint.position;
        }

    }

    
    void onAttack() // Called when the "Attack" action is triggered.
    {
        if (previewBallInstance != null)
        {
            //Debug.Log("Player is shooting!");

            ShootBall();

            GeneratePreviewBall();

        }
    }


    void GeneratePreviewBall() // Generates and displays a preview ball at the shooting point
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

    void ShootBall() // Fires the preview ball by applying a force and making it a "live" ball in the game
    {
        if (previewBallInstance != null)
        {
            PlayerBall playerBall = previewBallInstance.GetComponent<PlayerBall>();
            if (playerBall != null)
            {
                playerBall.isShot = true;
            }

            Rigidbody2D rb = previewBallInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                playerBall.circleCollider.enabled = true;
                rb.AddForce(shootingPoint.up * shootingForce);
            }
            previewBallInstance= null;

        }

    }
    
}
