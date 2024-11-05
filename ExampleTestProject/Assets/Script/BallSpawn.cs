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
    public bool Green;
    public bool Orange;
    public bool Red;
    public bool White;
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

        if (spawnPlenty)
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
        else if (spawnSingle)
        {
            List<string> selectedColors = new List<string>();
            if (Blue) selectedColors.Add("Blue");
            if (Green) selectedColors.Add("Green");
            if (Orange) selectedColors.Add("Orange");
            if (Red) selectedColors.Add("Red");
            if (White) selectedColors.Add("White");
            if (Yellow) selectedColors.Add("Yellow");

            if (selectedColors.Count > 1)
            {
                Debug.Log("Multiple colors selected. Please select only one color when spawnSingle is true.");
                return;
            }
            else if (selectedColors.Count == 1)
            {
                int colorIndex = selectedColors[0] switch
                {
                    "Blue" => 0,
                    "Green" => 1,
                    "Orange" => 2,
                    "Red" => 3,
                    "White" => 4,
                    "Yellow" => 5,
                    _ => -1
                };

                if (colorIndex != -1 && colorIndex < Targets.Count)
                {
                    spawnedObject = Instantiate(Targets[colorIndex], transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Selected color index out of range or color not assigned in Targets list.");
                }

            }
            else
            {
                Debug.Log("No color selected. Please select a color to spawn a single ball.");
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
