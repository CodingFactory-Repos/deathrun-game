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
    private bool isSocketConnected = false;

    private async void Start()
    {
        await SetupSocket();
        // Start checking the connection status regularly
        InvokeRepeating("CheckSocketAndSendProps", 0f, 5f);
    }

    // Set up the Socket.IO connection
    private async Task SetupSocket()
    {
        try
        {
            env.TryParseEnvironmentVariable("SOCKET_URL", out string socketUrl);
            var uri = new Uri(socketUrl);
            clientSocket = new SocketIOUnity(uri);

            // Corrected EventHandler for connection
            clientSocket.OnConnected += (sender, e) =>
            {
                isSocketConnected = true;
                Debug.Log("Socket connected.");
            };

            // Corrected EventHandler for disconnection
            clientSocket.OnDisconnected += (sender, reason) =>
            {
                isSocketConnected = false;
                Debug.LogWarning($"Socket disconnected: {reason}. Attempting to reconnect...");
                ReconnectSocket();
            };

            await clientSocket.ConnectAsync();

            if (clientSocket.Connected)
            {
                Debug.Log("Connected to backend.");
            }
            else
            {
                Debug.LogError("Failed to connect to backend.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Socket connection error: " + e.Message);
        }
    }

    // Attempt to reconnect the socket
    private async void ReconnectSocket()
    {
        while (!isSocketConnected)
        {
            try
            {
                Debug.Log("Reconnecting...");
                await clientSocket.ConnectAsync();
            }
            catch (Exception e)
            {
                Debug.LogError("Reconnect failed: " + e.Message);
            }

            await Task.Delay(5000);  // Wait for 5 seconds before retrying
        }
    }

    // Check if the socket is connected before sending props
    private void CheckSocketAndSendProps()
    {
        if (isSocketConnected)
        {
            FindAndSendProps();
        }
        else
        {
            Debug.LogWarning("Socket not connected, skipping props send.");
        }
    }

    // Find the grid and the props tilemap, then send props positions to the server
    private void FindAndSendProps()
    {
        GameObject grid = GameObject.Find("Grid");

        if (grid != null)
        {
            propsTilemap = grid.transform.Find("Props")?.GetComponent<Tilemap>();

            if (propsTilemap != null)
            {
                Debug.Log("Props Tilemap found!");
                List<Vector3> propsPositions = GetPropsPositions();

                if (propsPositions.Count > 0)
                {
                    List<string> propsPositionsString = ConvertPositionsToString(propsPositions);
                    EmitPropsToServer(propsPositionsString);
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
                    Debug.Log($"Prop found at grid position: {tilePosition}");
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
    private async void EmitPropsToServer(List<string> propsPositionsString)
    {
        try
        {
            await clientSocket.EmitAsync("props:current", propsPositionsString);
            Debug.Log("Props successfully sent to server.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to emit props: " + e.Message);
        }
    }
}
