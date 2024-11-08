using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 lastPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position; // Store initial position
    }

    private void Update()
    {
        // Check if the enemy has moved horizontally
        float movementDirection = transform.position.x - lastPosition.x;

        // Flip the sprite based on the movement direction
        if (movementDirection > 0.1f) // Moving right
        {
            spriteRenderer.flipX = false;
        }
        else if (movementDirection < -0.1f) // Moving left
        {
            spriteRenderer.flipX = true;
        }

        // Update last position for the next frame
        lastPosition = transform.position;
    }
}
