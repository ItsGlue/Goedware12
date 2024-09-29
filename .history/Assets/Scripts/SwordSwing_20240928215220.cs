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
    private Transform player;     
    private Vector3 swordOffsetRight = new Vector3(1f, 0f, 0f); 
    private Vector3 swordOffsetLeft = new Vector3(-1f, 0f, 0f); 

    void Start()
    {
        player = transform.parent; 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;  
        }

        UpdateSwordPosition();

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
                swingingForward = true;
            }
        }
    }

    void UpdateSwordPosition()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            if (playerController.movement.x > 0) 
            {
                transform.localPosition = swordOffsetRight;
                transform.localRotation = Quaternion.Euler(0f, 0f, currentSwingAngle); 
            }
            else if (playerController.movement.x < 0)
            {
                transform.localPosition = swordOffsetLeft;
                transform.localRotation = Quaternion.Euler(0f, 180f, currentSwingAngle); 
            }
        }
    }
}
