using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerShootScript : MonoBehaviour
{
    public List<GameObject> ballPrefabs;
    public bool blueBallOnly;
    public GameObject BlueBall;

    public float shootingForce;
    public Transform shootingPoint;

    private PlayerInput playerInput;
    private GameObject previewBallInstance = null;
    private BallBehavior ballBehavior;
    private PlayerBall playerBall;

    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is missing on this GameObject.");
            return;
        }

        var attackAction = playerInput.actions["Attack"];
        if (attackAction != null)
        {
            attackAction.performed += ctx => onAttack();
        }
        else
        {
            Debug.LogError("Attack action not found in PlayerInput actions.");
        }
    }
    
    void Start()
    {
        GeneratePreviewBall();
        ballBehavior = GetComponent<BallBehavior>();
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
        if (previewBallInstance != null && playerInput != null)
        {
            ShootBall();
            GeneratePreviewBall();
            playerBall.isShot = true;
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
            Debug.Log("No ball obj assigned to the list");
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
            previewBallInstance = null;
        }
    }
}

