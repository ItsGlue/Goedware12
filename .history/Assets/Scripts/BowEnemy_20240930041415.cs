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

    public float arrowRotationOffset = -90f; // Adjust this based on sprite orientation
    public GameObject deathParticlePrefab; 

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

            RotateTowardsPlayer();

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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
{
    if (player != null)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Calculate the angle based on direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Set the projectile's rotation to face the player, adjusting for the offset
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + arrowRotationOffset));

        // Start moving the projectile in the specified direction
        StartCoroutine(MoveProjectile(projectile, direction));

        // Destroy the projectile after a set lifetime
        Destroy(projectile, projectileLifetime);
    }
}


    IEnumerator MoveProjectile(GameObject projectile, Vector2 direction)
    {
        float elapsedTime = 0f;

        while (elapsedTime < projectileLifetime)
        {
            // Move the projectile in the specified direction
            projectile.transform.Translate(direction * projectileSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(projectile); // Destroy after lifetime
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            TakeDamage(1);
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
        VariableHandler.Instance.sword = false; 
        Debug.Log("Enemy has died!");

        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }

        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
