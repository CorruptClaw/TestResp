using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootScript : MonoBehaviour
{
    public List<GameObject> ballPrefabs;
    public bool blueBallOnly;
    public GameObject BlueBall;

    public float shootingForce;
    public Transform shootingPoint;

    private PlayerInput playerInput;
    private GameObject previewBallInstance = null;
    private PlayerBall playerBall;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Attack"].performed += ctx => onAttack();

    }

    void Start()
    {
        GeneratePreviewBall();

    }

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
            ShootBall();
            GeneratePreviewBall();
        }
    }

    void GeneratePreviewBall()
    {
        if (ballPrefabs.Count > 0)
        {
            if (previewBallInstance != null)
            {
                Destroy(previewBallInstance);
            }

            GameObject ballToShoot = blueBallOnly && BlueBall != null ? BlueBall : ballPrefabs[Random.Range(0, ballPrefabs.Count)];
            previewBallInstance = Instantiate(ballToShoot, shootingPoint.position, Quaternion.identity);
            previewBallInstance.tag = "PlayerBall";

            Debug.Log("Preview ball generated.");

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
            Debug.Log("No ball objects assigned to the list.");
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
                Debug.Log("Ball shot with force: " + shootingForce);
            }

            
            // Check if the preview ball has a PlayerBall component and mark it as shot
            playerBall = previewBallInstance.GetComponent<PlayerBall>();
            if (playerBall != null)
            {
                playerBall.isShot = true;
                Debug.Log("PlayerBall isShot set to true.");
            }
            else
            {
                Debug.LogWarning("PlayerBall component missing on instantiated ball.");
            }
            
            previewBallInstance = null;
        }
        else
        {
            Debug.LogWarning("No preview ball to shoot.");
        }
    }
}

