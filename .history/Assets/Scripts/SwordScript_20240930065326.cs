using System.Collections;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;                       // Reference to the player transform
    public GameObject bow;                         // Reference to the bow GameObject
    public float swingSpeed = 200f;                // Speed of the sword swing
    public float startAngleOffset = 45f;          // Offset for the start angle of the swing
    public float endAngleOffset = 45f;            // Offset for the end angle of the swing
    public float knockbackForce = 5f;              // Force of the knockback
    public float knockbackDuration = 0.5f;         // Duration for which enemy movement is disabled

    private SpriteRenderer swordRenderer;          // Reference to the sword's sprite renderer
    private BoxCollider2D swordCollider;           // Reference to the sword's collider
    private bool isSwinging = false;               // Flag to check if the sword is currently swinging
    private float startAngle;                       // Start angle of the swing
    private float endAngle;                         // End angle of the swing
    private float currentAngle;                     // Current angle of the sword during the swing
    private float swingDirection;                   // Direction of the swing

    void Start()
    {
        swordRenderer = GetComponent<SpriteRenderer>();
        swordCollider = GetComponent<BoxCollider2D>();

        swordRenderer.enabled = false;              // Initially disable the sword renderer
        swordCollider.enabled = false;              // Initially disable the sword collider
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check if the sword is not active
        if (!VariableHandler.Instance.sword)
        {
            // Hide the sword if it is not equipped
            swordRenderer.enabled = false;           // Ensure sprite is hidden
            swordCollider.enabled = false;           // Ensure collider is hidden
            return; // Exit the Update method early
        }

        // Only allow swinging when the sword is active
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
        Vector2 directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        float midpointAngle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
        startAngle = midpointAngle - startAngleOffset;
        endAngle = midpointAngle + endAngleOffset;
        currentAngle = startAngle;
        transform.position = player.position;         // Set the sword's position to the player's position
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));
        swordRenderer.enabled = true;                 // Enable sword renderer
        swordCollider.enabled = true;                 // Enable sword collider

        if (bow != null)
        {
            //bow.SetActive(false);                     // Hide the bow while swinging
        }

        isSwinging = true;                            // Set swinging flag to true
    }

    private void SwingSword()
    {
        float step = swingSpeed * Time.deltaTime * swingDirection;
        currentAngle += step;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

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
        isSwinging = false;                           // Reset swinging flag
        swordRenderer.enabled = false;                // Disable sword renderer
        swordCollider.enabled = false;                // Disable sword collider

        // Delay switching back to the bow to ensure the swing is fully finished
        StartCoroutine(DelaySwitchToBow());
    }

    private IEnumerator DelaySwitchToBow()
    {
        // Wait for a brief moment to ensure the swing is fully complete
        yield return new WaitForSeconds(0.1f);      // Adjust this duration if needed

        if (bow != null)
        {
            bow.SetActive(true);                      // Show the bow again after the swing
        }
    }

    // Detect collision with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("SwordEnemy"))
        {
            Rigidbody2D enemyRigidbody = collision.GetComponent<Rigidbody2D>();
            MonoBehaviour enemyMovement = collision.GetComponent<MonoBehaviour>(); // Assuming the movement script is a MonoBehaviour

            if (enemyRigidbody != null)
            {
                // Disable enemy movement script
                if (enemyMovement != null)
                {
                    enemyMovement.enabled = false;
                }

                // Calculate knockback direction (away from the player)
                Vector2 knockbackDirection = collision.transform.position - player.position;
                knockbackDirection.Normalize();

                // Apply force to enemy
                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                // Re-enable movement after knockback duration
                StartCoroutine(ReenableMovementAfterDelay(enemyMovement));
            }
        }
    }

    private IEnumerator ReenableMovementAfterDelay(MonoBehaviour enemyMovement)
    {
        yield return new WaitForSeconds(knockbackDuration);
        
        // Re-enable the enemy movement script
        if (enemyMovement != null)
        {
            enemyMovement.enabled = true;
        }
    }
}
