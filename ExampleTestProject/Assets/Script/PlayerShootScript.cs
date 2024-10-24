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


    private void Awake()
    {
        
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Attack"].performed += ctx => onAttack();


    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {

    }

    
    void onAttack()
    {
        //Debug.Log("Player is shooting!");

        ShootingRandomBalls();

    }

    void ShootingRandomBalls()
    {

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

    }
    
}
