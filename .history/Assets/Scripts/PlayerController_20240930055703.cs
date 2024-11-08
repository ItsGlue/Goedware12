using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 100;
    public int currentHealth;
    public float damageCooldown = 1f;
    public float speedBoostMultiplier = 2f; // Speed multiplier for the boost
    public float boostDuration = 3f;        // Duration of the speed and color change
    public Color boostColor = new Color(0.702f, 0.898f, 0.973f); // #B3E5F8

    private Vector2 movement;
    private bool canTakeDamage = true;
    private bool isBoosted = false; // Track if player is boosted

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Slider healthBar;
    private Color originalColor; // To store the original color of the player

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
        originalColor = spriteRenderer.color; // Store the original color
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("SwordEnemy"))
        {
            if (canTakeDamage) {
                StartCoroutine(ApplyDamageOverTime(10));
            }
        }
    }

    IEnumerator ApplyDamageOverTime(int damage)
    {
        canTakeDamage = false;

        ChangeHealth(-damage); // Use ChangeHealth to reduce health

        yield return new WaitForSeconds(damageCooldown);

        canTakeDamage = true;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died");
            // Add additional death handling logic here
        }
        else if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Clamp health to max value
        }
    }

    void FlipPlayer()
    {
        if (movement.x > 0)
        {
            spriteRenderer.flipX = true; // Change to true to flip left
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = false; // Change to false to flip right
        }
    }

    // Coroutine to temporarily boost speed and change color
    public void BoostPlayer()
    {
        if (!isBoosted)
        {
            StartCoroutine(TemporarySpeedAndColorChange());
        }
    }

    IEnumerator TemporarySpeedAndColorChange()
    {
        isBoosted = true;

        // Increase speed and change color to #B3E5F8
        float originalSpeed = speed;
        speed *= speedBoostMultiplier;
        spriteRenderer.color = boostColor;

        yield return new WaitForSeconds(boostDuration);

        // Revert speed and color back to normal
        speed = originalSpeed;
        spriteRenderer.color = originalColor;

        isBoosted = false;
    }
}
