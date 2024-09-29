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

    void Start()
    {
        swordRenderer = GetComponent<Renderer>();
        initialRotation = transform.rotation;
        swordRenderer.enabled = false;
    }

    void Update()
    {
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
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, -maxSwingAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isSwinging = false;
            swordRenderer.enabled = false;
            transform.rotation = initialRotation;  // Reset the rotation for the next swing
        }
    }
}
