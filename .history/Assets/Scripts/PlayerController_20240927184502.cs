using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed

    private Vector2 movement;

    void Update()
    {
        // Get input from the player (WASD or Arrow keys)
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        transform.Translate(movement * speed * Time.deltaTime);
    }
}
