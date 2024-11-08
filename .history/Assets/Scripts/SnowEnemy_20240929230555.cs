using UnityEngine;
using System.Collections;

public class FrostEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;       
    public float maxHealth = 50f;         
    private float currentHealth;
    public GameObject snowballPrefab;    
    public Transform firePoint;         
    public float shootInterval = 2f;    
    public float shootForce = 10f;

    public Color frozenColor = Color.blue; // Color when frozen
    private Color originalColor = Color.white; // Store original color
    
    private Transform player;
    private Rigidbody2D rb;
    private float shootTimer;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootTimer = 0f;
    }

    void Update()
    {
        MoveTowardsPlayer();
        ShootSnowball();
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
    }

    void ShootSnowball()
    {
        shootTimer += Time.deltaTime;

        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;

            if (player != null)
            {
                Vector2 direction = (player.position - firePoint.position).normalized;

                GameObject snowball = Instantiate(snowballPrefab, firePoint.position, Quaternion.identity);
                Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();
                snowballRb.velocity = direction * shootForce;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(FreezeEnemiesOnDeath());
            Destroy(gameObject);
        }
    }

    // Handle trigger collisions to take damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            int damage = 10; // Default damage if no script exists on the projectile
            TakeDamage(damage); // Apply damage
        }
    }

    private IEnumerator FreezeEnemiesOnDeath()
    {
        // Find all enemies with tags "Enemy" and "SwordEnemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] swordEnemies = GameObject.FindGameObjectsWithTag("SwordEnemy");

        // Freeze and change color for both groups
        foreach (GameObject enemy in enemies)
        {
            FreezeEnemy(enemy);
        }
        foreach (GameObject swordEnemy in swordEnemies)
        {
            FreezeEnemy(swordEnemy);
        }

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Unfreeze and restore original color
        foreach (GameObject enemy in enemies)
        {
            UnfreezeEnemy(enemy);
        }
        foreach (GameObject swordEnemy in swordEnemies)
        {
            UnfreezeEnemy(swordEnemy);
        }
    }

    private void FreezeEnemy(GameObject enemy)
    {
        // Disable movement by setting velocity to zero
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.velocity = Vector2.zero;
            enemyRb.isKinematic = true; // Optional, to stop any physics interaction
        }

        // Change the sprite color to blue
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Store the original color
            spriteRenderer.color = frozenColor;
        }
    }

    private void UnfreezeEnemy(GameObject enemy)
    {
        // Re-enable movement
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            enemyRb.isKinematic = false;
        }

        // Restore the original color
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
