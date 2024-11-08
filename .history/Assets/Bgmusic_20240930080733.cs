using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip backgroundMusic; // Assign your music clip in the Inspector
    private AudioSource audioSource;   // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component and set up the audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic; // Set the audio clip
        audioSource.loop = true;             // Enable looping
        audioSource.Play();                  // Start playing the music
    }

    void OnDestroy()
    {
        // Optional: Stop the music when this GameObject is destroyed
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
