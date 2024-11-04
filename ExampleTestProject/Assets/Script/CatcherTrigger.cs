using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatcherTrigger : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BallBehavior ball = other.GetComponent<BallBehavior>();
        if (ball != null)
        {
            Destroy(other.gameObject);
        }
    }



}
