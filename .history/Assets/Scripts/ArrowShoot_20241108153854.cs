using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    public GameObject bullet;                        // Reference
    public Camera mainCamera;                        // Reference to the main camera
    public SpriteRenderer spriteRenderer;            // Reference to the sprite renderer for the shooting sprite
    public Sprite spriteOnCooldown;                  // Sprite to show when on cooldown
    public Sprite spriteNotOnCooldown;               // Sprite to show when not on cooldown
    public float cooldownDuration = 1.0f;            // Duration of the cooldown

    private bool isOnCooldown = false;               // Flag to check if the shooting is on cooldown
    private float cooldownTimer = 0f;                // Timer for the cooldown

    void Update()
    {
        if (VariableHandler.Instance.sword) 
        {
            spriteRenderer.enabled = false; 
            return; 
        }
        
        spriteRenderer.enabled = true;

        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime; 
            if (cooldownTimer <= 0)
            {
                isOnCooldown = false; // Reset cooldown flag
                spriteRenderer.sprite = spriteNotOnCooldown; // Reset to normal sprite when off cooldown
            }
        }

        // Handle shooting input
        if (Input.GetMouseButtonDown(0) && !isOnCooldown)
        {
            ShootArrow(); // Call the method to shoot the arrow
            StartCooldown(); // Start the cooldown
        }
    }

    void ShootArrow()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure z-position is zero

        Vector3 direction = (mousePosition - transform.position).normalized; // Calculate direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Calculate angle
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Create rotation

        Instantiate(bullet, transform.position, rotation); // Instantiate the bullet
    }

    void StartCooldown()
    {
        isOnCooldown = true; // Set cooldown flag
        cooldownTimer = cooldownDuration; // Reset cooldown timer
        spriteRenderer.sprite = spriteOnCooldown; // Change sprite to cooldown sprite
    }
}
