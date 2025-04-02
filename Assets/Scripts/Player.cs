using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private bool isSliding;

    [SerializeField] private float speed = 5f;       // Normal movement speed
    [SerializeField] private float jumpForce = 7f;   // Jump force
    [SerializeField] private float slideSpeed = 15f; // Slide speed (faster than running)

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Movement with arrow keys
        float moveInput = 0f;

        // Horizontal movement (left and right)
        if (Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f; // Move left
        else if (Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f; // Move right

        // Vertical movement (up and down)
        if (Input.GetKey(KeyCode.UpArrow))
            body.velocity = new Vector2(body.velocity.x, jumpForce); // Jumping up
        else if (Input.GetKey(KeyCode.DownArrow))
            moveInput = 0f; // Reset horizontal movement if going down

        // Sliding (much faster movement)
        if (Input.GetKey(KeyCode.LeftShift) && moveInput != 0)
        {
            isSliding = true;
            body.velocity = new Vector2(moveInput * slideSpeed, body.velocity.y);
        }
        else
        {
            isSliding = false;
            body.velocity = new Vector2(moveInput * speed, body.velocity.y);
        }

        // Apply different physics when sliding
        if (isSliding)
        {
            // Reduce friction or disable jump during slide if needed
            body.drag = 0.5f; // Example: Lower friction while sliding
        }
        else
        {
            body.drag = 1f; // Restore normal physics
        }

        // Jumping while moving forward (up arrow for jump)
        if (Input.GetKey(KeyCode.UpArrow) && moveInput != 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        // Flip player direction
        if (moveInput > 0)
            sprite.flipX = false;
        else if (moveInput < 0)
            sprite.flipX = true;
    }
}
