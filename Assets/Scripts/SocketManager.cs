using System;
using System.Threading.Tasks;
using UnityEngine;
using SocketIOClient;
using CandyCoded.env;

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

    private async Task SetupSocket()
    {
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
