using UnityEngine;
using UnityEngine.SceneManagement; // Make sure to include this

public class SceneChanger : MonoBehaviour
{
    // Function to change the scene to "Game"
    public void ChangeSceneToGame()
    {
        // Check if the "Game" scene exists
        if (Application.CanStreamedLevelBeLoaded("Game"))
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            Debug.LogError("Scene 'Game' cannot be found!");
        }
    }
}
