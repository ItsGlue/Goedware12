using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRateInSeconds = 2f;
    public float projectileLifetime = 5f;
    
    public int maxHealth = 3;               // Maximum hits the enemy can take before dying
    private int currentHealth;

    private Transform player;
    private float fireCooldown;
    private SpriteRenderer spriteRenderer;
    
    // Adjust this value based on your sprite's default orientation
    public float arrowRotationOffset = -90f; // Adjust this based on sprite orientation

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fireCooldown = fireRateInSeconds;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
        currentHealth = maxHealth;            // Initialize current health
    }

    void Update()
    {
        if (player != null && !VariableHandler.Instance.frozen)
        {
            spriteRenderer.color = Color.white;

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
        
        if (VariableHandler.Instance.frozen) 
        {
            if (ColorUtility.TryParseHtmlString("#B3E5F8", out Color newColor)) 
            {
                spriteRenderer.color = newColor;
            }
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

    // This method gets called when the enemy collides with a PlayerWeapon
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("" + collision.gameObject.name);
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(1); // Decrease health by 1 when hit by a PlayerWeapon
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
