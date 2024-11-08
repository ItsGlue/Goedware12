using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;            // Speed of the enemy
    public int maxHealth = 100;             // Maximum health of the enemy
    private int currentHealth;              // Current health of the enemy
    private Transform player;               // Reference to the player's transform
    private Rigidbody2D rb;                 // Reference to the Rigidbody2D component
    private Vector2 movement;               // Movement direction

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;          // Initialize current health
        rb = GetComponent<Rigidbody2D>();   // Get the Rigidbody2D component attached to the enemy
    }

    private void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = direction;
        }
    }

    private void FixedUpdate()
    {
        // Move the enemy in the direction of the player
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        // Move the Rigidbody2D towards the player's position
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
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