using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingSpeed = 5f;
    public float maxSwingAngle = 45f;
    private bool swingingForward = true;
    private bool isSwinging = false;
    private Renderer swordRenderer;
    private Quaternion initialRotation;

    void Start()
    {
        swordRenderer = GetComponent<Renderer>();
        swordRenderer.enabled = false;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            swordRenderer.enabled = true;
            isSwinging = true;
            swingingForward = true;
        }

        if (isSwinging)
        {
            PerformSwing();
        }
    }

    void PerformSwing()
    {
        float targetSwingAngle = swingingForward ? -maxSwingAngle : 0f;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, targetSwingAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, swingSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            if (swingingForward)
            {
                swingingForward = false;
            }
            else
            {
                isSwinging = false;
                swordRenderer.enabled = false;
            }
        }
    }
}
