using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public float damageCooldown = 1f;

    private Vector2 movement;
    private bool canTakeDamage = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
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
            StartCoroutine(TakeDamage(10));
        }
    }

    IEnumerator TakeDamage(int damage)
    {
        canTakeDamage = false;
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        // Update the health bar slider
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
        }

        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void FlipPlayer()
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
