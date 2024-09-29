using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;

    private Vector2 movement;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;  // To flip the sprite

    void Start()
    {
        currentHealth = maxHealth;
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
        if (collision.gameObject.CompareTag("Danger"))
        {
            TakeDamage(10); // Adjust the damage amount as needed
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            // Implement player death logic, such as restarting the level
        }
    }

    void FlipPlayer()
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;  // Facing right
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;   // Facing left
        }
    }
}
