using System.Collections;
using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 2f; // Normal movement speed
    public float dashSpeed = f; // Dash movement speed
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 3f; // Time between dashes

    private bool isDashing = false; // Check if enemy is dashing
    private Vector2 dashDirection; // Store dash direction
    private float dashCooldownTimer;

    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCooldownTimer = dashCooldown; // Initialize cooldown timer
    }

    void Update()
    {
        dashCooldownTimer -= Time.deltaTime;

        if (!isDashing)
        {
            MoveTowardsPlayer(); // Regular movement
        }

        if (dashCooldownTimer <= 0)
        {
            StartCoroutine(Dash()); // Trigger dash
            dashCooldownTimer = dashCooldown; // Reset dash cooldown
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        // Calculate direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the player at normal speed
        rb.velocity = direction * moveSpeed;
    }

    IEnumerator Dash()
    {
        isDashing = true;

        if (player != null)
        {
            // Calculate direction towards the player for the dash
            dashDirection = (player.position - transform.position).normalized;
        }

        rb.velocity = dashDirection * dashSpeed; // Dash towards the player

        yield return new WaitForSeconds(dashDuration); // Wait for the dash to end

        isDashing = false; // End the dash
    }
}
