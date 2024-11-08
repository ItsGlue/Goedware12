using System.Collections;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public int maxHealth = 100;

    private Transform player;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashCooldownTimer;
    private int currentHealth;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;  // To store the original color

    // Particle effect to play on death
    public GameObject deathParticlePrefab;   // Assign this in the Inspector

    public AudioClip hitSound; // Sound to play when hit
    private AudioSource audioSource;
    public AudioClip deathSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the sprite renderer
        originalColor = spriteRenderer.color;  // Store the original color

        dashCooldownTimer = dashCooldown;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the enemy is frozen
        if (VariableHandler.Instance.frozen)
        {
            FreezeEnemy();
        }
        else
        {
            UnfreezeEnemy();
            dashCooldownTimer -= Time.deltaTime;

            RotateTowardsPlayer();

            if (!isDashing)
            {
                MoveTowardsPlayer();
            }

            if (dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash());
                dashCooldownTimer = dashCooldown;
            }
        }
    }

    // Freeze the enemy (set color to blue and stop movement)
    void FreezeEnemy()
    {
        rb.velocity = Vector2.zero;  // Stop movement

        if (ColorUtility.TryParseHtmlString("#B3E5F8", out Color newColor))
            spriteRenderer.color = newColor;
    }

    // Unfreeze the enemy (restore original color)
    void UnfreezeEnemy()
    {
        spriteRenderer.color = originalColor;  // Restore the original color
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    IEnumerator Dash()
    {
        isDashing = true;

        if (player != null)
        {
            dashDirection = (player.position - transform.position).normalized;
        }

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with");
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(1);
        }
    }

    // Method to handle taking damage
    void TakeDamage(int damageAmount)
    {
        audioSource.PlayOneShot(hitSound);
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
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
        Debug.Log("Enemy has died!");

        // Play death particle effect
        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }
        VariableHandler.Instance.score += 5;
        VariableHandler.Instance.sword = true;
        Destroy(gameObject);  // Destroy the enemy object
    }
}
