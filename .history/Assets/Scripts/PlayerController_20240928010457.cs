using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public float damageCooldown = 1f;  // Delay between damage ticks

    private Vector2 movement;
    private bool canTakeDamage = true;  // Flag to control damage cooldown

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;  // To flip the sprite
    [SerializeField] private Slider healthBar;  // Reference to the health bar UI slider

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;  // Set the max value of the health bar to the player's max health
        healthBar.value = currentHealth; // Initialize the health bar value
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        FlipPlayer();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * speed;
    }

    void OnDisable()
    {
        rb.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Danger") && canTakeDamage)
        {
            StartCoroutine(TakeDamage(10));  // Adjust the damage amount as needed
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        canTakeDamage = false;  // Disable further damage for the cooldown duration
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        // Update the health bar slider
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            // Implement player death logic, such as restarting the level
        }

        yield return new WaitForSeconds(damageCooldown);  // Wait for cooldown duration
        canTakeDamage = true;  // Re-enable damage after cooldown
    }

    void FlipPlayer()
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;  // Facing right
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
