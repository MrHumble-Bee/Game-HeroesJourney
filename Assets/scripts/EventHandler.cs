using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class EventHandler : MonoBehaviour
{
    void Start()
    {
        // Check if the target portal name is saved in PlayerPrefs
        if (PlayerPrefs.HasKey("TargetPortalName"))
        {
            // Get the target portal name
            string targetPortalName = PlayerPrefs.GetString("TargetPortalName");

            // Find the target portal GameObject in the scene
            GameObject targetPortal = GameObject.Find(targetPortalName);

            // Check if the target portal is found
            if (targetPortal != null)
            {
                // Move the player to the target portal's position
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 offset;
                    if (Mathf.Abs(targetPortal.transform.position.x) > Mathf.Abs(targetPortal.transform.position.z))
                    {
                        offset = new Vector3(-2 * (targetPortal.transform.position.x / Mathf.Abs(targetPortal.transform.position.x)),0.0f,0.0f);
                    }
                    else
                    {
                        offset = new Vector3(0.0f,0.0f,-2 * (targetPortal.transform.position.z / Mathf.Abs(targetPortal.transform.position.z)));
                    }
                    Debug.Log(targetPortal.transform.position.ToString());
                    Debug.Log(offset.ToString());
                    player.transform.position = targetPortal.transform.position + offset;
                }
            }

            // Remove the saved target portal name from PlayerPrefs
            PlayerPrefs.DeleteKey("TargetPortalName");
        }
    }

    private void ReconstructPlayerStats(Hero player)
    {
        
    }
}
