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
    public Color frozenColor = Color.blue;  // Color when frozen

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
            VariableHandler.Instance.frozen = true;
            Destroy(gameObject);  // Destroy the FrostEnemy, but continue the freeze effect
        }
    }

    private IEnumerator FreezeEnemiesOnDeath()
    {
        // Find all enemies with tags "Enemy" and "SwordEnemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] swordEnemies = GameObject.FindGameObjectsWithTag("SwordEnemy");

        // Combine both arrays into one list
        GameObject[] allEnemies = new GameObject[enemies.Length + swordEnemies.Length];
        enemies.CopyTo(allEnemies, 0);
        swordEnemies.CopyTo(allEnemies, enemies.Length);

        // Freeze and change color for all enemies
        foreach (GameObject enemy in allEnemies)
        {
            if (enemy != null)
            {
                StartCoroutine(FreezeAndUnfreezeEnemy(enemy));
            }
        }

        yield return null;  // Coroutine does not stop here, it waits inside the enemy loop
    }

    private IEnumerator FreezeAndUnfreezeEnemy(GameObject enemy)
    {
        // Store the original velocity and color of the enemy
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();

        Vector2 originalVelocity = Vector2.zero;
        bool wasKinematic = false;
        Color originalColor = Color.white;

        if (enemyRb != null)
        {
            originalVelocity = enemyRb.velocity;
            wasKinematic = enemyRb.isKinematic;
            enemyRb.velocity = Vector2.zero;  // Freeze movement
            enemyRb.isKinematic = true;  // Disable physics interaction
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = frozenColor;
        }

        // Freeze for 3 seconds
        yield return new WaitForSeconds(3f);

        // Restore movement and color
        if (enemyRb != null)
        {
            enemyRb.isKinematic = wasKinematic;  // Restore original kinematic state
            enemyRb.velocity = originalVelocity;  // Restore velocity
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;  // Restore original color
        }
    }

    // Handle trigger collisions to take damage
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            int damage = 10;  // Default damage if no script exists on the projectile
           
            TakeDamage(damage);  // Apply damage
        }
    }
}
