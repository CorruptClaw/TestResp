using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{

    public List <GameObject> Targets;
    //public GameObject target;

    public bool spawnPlenty;

    public bool spawnSingle;
    public bool Blue;
    public bool Yellow;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnTarget();

        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTarget()
    {

        GameObject spawnedObject = null;

        if (spawnPlenty == true)
        {

            if (Targets.Count > 0)
            {
                int randomIndex = Random.Range(0, Targets.Count);

                spawnedObject = Instantiate(Targets[randomIndex], transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("No ball obj assaigned to the list");
            }

        }
        else if (spawnSingle == true && spawnPlenty == false)
        {
            if (Blue == true)
            {
                spawnedObject = Instantiate(Targets[0], transform.position, Quaternion.identity);
            }
            else if (Yellow == true && Blue == false)
            {
                spawnedObject = Instantiate(Targets[5], transform.position, Quaternion.identity);
            }
            
        }

        if (spawnedObject != null)
        {

            Rigidbody2D rb = spawnedObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {

                rb.bodyType = RigidbodyType2D.Kinematic;

            }
            else
            {
                Debug.Log("No Rigidbody2D component found on the spawned object");
            }

        }

    }

}
