using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerShootScript : MonoBehaviour
{
    public List <GameObject> ballPrefabs;

    public float shootingForce = 500f;

    public Transform shootingPoint;

    private PlayerInput playerInput;

    private GameObject previewBallInstance = null;


    private void Awake()
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

    
    void onAttack()
    {

        if (previewBallInstance != null)
        {
            Debug.Log("Player is shooting!");

            ShootBall();

            GeneratePreviewBall();
        }
        //Debug.Log("Player is shooting!");

        //ShootingRandomBalls();

    }


    void GeneratePreviewBall()
    {
        if (ballPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, ballPrefabs.Count);

            if (previewBallInstance != null)
            {
                Destroy(previewBallInstance);
            }

            previewBallInstance = Instantiate(ballPrefabs[randomIndex], shootingPoint.position, Quaternion.identity);

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



        /*
        if (ballPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, ballPrefabs.Count);

            Vector3 spawnPosition = shootingPoint != null ? shootingPoint.position : transform.position;

            GameObject ballInstance = Instantiate(ballPrefabs[randomIndex], spawnPosition, Quaternion.identity);

            Rigidbody2D rb = ballInstance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(shootingPoint.up * shootingForce);
            }

        }
        else
        {
            Debug.Log("No ball obj assaigned to the list");
        }
        */
    }
    
}
