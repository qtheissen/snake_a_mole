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

    bool CheckPos(Vector2 pos, out bool a, out Color col) // Check if enemy at position would overlap anything
    {
        Vector2 size = enemyPrefab.transform.localScale;
        if (!(pos.x-size.x/2 >= -area.x/2 && pos.y-size.y/2 >= -area.y/2 && pos.x+size.x/2 <= area.x/2 && pos.y+size.y/2 <= area.y/2)) // Check if not in bounds
        {
            print("Outside of box");
            a = true;
            col = Color.blue;
            return a;
        }
        else if(Physics2D.OverlapBox(pos, enemyPrefab.transform.localScale, 0f)) // True if overlaps
        {
            print("Overlaps uhoh");
            a = true;
            col = Color.yellow;
            return a;
        }
        else
        {
            a = true;
            col = Color.red;
            return a;
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

                bool bo = false;
                Color col = Color.red;
                

                if (!CheckPos(spawnPos, out bo, out col)) // If generated position doesn't overlap anything stop loop
                {
                    succes = true;
                }
            }

            if (succes) // Dont spawn if failed to find spawnPos
            {
                print("Spawning enemy");
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                print("cant spawn enemy");
            }
            spawnTimer = spawnTime;
        }
        else{
            spawnTimer -= Time.deltaTime;
        }
            
    }
}
