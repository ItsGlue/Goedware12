using System.Collections;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;
    public GameObject bow;
    public float swingSpeed = 200f;
    public float startAngleOffset = 45f;
    public float endAngleOffset = 45f;
    public float knockbackForce = 5f; // Force of the knockback
    public float knockbackDuration = 0.5f; // Time enemy movement is disabled

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
        if (VariableHandler.Instance.sword) {
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
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));
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
        isSwinging = false;
        swordRenderer.enabled = false;
        swordCollider.enabled = false;

        if (bow != null)
        {
            bow.SetActive(true);
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
