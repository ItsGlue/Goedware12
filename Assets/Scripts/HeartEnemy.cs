using UnityEngine;

public class HeartEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;            // Speed of the enemy
    public int maxHealth = 50;             // Maximum health of the enemy
    private int currentHealth;              // Current health of the enemy
    private Transform player;               // Reference to the player's transform
    private Rigidbody2D rb;                 // Reference to the Rigidbody2D component
    private Vector2 movement;               // Movement direction

    private SpriteRenderer spriteRenderer;
    // Reference to the particle system or particle prefab
    public GameObject deathParticlePrefab; 

    public AudioClip hitSound; // Sound to play when hit
    public AudioClip deathSound;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;          // Initialize current health
        rb = GetComponent<Rigidbody2D>();   // Get the Rigidbody2D component attached to the enemy
    }

    private void Update()
    {
        if (!VariableHandler.Instance.frozen) {
            spriteRenderer.color = Color.white;
            if (player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                movement = direction;
            }
        } else {
            if (ColorUtility.TryParseHtmlString("#B3E5F8", out Color newColor)) 
            {
                spriteRenderer.color = newColor;
            }
            movement = Vector2.zero;
             rb.velocity = Vector2.zero;
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
        audioSource.PlayOneShot(hitSound);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            AudioSource audioSource = audioManager.GetComponent<AudioSource>();
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound); // Play the death sound
            }
        }
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("ahjklajkldfjaklsfdjklskjlfskljsadfljkadfjkslkldfjskljfs");
                playerController.ChangeHealth(10); // Apply damage to the player
            }
            
        Debug.Log("Enemy has died!");

        // Play death particle effect
        if (deathParticlePrefab != null)
        {
            // Instantiate the particle effect at the enemy's position
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }
        VariableHandler.Instance.score += 5;
        Destroy(gameObject); // Destroy the enemy object
    }
}
