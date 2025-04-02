using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int health = 1;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemySpawner spawner;
    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            // Move towards player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            
            // Flip sprite based on movement direction
            if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            
            // Set animation
            animator?.SetBool("IsMoving", true);
            
            // Attack if in range
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
        }
        else
        {
            // Idle behavior
            rb.velocity = Vector2.zero;
            animator?.SetBool("IsMoving", false);
        }
    }

    private void Attack()
    {
        // Play attack animation
        animator?.SetTrigger("Attack");
        
        // Damage is handled via animation event or in a separate method called from animation
    }

    public void DealDamage()
    {
        // Called from animation event
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            GameManager.Instance.PlayerTakeDamage(damageAmount);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0 && !isDead)
        {
            Die();
        }
        else
        {
            // Play hit animation
            animator?.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        isDead = true;
        
        // Disable collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Play death animation
        animator?.SetTrigger("Die");
        
        // Stop movement
        rb.velocity = Vector2.zero;
        
        // Notify spawner
        spawner?.EnemyDestroyed();
        
        // Destroy after animation
        Destroy(gameObject, 1f);
    }

    public void SetSpawner(EnemySpawner newSpawner)
    {
        spawner = newSpawner;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}

