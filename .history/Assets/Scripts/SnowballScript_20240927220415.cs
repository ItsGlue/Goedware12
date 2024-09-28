using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    public int damage = 10;         
    public float lifeTime = 5f;      // Time before the snowball is destroyed

    void Start()
    {
        Destroy(gameObject, lifeTime);  // Destroy snowball after a certain time
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy the snowball on collision
        if (collision.gameObject.CompareTag("Player")) {
Destroy(gameObject);
        }
        
    }
}
