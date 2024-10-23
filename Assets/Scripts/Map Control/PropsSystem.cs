using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using SocketIOClient;
using CandyCoded.env;

public class PropsManager : MonoBehaviour
{
    private Tilemap propsTilemap;  // Reference to the Tilemap component
    private SocketIOUnity clientSocket;
    private bool PropsSent = false;

    private void Start()
    {
        clientSocket = SocketManager.Instance.ClientSocket;
        InvokeRepeating(nameof(CheckSocketAndSendProps), 0f, 5f); // Check connection every 5 seconds
    }

    // Check if the socket is connected before sending props
    private async void CheckSocketAndSendProps()
    {
        Debug.Log("Checking socket connection...");

        if (!PropsSent && clientSocket != null && clientSocket.Connected)
        {
            await FindAndSendProps();
        }
        else
        {
            Debug.LogWarning("Socket not connected, skipping props send.");
        }
    }

    // Find the grid and the props tilemap, then send props positions to the server
    private async Task FindAndSendProps()
    {
        GameObject grid = GameObject.Find("Grid");

        if (grid != null)
        {
            propsTilemap = grid.transform.Find("Props")?.GetComponent<Tilemap>();

            if (propsTilemap != null)
            {
                List<Vector3> propsPositions = GetPropsPositions();

                if (propsPositions.Count > 0)
                {
                    List<string> propsPositionsString = ConvertPositionsToString(propsPositions);
                    await EmitPropsToServer(propsPositionsString);

                    CancelInvoke(nameof(CheckSocketAndSendProps)); // Stop repeating checks after sending
                }
            }
            else
            {
                Debug.LogError("Props Tilemap not found in Grid!");
            }
        }
        else
        {
            Debug.LogError("Grid object not found!");
        }
    }

    // Get all positions where there are props on the Tilemap
    private List<Vector3> GetPropsPositions()
    {
        BoundsInt bounds = propsTilemap.cellBounds;
        List<Vector3> propsPositions = new List<Vector3>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (propsTilemap.HasTile(tilePosition))
                {
                    propsPositions.Add(tilePosition);
                }
            }
        }

        Debug.Log($"Total props found: {propsPositions.Count}");
        return propsPositions;
    }

    // Convert Vector3 positions to strings
    private List<string> ConvertPositionsToString(List<Vector3> propsPositions)
    {
        List<string> propsPositionsString = new List<string>();

        foreach (Vector3 position in propsPositions)
        {
            string positionString = $"{position.x}, {position.y}";
            propsPositionsString.Add(positionString);
        }

        return propsPositionsString;
    }

    // Emit the props positions to the server
    private async Task EmitPropsToServer(List<string> propsPositionsString)
    {
        try
        {
            Debug.Log("Emitting props to server...");
            await clientSocket.EmitAsync("props:send", propsPositionsString);
            PropsSent = true;
            Debug.Log("Props successfully sent to server.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to emit props: " + e.Message);
        }
    }
}
