using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector2 playerInput;
    Rigidbody2D rb;
    public float moveSpeed;
    public float inputHorizontal;
    public bool facingRight = true;
    public bool isFlipped = true;
    public float orientation = 1;
    public float score;
    public float maxHealth;
    public float currHealth;
    public GameObject Player;

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
        rb.velocity = playerInput.normalized * moveSpeed;

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
        Destroy(Player);
    }
    void flip() {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;
        orientation *= -1;
        facingRight = !facingRight;
        isFlipped = !isFlipped;
    }

    void IncrementScore(int amt) {
        score += amt;
    }
    
    
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime) {
            currHealth -= 3;
            nextDamageTime = Time.time + damageCooldown;
        }
    }
}