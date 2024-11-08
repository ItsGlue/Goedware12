using System.Collections;
using UnityEngine;

public class VariableHandler : MonoBehaviour
{
    public static VariableHandler Instance { get; private set; }
    public int playerScore;
    public int playerLives;
    public float gameTimer;
    public bool frozen = false;      
    public int score;               
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
            yield return new WaitForSeconds(1f); 

            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                score += 1;
                Debug.Log("Score: " + score);
            }
        }
    }

    public void UpdatePlayerScore(int score)
    {
        playerScore += score;
    }

    private void ResetVariables()
    {
        playerScore = 0;
        playerLives = 3;
        gameTimer = 0f;
        frozen = false;
        score = 0;
        sword = true;
    }
}
