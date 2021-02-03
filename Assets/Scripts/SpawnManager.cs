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

    [Header("Spawn location:")]
    // Area where to spawn the enemy object
    public Vector2 area;
    
    // Size of grid to spawn enemies
    public float gridSize = 1f;

    // Max amount of times the game should retry finding a new spawn pos for enemy before giving up
    [Tooltip("Max amount of times the game should retry finding a new spawn pos for enemy before giving up")]
    public int maxRegenSpawnPosTries = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnEnemy();
    }

    Vector2 GenerateSpawnPos()
    {
        // Generate point in area
        Vector2 spawnPos = new Vector2(Random.Range(-area.x/2, area.x/2), Random.Range(-area.y/2, area.y/2));
        
        // Round position, divide by gridSize and multiply afterwards for changeable gridSize
        spawnPos = new Vector2(Mathf.Round(spawnPos.x/gridSize)*gridSize,Mathf.Round(spawnPos.y/gridSize)*gridSize);

        // Return but first offset by halve of its size so it is inside the grid squares not on the grid corners
        return spawnPos - new Vector2(-gridSize/2, -gridSize/2);
    }

    bool CheckPos(Vector2 pos) // Check if enemy at position would overlap anything
    {
        Vector2 size = enemyPrefab.transform.localScale;
        if (!(pos.x-size.x/2 >= -area.x/2 && pos.y-size.y/2 >= -area.y/2 && pos.x+size.x/2 <= area.x/2 && pos.y+size.y/2 <= area.y/2)) // Check if not in bounds
        {
            return true;
        }
        else if(Physics2D.OverlapBox(pos, enemyPrefab.transform.localScale, 0f)) // True if overlaps
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    void SpawnEnemy()
    {
        if (spawnTimer <= 0)
        {
            Vector2 spawnPos = Vector2.zero;
            
            bool succes = false;
            for(int attempt = 0; attempt <= maxRegenSpawnPosTries || !succes; attempt++)
            {
                spawnPos = GenerateSpawnPos();

                if (!CheckPos(spawnPos)) // If generated position doesn't overlap anything stop loop
                {
                    succes = true;
                }
            }

            if (succes) // Dont spawn if failed to find spawnPos
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            spawnTimer = spawnTime;
        }
        else{
            spawnTimer -= Time.deltaTime;
        }
            
    }
}
