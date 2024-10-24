using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawn : MonoBehaviour
{

    public List <GameObject> Targets;
    //public GameObject target;


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTarget()
    {
        if (Targets.Count > 0)
        {
            int randomIndex = Random.Range(0, Targets.Count);

            Instantiate(Targets[randomIndex], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No ball obj assaigned to the list");
        }

    }
    


}
