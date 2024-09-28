using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingSpeed = 5f;         // Speed of swinging motion
    public float maxSwingAngle = 45f;     // Maximum angle the sword swings to

    private float currentSwingAngle;      // Current rotation of the sword
    private bool swingingRight = true;    // Determines if swinging to the right or left
    private bool isSwinging = false;      // Flag to indicate if sword is swinging

    void Update()
    {
        // Trigger the sword swing on left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            SwingSword(); // Start swinging when the function is called
        }

        // Continue swinging if the sword is currently in motion
        if (isSwinging)
        {
            PerformSwing();
        }
    }

    void SwingSword()
    {
        if (!isSwinging)
        {
            isSwinging = true;  // Start the swing
        }
    }

    void PerformSwing()
    {
        // Determine the target angle based on the current swing direction
        float targetSwingAngle = swingingRight ? maxSwingAngle : -maxSwingAngle;

        // Smoothly interpolate the sword's rotation towards the target angle
        currentSwingAngle = Mathf.LerpAngle(currentSwingAngle, targetSwingAngle, swingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, currentSwingAngle);

        // Check if the sword has reached the target angle (with a small threshold to avoid stalling)
        if (Mathf.Abs(currentSwingAngle - targetSwingAngle) < 0.1f)
        {
            // Switch direction once the target angle is reached
            swingingRight = !swingingRight;

            // If it was swinging left and reaches the max left angle, stop swinging
            if (!swingingRight)
            {
                isSwinging = false;  // End the swing
            }
        }
    }
}
