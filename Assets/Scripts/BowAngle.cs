using UnityEngine;

public class BowAngle : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // Ensure correct Z depth
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate direction from the player to the mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        // Rotate the bow to face the mouse direction with a 90-degree offset
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Subtract 90 degrees to rotate right

        // Set the bow position relative to the player
        transform.position = player.transform.position;
    }
}
