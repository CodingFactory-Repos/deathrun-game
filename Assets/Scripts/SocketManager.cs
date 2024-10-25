using System;
using System.Threading.Tasks;
using UnityEngine;
using SocketIOClient;
using CandyCoded.env;
using System.IO;
using System.Collections.Generic;

public class SocketManager : MonoBehaviour
{
    private static SocketManager _instance;
    public static SocketManager Instance => _instance;

    public SocketIOUnity ClientSocket { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // Keep the socket alive across scenes
            SetupSocket().ConfigureAwait(false); // Ensure proper async handling
        }
    }

    private void LoadEnvVariables()
    {
        string envFilePath = Path.Combine(Application.dataPath, ".env");

        if (File.Exists(envFilePath))
        {
            foreach (var line in File.ReadAllLines(envFilePath))
            {
                if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                {
                    var keyValue = line.Split('=');
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        Environment.SetEnvironmentVariable(key, value);
                    }
                }
            }
        }
        else
        {
            Debug.LogError(".env file not found at " + envFilePath);
        }
    }

    private async Task SetupSocket()
    {

        LoadEnvVariables(); 

        try
        {
            var socketUrl = Environment.GetEnvironmentVariable("SOCKET_URL");
            var uri = new Uri(socketUrl);
            ClientSocket = new SocketIOUnity(uri);

            // Connect to the server
            await ClientSocket.ConnectAsync();
            Debug.Log("Connected to backend via SocketManager.");
        }
        catch (Exception e)
        {
            Debug.LogError("Socket connection error: " + e.Message);
        }

        try
        {
            env.TryParseEnvironmentVariable("SOCKET_URL", out string socketUrl);
            var uri = new Uri(socketUrl);
            ClientSocket = new SocketIOUnity(uri);

            // Connect to the server
            await ClientSocket.ConnectAsync();
            Debug.Log("Connected to backend via SocketManager.");
        }
        catch (Exception e)
        {
            Debug.LogError("Socket connection error: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (ClientSocket != null)
        {
            ClientSocket.Dispose(); // Properly close the connection
        }
    }
}
