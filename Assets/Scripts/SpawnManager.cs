using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public Camera camera; // Used to resize play area
    public GameObject enemyPrefab;

    // Time inbetween enemy spawns
    public float spawnTime = 1f;

    [Header("Spawn location:")]
    // Area where to spawn the enemy object
    public Vector2 spawnArea = new Vector2(22, 10);
    
    // Size of grid to spawn enemies on
    public float gridSize = 1f;

    // Max attempts for finding a new spawn pos for enemy
    [Tooltip("Max attempts for finding a new spawn pos for enemy")]
    public int maxSpawnPosTries = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        // Resize spawn area to camera size (and floor so enemies can't stick outside of the camera's view)
        spawnArea = new Vector2(Mathf.Floor(camera.orthographicSize*2* camera.aspect),Mathf.Floor(camera.orthographicSize*2));

        StartGame(); // Start Game
    }

    public void StartGame() // Public function so other scripts can acces
    {
        StartCoroutine("SpawnEnemy"); // Start SpawnEnemy loop
    }

    public Vector2 GenerateSpawnPos()
    {
        // Generate point in spawnArea
        Vector2 spawnPos = new Vector2(Random.Range(-spawnArea.x/2, spawnArea.x/2), Random.Range(-spawnArea.y/2, spawnArea.y/2));

        Vector2 gridOffset = Vector2.zero;
        if (spawnArea.x % 2 < 1) // If x is even add x offset
        {
            gridOffset+= new Vector2(gridSize/2,0);
        }
        if (spawnArea.y % 2 < 1) // If y is even add y offset
        {
            gridOffset+= new Vector2(0, gridSize/2);
        }
        
        // Offset so that the area of spawnArea is the same as the amount of enemies
        spawnPos += gridOffset;
        
        // Round position, divide by gridSize and multiply afterwards for changeable gridSize
        spawnPos = new Vector2(Mathf.Round(spawnPos.x/gridSize)*gridSize,Mathf.Round(spawnPos.y/gridSize)*gridSize);
        
        return spawnPos -gridOffset; // Remove offset after rounding so that grid is offset and not spawnPos
    }

    IEnumerator SpawnEnemy()
    {
        Vector2 spawnPos = Vector2.zero;
        
        bool succes = false;
        Vector2 enemySize = enemyPrefab.transform.localScale; // Put in variable for more readable code
        for(int attempt = 0; attempt <= maxSpawnPosTries && !succes; attempt++) // Repeat until run out of attempts or succes
        {
            spawnPos = GenerateSpawnPos();

            // Check for colliders in box
            succes = !Physics2D.OverlapBox(spawnPos, enemySize, 0);
        }

        if (succes) // Dont spawn if failed to find spawnPos
        {
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity, transform);
        }
        
        yield return new WaitForSeconds(spawnTime);
        StartCoroutine(SpawnEnemy()); // Loop after spawnTime
    }

    // Draw gizmo to show how big spawnArea is when selected (debug)
    #if UNITY_EDITOR // Remove this code in builds
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, spawnArea);
    }
    #endif
}

