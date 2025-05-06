using System;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{


    private float horizontalInput;
    private float moveSpeed = 5f;
    private float jumpForce = 4.5f;
    public int maxJumps = 2;
    private int jumpCount = 0;

    private bool isGrounded = false;
    private bool facingRight = true;
    private bool canMove = true;
    private bool isTouchingWall = false;

    private Rigidbody2D body;
    public Animator animator;

    //moving platforms
    private MovingFloatingPlatform currPlatform;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //this ensures player doesnt rotate when comes in contact to an object
        body.freezeRotation = true;
    }

    private void Start()
    {
        canMove = true;
    }

    private void Update()
    {
        if (!canMove) return;


        //Getting horizontal input. this ensures the keys are explicitely
        //defined instead of using left and right arrows if i were
        //to call Input.GetAxis("Horizontal");
        horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
            horizontalInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            horizontalInput = 1f;


        if (canMove)
        {
            MoveHorizontally();
            // Flip sprite based on direction
            Jump();
            Flip();
        }


        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        animator.SetFloat("yVelocity", body.velocity.y);
        animator.SetBool("isGrounded", isGrounded);

        // Falling detection
        if (!isGrounded && body.velocity.y < -0.1f)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);


        }
        //check if avatar landed
        if (isGrounded)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
        if (currPlatform != null)
        {
            transform.position += currPlatform.deltaMovement; // Move player with the platform
        }
        HandleWallSliding();

        if (isTouchingWall && !isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            JumpOffWall();
        }
    }

    private void MoveHorizontally()
    {
        if (canMove)
        //player is only able to move horizontally if canMove is true
        {
            body.velocity = new Vector2(horizontalInput * moveSpeed, body.velocity.y);
        }
        // Apply horizontal movement


        // Update animator parameters
        animator.SetFloat("xVelocity", Mathf.Abs(body.velocity.x));
        animator.SetFloat("yVelocity", body.velocity.y);
    }

    public void Flip()
    {
        if ((facingRight && horizontalInput < 0f) || (!facingRight && horizontalInput > 0f))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void Jump()
    {
        //if movement is disabled because panel is open, then return
        if (!canMove) return;
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if ((isGrounded && (

            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D)
            ) && Input.GetKeyDown(KeyCode.W))
            || (!isGrounded && jumpCount < maxJumps && Input.GetKeyDown(KeyCode.W))
            || isGrounded && jumpCount < maxJumps && Input.GetKeyDown(KeyCode.W))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpCount++;
            isGrounded = false;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isWallSliding", false);

        }
    }


    private void HandleWallSliding()
    {
        bool pressingTowardWall = (facingRight && horizontalInput > 0) || (!facingRight && horizontalInput < 0);
        bool falling = body.velocity.y < -0.1f;
        bool shouldWallSlide = !isGrounded && body.velocity.y < 0 && isTouchingWall && pressingTowardWall;

        if (shouldWallSlide)
        {
            body.velocity = new Vector2(0, body.velocity.y);  // stop horizontal motion on wall
            animator.SetBool("isWallSliding", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else
        {
            // Stop wall slide animation if it's no longer valid
            animator.SetBool("isWallSliding", false);

            // Explicitly check for falling if not grounded or wall sliding
            if (!isGrounded && !isTouchingWall && falling)
            {
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
            }

            // Reset when grounded
            if (isGrounded)
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("FloatingPlatform"))
        {
            MovingFloatingPlatform platform = collision.gameObject.GetComponent<MovingFloatingPlatform>();
            if (platform != null)
            {
                currPlatform = platform;
                isGrounded = true;
                jumpCount = 0;
                animator.SetBool("isGrounded", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWallSliding", false);


            }

            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                jumpCount = 0;
                animator.SetBool("isGrounded", true);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isWallSliding", false);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            animator.SetBool("isWallSliding", false);
            animator.SetBool("isFalling", true);
        }
        if (collision.gameObject.CompareTag("FloatingPlatform"))
        {
            // Make sure to unparent the player from the platform only if the platform is active
            currPlatform = null;

        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        //this is if player comes in contact with the ground, jumping/falling
        //animations turn off, and isGrounded triggers idle/running animations.
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }


    }


    public void JumpOffWall()
    {
        if (isTouchingWall && !isGrounded)
        {
            body.velocity = new Vector2(facingRight ? -jumpForce : jumpForce, jumpForce);
            isTouchingWall = false;
            animator.SetBool("isWallSliding", false);  // Stop wall sliding after jumping off
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    public void DisableMovement()
    {
        canMove = false;

    }






}

