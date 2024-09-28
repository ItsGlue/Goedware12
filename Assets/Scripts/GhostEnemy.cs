using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;         
    public int maxHealth = 40;           
    private int currentHealth;
    public float invisibilityDuration = 2f; // Duration of invisibility
    public float invisibilityCooldown = 10f; // Cooldown between invisibility uses
    private bool isInvisible = false;     // Tracks if ghost is invisible
    private float invisibilityTimer = 0f; // Timer for invisibility cooldown
    private float invisibleTime = 0f;     // Timer for how long ghost has been invisible

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Collider2D ghostCollider;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ghostCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        MoveTowardsPlayer();
        HandleInvisibility();
    }

    void MoveTowardsPlayer()
    {
        if (player != null && !isInvisible) // Ghost only moves when visible
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }
    }

    void HandleInvisibility()
    {
        invisibilityTimer += Time.deltaTime;

        if (isInvisible)
        {
            invisibleTime += Time.deltaTime;

            // End invisibility after the duration
            if (invisibleTime >= invisibilityDuration)
            {
                BecomeVisible();
            }
        }
        else if (invisibilityTimer >= invisibilityCooldown)
        {
            // Become invisible every 10 seconds
            BecomeInvisible();
        }
    }

    void BecomeInvisible()
    {
        isInvisible = true;
        invisibleTime = 0f;
        invisibilityTimer = 0f;

        // Make the ghost invisible and untouchable
        spriteRenderer.enabled = false;   // Hide sprite
        ghostCollider.enabled = false;    // Disable collisions
    }

    void BecomeVisible()
    {
        isInvisible = false;

        // Make the ghost visible and vulnerable again
        spriteRenderer.enabled = true;    // Show sprite
        ghostCollider.enabled = true;     // Enable collisions
    }

    public void TakeDamage(int damage)
    {
        // Ghost cannot take damage while invisible cause goofy
        if (!isInvisible)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
