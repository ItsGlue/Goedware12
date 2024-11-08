using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public float speed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * speed);
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
    if (!collision.gameObject.CompareTag("Player"))
    {
        Destroy(gameObject); // Destroy the arrow upon collision with anything that's not the Player
    }
}

}
