using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;
    public GameObject bow;
    public float swingSpeed = 200f;
    public float startAngleOffset = 45f;
    public float endAngleOffset = 45f;
    public float knockbackForce = 5f; // Force of the knockback

    private SpriteRenderer swordRenderer;
    private BoxCollider2D swordCollider;
    private bool isSwinging = false;
    private float startAngle;
    private float endAngle;
    private float currentAngle;
    private float swingDirection;

    void Start()
    {
        swordRenderer = GetComponent<SpriteRenderer>();
        swordCollider = GetComponent<BoxCollider2D>();

        swordRenderer.enabled = false;
        swordCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartSwing();
        }

        if (isSwinging)
        {
            SwingSword();
        }
    }

    private void StartSwing()
    {
        // Calculate the angle based on the mouse position relative to the player
        Vector2 directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)player.position;
        float midpointAngle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;

        // Set the start and end angles
        startAngle = midpointAngle - startAngleOffset;
        endAngle = midpointAngle + endAngleOffset;
        currentAngle = startAngle;

        // Position and rotate the sword at the start of the swing
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);

        // Determine the swing direction
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));

        // Enable sword visuals and collision
        swordRenderer.enabled = true;
        swordCollider.enabled = true;

        if (bow != null)
        {
            bow.SetActive(false);
        }

        isSwinging = true;
    }

    private void SwingSword()
    {
        // Move the sword's rotation during the swing
        float step = swingSpeed * Time.deltaTime * swingDirection;
        currentAngle += step;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Check if the swing has reached its end angle
        bool reachedEnd = swingDirection > 0 
            ? Mathf.DeltaAngle(currentAngle, endAngle) <= 0
            : Mathf.DeltaAngle(currentAngle, endAngle) >= 0;

        if (reachedEnd)
        {
            EndSwing();
        }
    }

    private void EndSwing()
    {
        isSwinging = false;

        // Disable sword visuals and collision
        swordRenderer.enabled = false;
        swordCollider.enabled = false;

        if (bow != null)
        {
            bow.SetActive(true);
        }

        // Reset the sword's position and rotation
        transform.rotation = Quaternion.identity;
    }

    // Detect collision with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("SwordEnemy"))
        {
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();

            if (enemyRigidbody != null)
            {
                // Calculate knockback direction (away from the player)
                Vector2 knockbackDirection = collision.transform.position - player.position;
                knockbackDirection.Normalize();

                // Apply force to enemy
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
