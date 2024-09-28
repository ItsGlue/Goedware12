using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player; 
    public float moveSpeed = 2f;
    public float dashSpeed = 8f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    private bool isDashing = false;
    private Vector2 dashDirection;
    private float dashCooldownTimer;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashCooldownTimer = dashCooldown;
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
            dashDirection = (player.position - transform.position).normalized;
        }

        rb.velocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false; // End the dash
    }
}
