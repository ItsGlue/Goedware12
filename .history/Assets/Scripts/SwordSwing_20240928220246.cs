using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingSpeed = 5f;
    public float maxSwingAngle = 45f;
    private float currentSwingAngle = 0f;
    private bool swingingForward = true;
    private bool isSwinging = false;
    private Renderer swordRenderer;

    void Start()
    {
        swordRenderer = GetComponent<Renderer>();
        swordRenderer.enabled = false; // Hide the sword at the start
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            swordRenderer.enabled = true;  // Show the sword
            isSwinging = true;
            currentSwingAngle = 0f;
            swingingForward = true;
        }

        if (isSwinging)
        {
            PerformSwing();
        }
    }

    void PerformSwing()
    {
        float targetSwingAngle = swingingForward ? maxSwingAngle : 130f;

        currentSwingAngle = Mathf.LerpAngle(currentSwingAngle, targetSwingAngle, swingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, currentSwingAngle);

        if (Mathf.Abs(currentSwingAngle - targetSwingAngle) < 1f)
        {
            if (swingingForward)
            {
                swingingForward = false;
            }
            else
            {
                isSwinging = false;
                swordRenderer.enabled = false;  // Hide the sword after the swing
            }
        }
    }
}
