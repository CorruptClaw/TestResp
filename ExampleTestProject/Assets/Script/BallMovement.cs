using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float rotationDir = Random.Range(-1f, 1f);
        rb.AddTorque(rotationDir * 10);


    }

    // Update is called once per frame
    void Update()
    {
        


    }

}
