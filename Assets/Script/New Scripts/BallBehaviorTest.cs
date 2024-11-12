using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviorTest : MonoBehaviour
{
    private Rigidbody2D rb;
    public string ballColor;
    public Vector2Int position; // Position in the grid (e.g., (0, 1))
    private Collider2D ballCollider;

    public bool kinematicOn;


    public void Initialize(string color, Vector2Int position)
    {
        this.ballColor = color;
        this.position = position;
    }



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ballCollider = GetComponent<Collider2D>();
        if (kinematicOn)
        {
            position = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D otherCollision)
    {
        if (otherCollision.gameObject.CompareTag("PlayerBall"))
        {
            SetColliderTrigger(otherCollision.gameObject);
        }

    }

    public void SetColliderTrigger(bool isTrigger)
    {
        if (ballCollider != null)
        {
            ballCollider.isTrigger = isTrigger;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }



}

