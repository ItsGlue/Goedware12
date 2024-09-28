using UnityEngine;

public class FrostEnemy : MonoBehaviour
{
    public float moveSpeed = 1.5f;       
    public float maxHealth = 50f;         
    private float currentHealth;
    public GameObject snowballPrefab;    
    public Transform firePoint;         
    public float shootInterval = 2f;    
    public float shootForce = 10f;       

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
                // Calculate the direction to shoot the snowball
                Vector2 direction = (player.position - firePoint.position).normalized;

                // Create a snowball
                GameObject snowball = Instantiate(snowballPrefab, firePoint.position, Quaternion.identity);

                // Rotate the snowball to face the player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                snowball.transform.rotation = Quaternion.Euler(0, 0, angle);

                // Add force to the snowball
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
            Destroy(gameObject);
        }
    }
}
