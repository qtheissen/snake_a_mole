using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBehaviour : MonoBehaviour
{   
    Rigidbody2D rb;
    SpriteRenderer spr;
    SpawnManager spawnManager;
    GameManager gameManager;
    Animator anim;

    Vector2 goalLocation;
    public float moveSpeed = 10;
    public float maxDistance = 4;
    public float exposedTime = 5;
    float exposedTimer = 5;
    float goalDistance;

    bool canMove;
    

    public enum State {exposed, burrowed, ball, hurt};
    [SerializeField]
    public State state = State.exposed;

    // Stores the old state so the state machine knows what the state in the previous frame was
    State oldState;
    // The state the mole is to switch to at the end of a frame
    State stateToSwitch;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();

        spawnManager = FindObjectOfType<SpawnManager>();

        gameManager = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Behaviour();
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
                    // Dont allow player to move anymore
                    canMove = false;
                    // Play unburrow animation
                    anim.SetTrigger("appear");

                    // Reset the exposed timer
                    exposedTimer = exposedTime;
                }
                // Count down the exposed timer
                if (exposedTimer > 0)
                {
                    exposedTimer -= Time.deltaTime;
                }
                else{
                    // Switch to burrowed state when exposed timer is over
                    stateToSwitch = State.burrowed;
                }

                oldState = state;
                // Set the actual state to the switch state
                state = stateToSwitch;
                break;

            case State.burrowed:
                // Code to be executed when switched to this new state
                if(oldState != state)
                {
                    anim.SetTrigger("burrow");

                    // Function that picks a place for the mole to move to
                    PickGoal(maxDistance);
                    
                    
                }

                // Move the mole if they are not at their goal location
                if(new Vector2(transform.position.x, transform.position.y) != goalLocation)
                {
                    // Only move the mole if canMove is true;
                    if(canMove)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, goalLocation, moveSpeed * Time.deltaTime);
                    }
                    
                }
                else{
                    // Set the state to switch to
                    stateToSwitch = State.exposed;
                }
                
                oldState = state;
                // Set the actual state to the switch state
                state = stateToSwitch;
                break;

        }
    }

    // Function that returns a vector2 goal location within the grid used for spawning
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

    // Function used from an animation event to only allow the mole to move once they are underground
    public void AllowMove()
    {
        canMove = true;
    }
}
