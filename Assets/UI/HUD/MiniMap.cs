using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Camera miniMapCamera;  // Assign your mini-map camera in the Inspector
    public RawImage miniMapImage; // Assign your Raw Image component in the Inspector

    void Start()
    {
        if (miniMapCamera != null && miniMapImage != null)
        {
            RenderTexture renderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            miniMapCamera.targetTexture = renderTexture;
            miniMapImage.texture = renderTexture;
        }
        else
        {
            Debug.LogError("Mini-map setup is incomplete. Assign miniMapCamera and miniMapImage.");
        }
    }
}
