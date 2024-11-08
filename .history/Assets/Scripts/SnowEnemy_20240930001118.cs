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
