using UnityEngine;

public class EnemyFollowAndTakeDamage : MonoBehaviour
{
    public float moveSpeed = 2f;           // Speed of the enemy
    public int maxHealth = 100;             // Maximum health of the enemy
    private int currentHealth;               // Current health of the enemy
    private Transform player;                // Reference to the player's transform

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;           // Initialize current health
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // Move towards the player's position
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if collided with a PlayerWeapon
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(20); // Assume a damage value of 20
        }
    }

    void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
