using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingSpeed = 5f;
    public float maxSwingAngle = 45f;
    private bool isSwinging = false;
    private Renderer swordRenderer;
    private Quaternion initialRotation;
    public Transform player;  // Reference to the player's transform
    private bool facingRight = true;

    void Start()
    {
        swordRenderer = GetComponent<Renderer>();
        initialRotation = transform.rotation;
        swordRenderer.enabled = false;
    }

    void Update()
    {
        // Determine which direction the player is facing
        if (player.localScale.x > 0)
        {
            facingRight = true;
        }
        else
        {
            facingRight = false;
        }

        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            swordRenderer.enabled = true;
            isSwinging = true;
        }

        if (isSwinging)
        {
            PerformSwing();
        }
    }

    void PerformSwing()
    {
        // Adjust the swing direction based on the player's facing direction
        float swingDirection = facingRight ? -maxSwingAngle : maxSwingAngle;

        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, swingDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isSwinging = false;
            swordRenderer.enabled = false;
            transform.rotation = initialRotation;  // Reset the rotation for the next swing
        }
    }
}
