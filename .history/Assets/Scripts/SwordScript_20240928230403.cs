using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public Transform player;                // Reference to the player's transform
    public float swingSpeed = 200f;         // Speed of the swinging motion
    public float startAngleOffset = 45f;    // Degrees offset from the midpoint (cursor angle)
    public float endAngleOffset = 45f;      // Degrees offset on the other side of the midpoint

    private SpriteRenderer swordRenderer;   // Reference to the sword's sprite renderer
    private bool isSwinging = false;        // Whether the sword is currently swinging
    private float startAngle;               // The starting angle of the swing
    private float endAngle;                 // The ending angle of the swing
    private float currentAngle;             // Current angle of the sword during the swing
    private float swingDirection;           // Direction of the swing (1 for clockwise, -1 for counterclockwise)

    void Start()
    {
        swordRenderer = GetComponent<SpriteRenderer>();
        swordRenderer.enabled = false;      // Sword starts hidden
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

        // Adjust the midpoint angle by 180 degrees to make the sword face the cursor correctly
        midpointAngle -= 90f; // Adjust this value if the sword still doesn't face the correct direction

        // Determine the start and end angles relative to the adjusted midpoint angle
        startAngle = NormalizeAngle(midpointAngle - startAngleOffset);
        endAngle = NormalizeAngle(midpointAngle + endAngleOffset);
        currentAngle = startAngle;

        // Set the initial rotation of the sword
        transform.position = player.position;
        transform.rotation = Quaternion.Euler(0, 0, startAngle);

        // Determine the direction of the swing (1 for clockwise, -1 for counterclockwise)
        swingDirection = Mathf.Sign(Mathf.DeltaAngle(startAngle, endAngle));

        // Enable the sword and start swinging
        swordRenderer.enabled = true;
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
        swordRenderer.enabled = false; // Hide the sword after the swing
    }

    // Normalize the angle to be between 0 and 360 degrees
    private float NormalizeAngle(float angle)
    {
        return (angle + 360f) % 360f;
    }
}
