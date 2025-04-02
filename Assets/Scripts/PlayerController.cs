using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float slideDuration = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;

    // References
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // State variables
    private bool isGrounded;
    private bool isSliding;
    private float slideTimer;
    private int bulletCount = 0;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movement input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator?.SetTrigger("Jump");
        }

        // Handle sliding
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !isSliding)
        {
            isSliding = true;
            slideTimer = slideDuration;
            animator?.SetBool("IsSliding", true);
        }

        // Handle shooting
        if (Input.GetKeyDown(KeyCode.Space) && bulletCount > 0)
        {
            Shoot();
        }

        // Update animator
        animator?.SetBool("IsGrounded", isGrounded);
        animator?.SetFloat("Speed", Mathf.Abs(horizontalInput));

        // Handle movement and sliding
        HandleMovement(horizontalInput);
        
        // Handle sprite flipping
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }
    }

    private void HandleMovement(float horizontalInput)
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            float direction = facingRight ? 1 : -1;
            rb.velocity = new Vector2(direction * slideSpeed, rb.velocity.y);

            if (slideTimer <= 0)
            {
                isSliding = false;
                animator?.SetBool("IsSliding", false);
            }
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    private void Shoot()
    {
        if (bulletCount <= 0) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        
        if (bulletRb != null)
        {
            float direction = facingRight ? 1 : -1;
            bulletRb.velocity = new Vector2(direction * bulletSpeed, 0);
            bulletCount--;
            
            // Update UI
            GameManager.Instance.UpdateBulletCount(bulletCount);
        }
    }

    public void AddBullets(int amount)
    {
        bulletCount += amount;
        GameManager.Instance.UpdateBulletCount(bulletCount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MathChest"))
        {
            other.GetComponent<MathChest>()?.ActivateChest();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

