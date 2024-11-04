using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnCopyTest : MonoBehaviour
{
    public List<GameObject> Targets;
    //public GameObject target;

    public float minX = -5f;
    public float maxX = 5f;

    public float spawnDuration;
    [SerializeField] private float timer;

    [SerializeField] private float spawnInterval;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = spawnDuration;
        StartCoroutine(SpawnTarget());

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator SpawnTarget()
    {

        while (timer > 0)
        {
            SpawnRandomBalls();
            yield return new WaitForSeconds(spawnInterval);

            timer -= 1f;

        }

    }

    void SpawnRandomBalls()
    {
        if (Targets.Count > 0)
        {
            int randomIndex = Random.Range(0, Targets.Count);

            float randomX = Random.Range(minX, maxX);

            float dynamicYPosition = transform.position.y;

            Vector2 randomSpawnPosition = new Vector2(randomX, dynamicYPosition);


            GameObject spawnedBall = Instantiate(Targets[randomIndex], randomSpawnPosition, Quaternion.identity);
            spawnedBall.tag = "Ball";

        }
        else
        {
            Debug.Log("No ball obj assaigned to the list");
        }
    }

    



}
