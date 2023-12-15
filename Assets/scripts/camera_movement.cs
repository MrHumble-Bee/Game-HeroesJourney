using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player's transform
    public float distanceAbove = 12f; // Fixed distance above the player
    public float distanceBehind = 12f; // Fixed distance behind the player
    public float smoothTime = 0.3f;   // Smoothing time for camera movement

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (player != null)
        {
            // Calculate the desired position for the camera
            Vector3 targetPosition = new Vector3(
                player.position.x,
                player.position.y + distanceAbove,
                player.position.z + distanceBehind
            );

            // Smoothly move the camera towards the desired position using SmoothDamp
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Make the camera look at the player, only rotating on the y-axis
            // transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z), Vector3.up);
        }
    }
}
