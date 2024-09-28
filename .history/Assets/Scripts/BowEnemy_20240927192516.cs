using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRateInSeconds = 2f;
    public float projectileLifetime = 5f;

    private float fireCooldown;

    void Start()
    {
        fireCooldown = fireRateInSeconds;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
            Shoot();
            fireCooldown = fireRateInSeconds;
        }
    }

    void Shoot()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            
            // Rotate projectile to point towards the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            rb.velocity = direction * projectileSpeed;

            // Destroy the projectile after a few seconds
            Destroy(projectile, projectileLifetime);
        }
    }
}
