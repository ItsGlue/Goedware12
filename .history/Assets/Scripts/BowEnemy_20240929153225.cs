using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    public float fireRateInSeconds = 2f;
    public float projectileLifetime = 5f;

    private Transform player;
    private float fireCooldown;

    void Start()
    {
        fireCooldown = fireRateInSeconds;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            RotateTowardsPlayer();
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0)
            {
                Shoot();
                fireCooldown = fireRateInSeconds;
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void Shoot()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            rb.velocity = direction * projectileSpeed;

            Destroy(projectile, projectileLifetime);
        }
    }
}
