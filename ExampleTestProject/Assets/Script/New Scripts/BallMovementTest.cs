using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementTest : MonoBehaviour
{

    private Rigidbody2D rb;
    //private float stopThreshold = 0.2f;
    public bool kinematicOn;
    //public LayerMask groundLayer;
    //public LayerMask ballLayer;
    public bool isGrounded = false;
    public bool isOnBall = false;
    public bool isConnected = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (kinematicOn)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void Update()
    {
        
        // Periodically check for support
        /*
        if (!isGrounded || !isOnBall)
        {
            CheckAndHandleSupport();
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D otherCollision)
    {

        if (otherCollision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isConnected = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log($"{gameObject.name} grounded on {otherCollision.gameObject.name}");
        }
        else if (otherCollision.gameObject.CompareTag("Ball") || otherCollision.gameObject.CompareTag("PlayerBall"))
        {
            isConnected = true;
            isOnBall = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log($"{gameObject.name} stopped after colliding with {otherCollision.gameObject.name}");
            //isConnected = true;
        }

        /*
        if ((otherCollision.gameObject.CompareTag("Ball") || otherCollision.gameObject.CompareTag("PlayerBall")) && (!isGrounded || !isOnBall || !isConnected))
        {
            Debug.Log($"{gameObject.name} no longer colliding with {otherCollision.gameObject.name}");
        }
        */
    }
    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log($"{gameObject.name} is still grounded on {collision.gameObject.name}");
        }
        else if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            Debug.Log($"{gameObject.name} is still stopped after colliding with {collision.gameObject.name}");
        }
    }
    */

    
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        

        
        if (!collision.gameObject.CompareTag("Ball") || !collision.gameObject.CompareTag("PlayerBall"))
        {
            Debug.Log($"{gameObject.name} no longer colliding with {collision.gameObject.name}");
            isOnBall = false;
            isConnected = false;
        }
        
    }
    */
    /*
    private void CheckAndHandleSupport()
    {
        MakeDynamicAndFall();
    }
    
    public void MakeDynamicAndFall()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
        //Debug.Log($"Ball {gameObject.name} set to dynamic and falling.");
    }
    */


}
