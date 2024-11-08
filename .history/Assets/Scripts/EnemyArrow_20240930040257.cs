using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{

    void Start()
    {
        
    }


     private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is not the Player
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy the arrow upon collision with anything that's not the Player
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    // Check if the collider is not the Player
    if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
    {
        Destroy(gameObject); // Destroy the arrow upon collision with anything that's not the Player
    }
}

}
