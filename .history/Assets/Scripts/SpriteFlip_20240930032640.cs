using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 lastPosition;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; // Initialize last position
    }

    private void Update()
    {
        FlipSpriteBasedOnDirection();
    }

    void FlipSpriteBasedOnDirection()
    {
        Vector2 currentPosition = transform.position;
        float direction = currentPosition.x - lastPosition.x;

        // Check if the enemy is moving left or right
        if (direction > 0.1f)
        {
            spriteRenderer.flipX = false;  // Moving right
        }
        else if (direction < -0.1f)
        {
            spriteRenderer.flipX = true;   // Moving left
        }

        lastPosition = currentPosition;  // Update last position
    }
}
