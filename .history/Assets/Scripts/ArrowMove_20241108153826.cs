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
        if (!collision.CompareTag("Player") && !collision.CompareTag("Shield") && !collision.CompareTag("EnemyProjectile"))
        {
            Destroy(gameObject);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    // Check if the collider is not the Player
    if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Shield"))
    {
         
        Destroy(gameObject);
    }
}

}
