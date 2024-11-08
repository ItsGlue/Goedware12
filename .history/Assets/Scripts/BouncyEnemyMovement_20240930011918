using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyEnemyMovement : MonoBehaviour
{
    // Horizontal movement speed
    public float speed = 5f;

    // Bouncing height and speed
    public float bounceHeight = 0.5f;
    public float bounceSpeed = 2f;

    // The original Y position of the enemy
    private float originalY;

    void Start()
    {
        // Store the original Y position of the enemy
        originalY = transform.position.y;
    }

    void Update()
    {
        // Horizontal movement
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Apply bounce effect on the Y axis using a sine wave
        float newY = originalY + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
