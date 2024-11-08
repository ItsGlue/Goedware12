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
    }

    void Update()
    {
        // Make the snowball face the player
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
