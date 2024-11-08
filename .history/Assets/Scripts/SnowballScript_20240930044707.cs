using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    public int damage = 10;         
    public float lifeTime = 5f;      // Time before the snowball is destroyed
    public Transform player;         // Reference to the player transform

    void Start()
    {
        // Destroy snowball after a certain time
        Destroy(gameObject, lifeTime);
        
        // Find the player GameObject if not already set
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // Rotate the snowball to face the player
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // Adjust the angle by 90 degrees
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            // You can apply damage logic here if needed

            // Destroy the snowball when it hits the player
            Destroy(gameObject);
        }
    }
}