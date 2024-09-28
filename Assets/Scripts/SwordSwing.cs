using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public float swingSpeed = 5f;         // Speed of swinging motion
    public float maxSwingAngle = 45f;     
    private float currentSwingAngle = 0f; 
    private bool swingingForward = true;  
    private bool isSwinging = false;
    public GameObject player;
    void Update()
    {
        Vector2 pos = new Vector2(player.transform.position.x + 0.5f, player.transform.position.y + 0.5f);
        transform.position = pos;

        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;  
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
                swingingForward = true; 
            }
        }
    }
}
