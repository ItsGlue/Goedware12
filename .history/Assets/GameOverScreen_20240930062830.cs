using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void setUp() {
        gameObject.SetActive(true);
    }

    public void RestartScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();
        
        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }
}
