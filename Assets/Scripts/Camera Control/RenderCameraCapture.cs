using UnityEngine;
using System.IO;

public class RenderCameraCapture : MonoBehaviour
{
    public Camera renderCamera; 
    public Camera mainCamera; 
    private RenderTexture renderTexture;
    private Texture2D texture;
    private float timeSinceLastCapture = 0f;
    private float captureInterval = 0.1f; 

    static ulong frameIndex= 0; 

    void Start()
    {
        int width = 1280;
        int height = 720;

        renderTexture = new RenderTexture(width, height, 24);
        renderCamera.targetTexture = renderTexture;

        texture = new Texture2D(width, height, TextureFormat.RGB24, false);
    }

    void Update()
    {
     
        if (mainCamera != null && renderCamera != null)
        {
            renderCamera.transform.position = mainCamera.transform.position;
            renderCamera.transform.rotation = mainCamera.transform.rotation;
        }

        timeSinceLastCapture += Time.deltaTime;

        if (timeSinceLastCapture >= captureInterval)
        {
            timeSinceLastCapture = 0f;

            frameIndex +=1;
            RenderTexture.active = renderTexture;
            renderCamera.Render();
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();
            RenderTexture.active = null; 

            SaveFrameAsImage(frameIndex);
        }
    }

    void SaveFrameAsImage(ulong frameIndex)
    {
        byte[] imageBytes = texture.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.dataPath, $"frame_{frameIndex}.png"), imageBytes);
    }
}
