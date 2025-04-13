//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    private float horizontalInput;
//    float moveSpeed = 5f;
//    //private SpriteRenderer sprite;
//    //private bool isSliding;
//    private bool isGrounded = false;
//    private bool facingRight = true;
//    //[SerializeField] private float speed = 5f;       
//    [SerializeField] private float jumpForce = 5f;
//    //[SerializeField] private float slideSpeed = 15f; 

//    private Rigidbody2D body;
//    private Animator animator;

//    private void Awake()
//    {
//        body = GetComponent<Rigidbody2D>();
//        animator = GetComponent<Animator>();

//        //sprite = GetComponent<SpriteRenderer>();
//    }

//    private void Update()
//    {
//        horizontalInput = Input.GetAxis("Horizontal");
//        Flip();

//        //vertical movement
//        if (Input.GetKey(KeyCode.Space) && isGrounded)
//        {
//            body.velocity = new Vector2(body.velocity.x, jumpForce);
//            isGrounded = false;
//            animator.SetBool("isJumping", !isGrounded);
//        }




//        // Movement with arrow keys
//        //float moveInput = 0f;

//        //// Horizontal movement (left and right)
//        //if (Input.GetKey(KeyCode.LeftArrow))
//        //    moveInput = -1f; // Move left
//        //else if (Input.GetKey(KeyCode.RightArrow))
//        //    moveInput = 1f; // Move right



//        // Sliding (much faster movement)
//        //    if (Input.GetKey(KeyCode.LeftShift) && moveInput != 0)
//        //    {
//        //        isSliding = true;
//        //        body.velocity = new Vector2(moveInput * slideSpeed, body.velocity.y);
//        //    }
//        //    else
//        //    {
//        //        isSliding = false;
//        //        body.velocity = new Vector2(moveInput * speed, body.velocity.y);
//        //    }

//        //    // Apply different physics when sliding
//        //    if (isSliding)
//        //    {
//        //        // Reduce friction or disable jump during slide if needed
//        //        body.drag = 0.5f; // Example: Lower friction while sliding
//        //    }
//        //    else
//        //    {
//        //        body.drag = 1f; // Restore normal physics
//        //    }

//        //    // Jumping while moving forward (up arrow for jump)
//        //    if (Input.GetKey(KeyCode.UpArrow) && moveInput != 0)
//        //    {
//        //        body.velocity = new Vector2(body.velocity.x, jumpForce);
//        //    }

//        //    // Flip player direction
//        //    if (moveInput > 0)
//        //        sprite.flipX = false;
//        //    else if (moveInput < 0)
//        //        sprite.flipX = true;
//        //}
//    }

//    private void FixedUpdate()
//    {
//        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);
//        animator.SetFloat("xVelocity", Math.Abs(body.velocity.x));
//        animator.SetFloat("yVelocity", body.velocity.x);

//    }
//    //private void Flip()
//    //{
//    //    facingRight = !facingRight;
//    //    sprite.flipX = !facingRight;
//    //}

//    void Flip()
//    {
//        if ((facingRight && horizontalInput < 0f) || (!facingRight && horizontalInput > 0f))
//        {
//            facingRight = !facingRight;
//            Vector3 local_scale = transform.localScale;
//            local_scale.x *= -1f;
//            transform.localScale = local_scale;
//        }
//    }
//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        isGrounded = true;
//        animator.SetBool("isJumping", !isGrounded);
//        //if (collision.CompareTag("Ground"))
//        //{
//        //    isGrounded = true;
//        //    animator.SetBool("isJumping", !isGrounded);
//        //}

//    }
//}




























using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private bool isGrounded = false;
    private bool facingRight = true;

    private Rigidbody2D body;
    private Animator animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //this ensures player doesnt rotate when comes in contact to an object
        body.freezeRotation = true;
    }

    private void Update()
    {
        // Get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip sprite based on direction
        Flip();

        // Jump if grounded and space is pressed
        if (isGrounded && (Input.GetKey(KeyCode.LeftArrow) || 
            Input.GetKey(KeyCode.RightArrow) &&
            Input.GetKeyDown(KeyCode.Space) || 
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) &&
            Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

        }
        // Falling detection
        if (!isGrounded && body.velocity.y < -0.1f)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement
        body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);

        // Update animator parameters
        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        animator.SetFloat("yVelocity", body.velocity.y);
    }

    private void Flip()
    {
        if ((facingRight && horizontalInput < 0f) || (!facingRight && horizontalInput > 0f))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
            isGrounded = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        


    }
}

