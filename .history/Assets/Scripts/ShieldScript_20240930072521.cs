using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public GameObject shield; 
    public void ActivateAndDeactivate(float delay = 3f)
    {
        shield.SetActive(true); // Activate the GameObject
        StartCoroutine(DeactivateAfterDelay(delay)); // Start the coroutine to deactivate
    }

    // Coroutine to deactivate the GameObject after a specified delay
    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        gameObject.SetActive(false); // Deactivate the GameObject
    }
}
