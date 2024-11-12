using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviorTest : MonoBehaviour
{
    public string color;       // Color identifier for the ball (e.g., "Blue")
    public Vector2Int position; // Position in the grid (e.g., (0, 1))

    public void Initialize(string color, Vector2Int position)
    {
        this.color = color;
        this.position = position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


}

