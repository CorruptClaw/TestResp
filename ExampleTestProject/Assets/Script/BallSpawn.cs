using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawn : MonoBehaviour
{

    public GameObject[] Targets;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnAttack()
    {
        SpawnTarget();
    }

    private void SpawnTarget()
    {
        Instantiate(Targets[0], transform.position, Quaternion.identity);
    }

}
