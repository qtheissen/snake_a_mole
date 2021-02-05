using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenButtons : MonoBehaviour
{
    public GameManager gameManager;
    public EndScreen endScreenAnimater;
    public Transform player;
    public GameObject spawnManager;
    
    public void Restart()
    {
        endScreenAnimater.HideEndScreen(); // Hide end screen with animation

        gameManager.AddScore(-gameManager.score); // Set score to 0
        gameManager.totalTimeSeconds = gameManager.gameLengthSeconds; // Set time to max time
        gameManager.gameOver = false; // No longer gameOver

        player.position = Vector3.zero - new Vector3(0, 0, -0.01f); // Reset player to center (but with offset on z-axis so player is in front of moles)
        
        foreach (Transform child in spawnManager.transform) { // Destroy all moles from previous game
            Destroy(child.gameObject);
        }
        
        spawnManager.GetComponent<SpawnManager>().StartGame(); // Restart mole spawning loop
    }

    public void MainMenu()
    {
        //TODO make a main menu to return to
    }
}
