using System.Diagnostics;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject gridObject;  

    void Start()
    {
            AdjustCameraToGrid();
    }

    void AdjustCameraToGrid()
    {
        Renderer gridRenderer = gridObject.GetComponent<Renderer>();

        if (gridRenderer != null)
        {
            Bounds gridBounds = gridRenderer.bounds;

            float gridWidth = gridBounds.size.x;
            float gridHeight = gridBounds.size.y;

            Vector3 gridCenter = gridBounds.center;

            mainCamera.transform.position = new Vector3(gridCenter.x, gridCenter.y, mainCamera.transform.position.z);
            float aspectRatio = (float)Screen.width / (float)Screen.height;

            if (gridWidth / aspectRatio > gridHeight)
            {
                mainCamera.orthographicSize = (gridWidth / 2f) / aspectRatio;
            }
            else
            {
                mainCamera.orthographicSize = gridHeight / 2f;
            }
        }
    }
}
