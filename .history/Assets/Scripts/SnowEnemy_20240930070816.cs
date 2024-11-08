using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FrostEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float maxHealth = 50f;
    private float currentHealth;
    public GameObject snowballPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    public float shootForce = 10f;
    public GameObject deathParticlesPrefab; // Reference to the particle system prefab

    private Transform player;
    private Rigidbody2D rb;
    private float shootTimer;

    public AudioClip hitSound; // Sound to play when hit
    private AudioSource audioSource;
    public AudioClip deathSound;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootTimer = 0f;
        audioSource = GetComponent<AudioSource>();
        
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
            Die(); // Call Die method to handle death
        }
    }

    private void Die()
    {
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager != null)
        {
            AudioSource audioSource = audioManager.GetComponent<AudioSource>();
            if (audioSource != null && deathSound != null)
            {
                audioSource.PlayOneShot(deathSound); // Play the death sound
            }
        }
        VariableHandler.Instance.frozen = true;
        // Spawn particles on death
        if (deathParticlesPrefab != null)
        {
            Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        }
        VariableHandler.Instance.score += 5;
        Destroy(gameObject);  // Destroy the FrostEnemy
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon"))
        {
            audioSource.PlayOneShot(hitSound);
            int damage = 10;  
            TakeDamage(damage);  
        }
    }
}