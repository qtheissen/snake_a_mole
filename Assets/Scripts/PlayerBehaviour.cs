using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    Rigidbody2D rb;
    public float moveSpeed;
    float currentMoveSpeed;

    bool isMoving;

    [SerializeField]
    public static float diagonalMultiplier = .675f;

    float moveSpeedDiagonal;

    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        moveSpeedDiagonal = diagonalMultiplier * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SetDirection();
        Move();
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
}
