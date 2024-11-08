using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;
    public GameObject bow;
    public float swingSpeed = 200f;
    public float startAngleOffset = 45f;
    public float endAngleOffset = 45f;
    public float knockbackForce = 5f;
    public LayerMask enemyLayer;  // Layer to detect enemies

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

        // Continuously check for enemies within the sword's collider
        CheckForEnemies();

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

    // Check for enemies within the sword's collider and apply knockback
    private void CheckForEnemies()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(transform.position, swordCollider.size, currentAngle, enemyLayer);

        foreach (Collider2D enemy in enemiesHit)
        {
            Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();

            if (enemyRigidbody != null)
            {
                Vector2 knockbackDirection = enemy.transform.position - player.position;
                knockbackDirection.Normalize();

                enemyRigidbody.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    // Draw the box collider for debugging in Scene view
    private void OnDrawGizmos()
    {
        if (swordCollider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, swordCollider.size);
        }
    }
}
