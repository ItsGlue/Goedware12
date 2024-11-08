using System.Collections;
using UnityEngine;

public class VariableHandler : MonoBehaviour
{
    public static VariableHandler Instance { get; private set; }
    public int playerScore;           // Player's score
    public int playerLives;           // Player's lives
    public float gameTimer;           // Timer for the game
    public bool frozen = false;       // Flag to check if the game is frozen
    public int score;                 // Score variable
    public bool sword = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start() 
    {
        ResetVariables(); // Reset the variables each time the scene loads
        StartCoroutine(IncrementScoreEverySecond()); // Start the score increment coroutine
    }

    void Update() 
    {
        if (frozen) 
        {
            StartCoroutine(UnfreezeAfterSeconds(3f));
        }
    }

    public void FreezeForSeconds(float freezeDuration = 3f)
    {
        if (!frozen)
        {
            frozen = true;
            StartCoroutine(UnfreezeAfterSeconds(freezeDuration));
        }
    }

    private IEnumerator UnfreezeAfterSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        frozen = false;
        Debug.Log("Game has unfrozen!");
    }

    private IEnumerator IncrementScoreEverySecond()
    {
        while (true) // Infinite loop to keep adding score every second
        {
            yield return new WaitForSeconds(1f); // Wait for 1 second

            // Check if the player exists
            if (GameObject.FindGameObjectWithTag("Player") != null) // Assuming your player GameObject is tagged as "Player"
            {
                score += 1; // Increment the score
                Debug.Log("Score: " + score); // Optional: Log the current score
            }
        }
    }

    // Example function to modify variables
    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
    }

    // Resets variables whenever the scene is reloaded
    private void ResetVariables()
    {
        playerScore = 0;
        playerLives = 3;  // Assuming 3 lives by default
        gameTimer = 0f;
        frozen = false;
        score = 0;
        sword = true;
    }
}
