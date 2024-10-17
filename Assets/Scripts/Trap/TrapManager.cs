using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json.Linq;

public class TrapManager : MonoBehaviour
{
    public List<GameObject> trapPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> trapPrefabDictionary = new Dictionary<string, GameObject>();
    private int gridSizeX = 9;
    private int gridSizeY = 9;
    private string[,] grid;

    private Queue<TrapPlacement> placementQueue = new Queue<TrapPlacement>();
    private SocketIOUnity clientSocket; // SocketIO client

    void Start()
    {
        // Initialize the grid
        grid = new string[gridSizeX, gridSizeY];

        // Map trap names to their prefabs
        trapPrefabDictionary.Add("crossbow_down_prefab", trapPrefabs[0]);
        trapPrefabDictionary.Add("crossbow_up_prefab", trapPrefabs[1]);
        trapPrefabDictionary.Add("crossbow_side_prefab", trapPrefabs[2]);

        // Set up Socket.IO client
        SetupSocket();
        StartCoroutine(ProcessPlacementQueue());
    }

    async void SetupSocket()
    {
        try
        {
            var uri = new Uri("https://d4cc-86-245-196-157.ngrok-free.app"); // Replace with your backend URL
            clientSocket = new SocketIOUnity(uri);

            // Connect to the server
            await clientSocket.ConnectAsync();

            Debug.Log("Connected to backend and listening for trap placement events.");
        }
        catch (Exception e)
        {
            Debug.LogError("Socket connection error: " + e.Message);
        }
    }

    public void Update()
    {
        clientSocket.On("traps:place", response =>
        {
            JArray trapDataArray = JArray.Parse(response.ToString());

            foreach (var trapData in trapDataArray)
            {
                JObject trap = (JObject)trapData["trap"];

                int x = trap["x"].Value<int>();
                int y = trap["y"].Value<int>();
                string trapType = trap["trapType"].Value<string>();

                EnqueuePlacementOrder(x, y, trapType);
            }
        });
    }

    private void EnqueuePlacementOrder(int x, int y, string trapType)
    {
        placementQueue.Enqueue(new TrapPlacement(x, y, trapType));
    }

    private IEnumerator ProcessPlacementQueue()
    {
        while (true)
        {
            if (placementQueue.Count > 0)
            {
                TrapPlacement placement = placementQueue.Dequeue();
                SpawnTrapAtPosition(placement.x, placement.y, placement.trapType);
            }

            yield return null; // Wait for the next frame
        }
    }

    private void SpawnTrapAtPosition(int x, int y, string trapType)
    {

        // Check if the coordinates are valid and the spot is not occupied
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && grid[x, y] == null)
        {

            // Check if the trap type exists in the dictionary
            if (trapPrefabDictionary.TryGetValue(trapType, out GameObject prefabToSpawn))
            {
                if (prefabToSpawn != null)
                {

                    // Instantiate the prefab at the calculated position
                    Vector3 spawnPosition = new Vector3(x, y, 0);
                    GameObject spawnedTrap = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

                    // Mark the grid spot as occupied
                    grid[x, y] = trapType;
                }
                else
                {
                    Debug.LogError($"Prefab for '{trapType}' is null! Check if the prefab is correctly assigned in the Inspector.");
                }
            }
            else
            {
                Debug.LogWarning($"The trap type '{trapType}' does not exist in the dictionary.");
            }
        }
        else
        {
            Debug.LogWarning($"Position ({x}, {y}) is either occupied or out of bounds.");
        }
    }

    void OnApplicationQuit()
    {
        if (clientSocket != null)
        {
            clientSocket.Dispose(); // Properly close the connection
        }
    }

    private class TrapPlacement
    {
        public int x;
        public int y;
        public string trapType;

        public TrapPlacement(int x, int y, string trapType)
        {
            this.x = x;
            this.y = y;
            this.trapType = trapType;
        }
    }
}
