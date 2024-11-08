using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRateInSeconds = 2f;
    public float projectileLifetime = 5f;
    
    private Transform player;
    private float fireCooldown;
    
    // Adjust this value based on your sprite's default orientation
    public float arrowRotationOffset = -90f; // Adjust this based on sprite orientation

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireCooldown = fireRateInSeconds;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
    }

    void Update()
    {
        if (player != null && !VariableHandler.Instance.frozen)
        {
            // Rotate the enemy to face the player
            RotateTowardsPlayer();

            // Handle shooting
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0)
            {
                Shoot();
                fireCooldown = fireRateInSeconds;
            }
        }
        if (VariableHandler.Instance.frozen) {
            if (ColorUtility.TryParseHtmlString("#B3E5F8", out Color newColor)) spriteRenderer.color = newColor;
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the enemy to face the player
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            // Set the projectile's rotation to face the player with an offset for the arrow
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + arrowRotationOffset));

            // Set the projectile's velocity
            rb.velocity = direction * projectileSpeed;

            // Destroy the projectile after a set lifetime
            Destroy(projectile, projectileLifetime);
        }
    }
}
