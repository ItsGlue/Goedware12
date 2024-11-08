using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{

    void Start()
    {
        
    }


     private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collider is not the Player
        if (collision.gameObject.CompareTag("Shield")) {
            
        }
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
        {
            if (collision.gameObject.CompareTag("Player")) {
                Destroy(gameObject); // Destroy the arrow upon collision with anything that's not the Player
                PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ChangeHealth(-10); // Apply damage to the player
                }
            }
            
        }
    }

//     private void OnCollisionEnter2D(Collision2D collision)
// {
//     // Check if the collider is not the Player
//     if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shield"))
//     {
//         Destroy(gameObject); // Destroy the arrow upon collision with anything that's not the Player
//         PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
//             if (playerController != null)
//             {
//                 playerController.ChangeHealth(-10); // Apply damage to the player
//             }
//     }
// }

}
