using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;
    public float swingSpeed = 200f;
    public float startAngleOffset = 45f;
    public float endAngleOffset = 45f;

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

        swordRenderer.enabled = false;      // Sword starts hidden
        swordCollider.enabled = false;      // Collider starts disabled
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging) // Check for left mouse button click
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
        // Calculate the midpoint angle based on the player's position and cursor
        Vector2 directionToCursor = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        float midpointAngle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;

        // Determine the start and end angles relative to the midpoint angle
        startAngle = midpointAngle - startAngleOffset;
        endAngle = midpointAngle + endAngleOffset;
        currentAngle = startAngle;

        // Set the initial rotation of the sword
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);

        // Determine the direction of the swing (1 for clockwise, -1 for counterclockwise)
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));

        // Enable the sword, collider and start swinging
        swordRenderer.enabled = true;
        swordCollider.enabled = true;
        isSwinging = true;
    }

    private void SwingSword()
    {
        // Calculate the angle to rotate based on the swing speed and direction
        float step = swingSpeed * Time.deltaTime * swingDirection;
        currentAngle += step;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Check if the sword has reached or passed the end angle
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
        swordRenderer.enabled = false;  // Hide the sword after the swing
        swordCollider.enabled = false;  // Disable the collider after the swing
    }
}
