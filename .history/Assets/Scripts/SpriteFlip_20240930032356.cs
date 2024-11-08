using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    public Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FlipSpriteBasedOnDirection();
    }

    void FlipSpriteBasedOnDirection()
    {
        if (rb.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }
}
