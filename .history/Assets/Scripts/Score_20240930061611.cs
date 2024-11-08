using UnityEngine;
using TMPro; // Make sure to include the TMP namespace

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshPro text component

    void Start()
    {
        // Get the TextMeshProUGUI component if not assigned in the inspector
        if (scoreText == null)
        {
            scoreText = GetComponent<TextMeshProUGUI>();
        }
    }

    void Update()
    {
        // Update the score text with the current score
        scoreText.text = "Score: " + VariableHandler.Instance.score; // Access the score from the VariableHandler
    }
}
