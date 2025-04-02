using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

