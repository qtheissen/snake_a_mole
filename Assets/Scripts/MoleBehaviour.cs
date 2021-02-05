using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBehaviour : MonoBehaviour
{   
    Rigidbody2D rb;
    SpriteRenderer spr;
    SpawnManager spawnManager;
    GameManager gameManager;

    Vector2 goalLocation;
    public float moveSpeed = 10;
    public float maxDistance = 4;
    public float exposedTime = 5;
    float exposedTimer = 5;
    float goalDistance;
    

    public enum State {exposed, burrowed, ball, hurt};
    [SerializeField]
    public State state = State.exposed;
    State oldState;
    State stateToSwitch;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();

        spawnManager = FindObjectOfType<SpawnManager>();

        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Behaviour();
    }

    
    void SetStates()
    {

    }

    // State specific behaviour
    void Behaviour()
    {
        switch (state)
        {
            case State.exposed:
                // Code to be executed when switched to this new state
                if(oldState != state)
                {
                    // TODO: Play unburrow animation
                    spr.color = Color.red;

                    // Reset the exposed timer
                    exposedTimer = exposedTime;
                }
                if (exposedTimer > 0)
                {
                    exposedTimer -= Time.deltaTime;
                }
                else{
                    // Switch to burrowed state
                    stateToSwitch = State.burrowed;
                }

                oldState = state;
                state = stateToSwitch;
                break;

            case State.burrowed:
                // Code to be executed when switched to this new state
                if(oldState != state)
                {
                    // TODO: Play burrow animation
                    spr.color = Color.gray;

                    // Function that picks a place for the mole to move to
                    PickGoal(maxDistance);
                    
                    
                }

                if(new Vector2(transform.position.x, transform.position.y) != goalLocation)
                {
                    transform.position = Vector2.MoveTowards(transform.position, goalLocation, moveSpeed * Time.deltaTime);
                }
                else{
                    stateToSwitch = State.exposed;
                }
                
                oldState = state;
                state = stateToSwitch;
                break;

        }
    }

    Vector2 PickGoal(float distance)
    {
        goalDistance = 9999;
        // Reuse a piece of code from the spawnmanager to select a random grid position to move to
        while (goalDistance > distance)
        {
            goalLocation = spawnManager.GenerateSpawnPos();
            goalDistance = Vector2.Distance(transform.position, goalLocation);
        }
            
        return goalLocation;
    }
}
