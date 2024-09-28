using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 2f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

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

        RotateTowardsPlayer(); // Ensure the enemy always points towards the player

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

    void RotateTowardsPlayer()
    {
        if (player == null) return;

        // Calculate the direction vector towards the player
        Vector2 direction = player.position - transform.position;

        // Calculate the angle from the enemy's current position to the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the enemy to face the player
        rb.rotation = angle;
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
