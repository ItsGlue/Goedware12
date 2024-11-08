using UnityEngine;
using System.Collections;

public class ShieldEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;            // Speed of the enemy
    public int maxHealth = 100;             // Maximum health of the enemy
    private int currentHealth;              // Current health of the enemy
    private Transform player;               // Reference to the player's transform
    private Rigidbody2D rb;                 // Reference to the Rigidbody2D component
    private Vector2 movement;               // Movement direction

    private SpriteRenderer spriteRenderer;   // Reference to the sprite renderer
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
        if (!VariableHandler.Instance.frozen) 
        {
            spriteRenderer.color = Color.white;
            if (player != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                movement = direction;
            }
        } 
        else 
        {
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
            audioSource.PlayOneShot(hitSound);
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
        
            // Find the player GameObject
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                // Get the AutoDeactivate component from the player
                ShieldScript shieldScript = player.GetComponent<ShieldScript>();

                if (shieldScript != null)
                {
                    // Call the method on the AutoDeactivate script
                    shieldScript.ActivateAndDeactivate(5f); // Activate for 3 seconds
                }
                else
                {
                    Debug.LogWarning("AutoDeactivate script not found on Player GameObject.");
                }
            }
            else
            {
                Debug.LogWarning("Player GameObject not found.");
            }
        
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            AudioSource audioSource = audioManager.GetComponent<AudioSource>();
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound); // Play the death sound
            }
        }
        Debug.Log("Enemy has died!");

        // Play death particle effect
        if (deathParticlePrefab != null)
        {
            // Instantiate the particle effect at the enemy's position
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }

        // Find the GameObject tagged with "Shield" and enable it
        GameObject shield = GameObject.FindGameObjectWithTag("Shield");
        if (shield != null)
        {
            shield.SetActive(true); // Enable the shield
            StartCoroutine(DisableShieldAfterDelay(shield, 3f)); // Start coroutine to disable after 3 seconds
        }
        VariableHandler.Instance.score += 5;
        Destroy(gameObject); // Destroy the enemy object
    }

    private IEnumerator DisableShieldAfterDelay(GameObject shield, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified duration
        shield.SetActive(false); // Disable the shield after the wait
    }
}
