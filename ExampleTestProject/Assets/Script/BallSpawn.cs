using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawn : MonoBehaviour
{

    public List <GameObject> Targets;
    //public GameObject target;

    public float minX = -5f;
    public float maxX = 5f;

    

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
        if (Targets.Count > 0)
        {
            int randomIndex = Random.Range(0, Targets.Count);
            
            float randomX = Random.Range(minX, maxX);

            float dynamicYPosition = transform.position.y;

            Vector2 randomSpawnPosition = new Vector2(randomX, dynamicYPosition);


            Instantiate(Targets[randomIndex], randomSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("No ball obj assaigned to the list");
        }

    }



}
