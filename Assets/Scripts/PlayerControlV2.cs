using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlV2 : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    public float moveSpeed = 10;
    public float inputHorizontal;
    public bool facingRight = true;
    public bool isFlipped = true;
    public float orientation = 1;
    public float score;
    public float maxHealth;
    public float currHealth;
    public Vector2 playerInput;
    public bool isDead = false;
    private float damageCooldown = 1.0f; 
    private float nextDamageTime = 0.0f; 
    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        rb = GetComponent<Rigidbody2D>();
        maxHealth = 9;
        currHealth = maxHealth;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = playerInput.normalized * moveSpeed * Time.deltaTime;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (inputHorizontal > 0 && !facingRight) {
            flip();
            
        } else if (inputHorizontal < 0 && facingRight) {
            flip();
            
        } 

        if (currHealth <= 0) {
            die();
        }
    }
    void die() {
        isDead = true;
     // Code this later   Destroy(Player);
    }
    void flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        orientation *= -1;
        facingRight = !facingRight;
        isFlipped = !isFlipped;
    }
    
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime) {
            currHealth -= 3;
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}
