using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // The name of the scene you want to load
    public string targetSceneName;
    public string targetPortalName;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the portal has a specific tag (e.g., "Player")
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("TargetPortalName", targetPortalName);

            // Load the target scene
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
