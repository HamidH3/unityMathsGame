using UnityEngine;

public class MathChest : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 30f;
    [SerializeField] private GameObject visualCue;
    
    private bool isActive = true;
    private float cooldownTimer = 0f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                ReactivateChest();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            visualCue?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            visualCue?.SetActive(false);
        }
    }

    public void ActivateChest()
    {
        if (!isActive) return;

        // Play animation
        animator?.SetTrigger("Open");
        
        // Generate math question
        GameManager.Instance.ShowMathQuestion();
        
        // Deactivate chest
        isActive = false;
        cooldownTimer = cooldownTime;
        
        // Hide visual cue
        visualCue?.SetActive(false);
    }

    private void ReactivateChest()
    {
        isActive = true;
        animator?.SetTrigger("Close");
    }
}

