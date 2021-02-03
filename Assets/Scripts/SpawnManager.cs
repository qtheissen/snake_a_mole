using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;

    // Timer for enemy spawning
    float spawnTimer;

    // Time it takes inbetween enemy spawns
    public float spawnTime;

    // Area to spawn the enemy object
    public Vector2 area;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (spawnTimer <= 0)
        {
            
            Vector2 spawnPos = new Vector2(Random.Range(-area.x/2, area.x/2), Random.Range(-area.y/2, area.y/2));
            
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnTimer = spawnTime;
        }
        else{
            spawnTimer -= Time.deltaTime;
        }
            
    }
}
