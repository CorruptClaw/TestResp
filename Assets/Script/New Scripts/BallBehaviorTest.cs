using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviorTest : MonoBehaviour
{
    private Rigidbody2D rb;
    public string ballColor;       // Color identifier for the ball (e.g., "Blue")
    public Vector2Int position; // Position in the grid (e.g., (0, 1))

    public bool kinematicOn;

    public void Initialize(string color, Vector2Int position)
    {
        this.ballColor = color;
        this.position = position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (kinematicOn)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}

