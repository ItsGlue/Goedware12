using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float speed = 5f;

    private Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        transform.Translate(movement * speed * Time.deltaTime).norm;
    }
}
