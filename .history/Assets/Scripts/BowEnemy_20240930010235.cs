using System.Collections;
using UnityEngine;

public class EnemyWanderAndRun : MonoBehaviour
{
    public float wanderSpeed = 1f;         // Speed while wandering
    public float sprintSpeed = 5f;          // Speed while sprinting away from the player
    public float detectionRadius = 5f;      // Radius to detect the player
    public float wanderRange = 10f;         // Range to wander
    public int healthReward = 20;           // Health to give to the player upon death

    private Transform player;
    private Vector2 wanderTarget;
    private bool isWandering = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetNewWanderTarget();
    }

    private void Update()
    {
        if (isWandering)
        {
            Wander();
            CheckForPlayer();
        }
    }

    void Wander()
    {
        // Move towards the wander target
        transform.position = Vector2.MoveTowards(transform.position, wanderTarget, wanderSpeed * Time.deltaTime);

        // Check if reached the wander target
        if (Vector2.Distance(transform.position, wanderTarget) < 0.1f)
        {
            SetNewWanderTarget();
        }
    }

    void SetNewWanderTarget()
    {
        // Set a new random target within the wander range
        wanderTarget = new Vector2(
            transform.position.x + Random.Range(-wanderRange, wanderRange),
            transform.position.y + Random.Range(-wanderRange, wanderRange)
        );
    }

    void CheckForPlayer()
    {
        // Check if the player is within detection radius
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            // Sprint away from the player
            SprintAwayFromPlayer();
        }
    }

    void SprintAwayFromPlayer()
    {
        // Calculate the direction away from the player
        Vector2 directionAwayFromPlayer = (transform.position - player.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + directionAwayFromPlayer, sprintSpeed * Time.deltaTime);
        isWandering = false;  // Stop wandering when sprinting
    }

    public void TakeDamage(int damage)
    {
        // Implement damage logic here (e.g., reduce health)

        // If health reaches zero, call Die method
        Die();
    }

    void Die()
    {
        // Reward player with health
        PlayerHealth.Instance.RestoreHealth(healthReward);  // Assumes you have a PlayerHealth script with a RestoreHealth method

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
