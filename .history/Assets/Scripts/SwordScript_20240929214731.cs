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

        if (enemyRigidbody != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - player.position).normalized;
            StartCoroutine(ApplyKnockback(enemyRigidbody, knockbackDirection));
        }
    }
}

private IEnumerator ApplyKnockback(Rigidbody2D enemyRigidbody, Vector2 knockbackDirection)
{
    // Set velocity for knockback
    enemyRigidbody.velocity = knockbackDirection * knockbackForce;

    // Disable enemy movement for a short period (e.g., 0.2 seconds)
    // Assuming there's a movement script you might need to disable
    // Example: enemyMovement.enabled = false;

    yield return new WaitForSeconds(0.2f);

    // Re-enable movement after knockback is applied
    // Example: enemyMovement.enabled = true;
}



}



