using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootScript : MonoBehaviour
{
    public GameObject[] ballPrefabs;

    public float shootingForce = 500f;

    public Transform shootingPoint;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    void onAttack()
    {

        ShootingRandomBalls();

    }

    void ShootingRandomBalls()
    {

        if (ballPrefabs.Length == 0)
        {
            Debug.Log("No Ball prefabs assigned!");

        }

        int randomIndex = Random.Range(0, ballPrefabs.Length);
        GameObject selectedBall = ballPrefabs[randomIndex];

        GameObject ballInstance = Instantiate(selectedBall, shootingPoint.position, shootingPoint.rotation);

        Rigidbody2D ballRB = ballInstance.GetComponent<Rigidbody2D>();
        if (ballRB != null)
        {
            ballRB.AddForce(shootingPoint.right *  shootingForce);
        }

    }
    */
}
