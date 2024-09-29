using System.Collections;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;
    public int maxHealth = 3; // Enemy health

    private Transform player;
    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashCooldownTimer;
    private int currentHealth;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCooldownTimer = dashCooldown;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag

        currentHealth = maxHealth; // Initialize health
    }

    void Update()
    {
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

    // This method is called when the enemy enters a trigger collider
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider belongs to an object tagged as "PlayerWeapon"
        Debug.Log("collided with");
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(1); // Reduce health by 1 (or change this value as needed)
        }
    }

    // Method to handle taking damage
    void TakeDamage(int damageAmount)
    {
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
        Debug.Log("Enemy has died!");
        Destroy(gameObject); // Destroy the enemy object
    }
}
