using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    // normal move speed, used when the player walks in a straight line towards any directions
    public float moveSpeed;

    // actual move speed that gets applied to the player
    float currentMoveSpeed;

    // bool to determine if the player is moving
    bool isMoving;

    [SerializeField]
    public static float diagonalMultiplier = .675f;

    float moveSpeedDiagonal;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        // Set component references
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        moveSpeedDiagonal = diagonalMultiplier * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Call separate functions to have cleaner code
        SetDirection();
        Move();
        Animate();
    }

    void Move()
    {
        rb.velocity = new Vector2(direction.x * currentMoveSpeed, direction.y * currentMoveSpeed);
    }

    void SetDirection()
    {
        if(direction.x != 0 || direction.y != 0)
        {
            isMoving = true;
        }
        else{
            isMoving = false;
        }

        // Set the movement vector depending on the control scheme
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        if (direction.y != 0 && direction.x != 0)
        {
            currentMoveSpeed = moveSpeedDiagonal;
        }
        else{
            currentMoveSpeed = moveSpeed;
            
        }
    }

    void Animate()
    {
        // Flip player depending on which side the mouse is relative to the player sprite object
        if (transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x > 0)
        {
            spriteRenderer.flipX = false;
        
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
}
