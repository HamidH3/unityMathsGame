using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject QPanel;

    private float horizontalInput;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public int maxJumps = 2;
    private int jumpCount = 0;

    private bool isGrounded = false;
    private bool facingRight = true;
    private bool canMove = true;

    private Rigidbody2D body;
    private Animator animator;

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
        QPanel.SetActive(false);
    }

    private void Update()
    {
        if (!canMove) return;
        
        // Get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");

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

    public void Jump()
    {
        //if movement is disabled because panel is open, then return
        if (!canMove) return;
        if (isGrounded)
        {
            jumpCount = 0;
        }

        if ((isGrounded && (
            //Input.GetKey(KeyCode.LeftArrow) ||
            //Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D)
            ) && Input.GetKeyDown(KeyCode.Space))
            || (!isGrounded && jumpCount < maxJumps && Input.GetKeyDown(KeyCode.Space))
            || isGrounded && jumpCount < maxJumps && Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpCount++;
            isGrounded = false;
            animator.SetBool("isJumping", true);
            animator.SetBool("isFalling", false);

        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false);
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //this is if player comes in contact with the ground, jumping/falling
        //animations turn off, and isGrounded triggers idle/running animations.
        isGrounded = true;
        animator.SetBool("isGrounded", true);
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);


        //shows question panel if player comes in contact with Q-Box
        if (collision.gameObject.CompareTag("Q-Box"))
        {
            if (QPanel != null)
            {
                QPanelOpen();
            }
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

    public void QPanelOpen()
    {   
        QPanel.SetActive(true);
        DisableMovement();

    }
 



}

