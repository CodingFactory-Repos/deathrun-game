using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json.Linq;
using CandyCoded.env;

public class TrapManager : MonoBehaviour
{
    public List<GameObject> trapPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> trapPrefabDictionary = new Dictionary<string, GameObject>();
    private int gridSizeX = 9;
    private int gridSizeY = 23;
    private string[,] grid;

    private Queue<TrapPlacement> placementQueue = new Queue<TrapPlacement>();
    private SocketIOUnity clientSocket; // SocketIO client

    private string socketUrl;

    private bool trapsLoaded = false;

    async void Start()
    {
        // Initialize the grid
        grid = new string[gridSizeX, gridSizeY];

        // Map trap names to their prefabs
        trapPrefabDictionary.Add("crossbow_down_prefab", trapPrefabs[0]);
        trapPrefabDictionary.Add("crossbow_up_prefab", trapPrefabs[1]);
        trapPrefabDictionary.Add("crossbow_side_left_prefab", trapPrefabs[2]);
        trapPrefabDictionary.Add("crossbow_side_right_prefab", trapPrefabs[3]);
        trapPrefabDictionary.Add("bear_trap_prefab", trapPrefabs[4]);
        trapPrefabDictionary.Add("spike_prefab", trapPrefabs[5]);

        clientSocket = SocketManager.Instance.ClientSocket;

        StartCoroutine(ProcessPlacementQueue());
    }


    public void Update()
    {
        clientSocket.On("traps:place", response =>
        {
            JArray trapDataArray = JArray.Parse(response.ToString());

            foreach (var trapData in trapDataArray)
            {
                JObject trap = (JObject)trapData["trap"];
                JArray traps = (JArray)trapData["traps"];

                if (traps != null && traps.Count > 0)
                {
                    PlaceMultipleTraps(traps);
                    trapsLoaded = true;
                }
                else if (trap != null)
                {
                    PlaceSingleTrap(trap);
                }
                else
                {
                    // Handle cases where both traps and trap are null or empty
                    if (traps == null || traps.Count == 0)
                    {
                        trapsLoaded = true;
                    }
                    else
                    {
                        Debug.LogWarning("No traps to place.");
                    }
                }
            }

        });

        if (trapsLoaded == false)
        {
            Debug.Log("Trying to reload...");
            clientSocket.Emit("traps:reload");
        }
    }

    private void PlaceMultipleTraps(JArray traps)
    {
        foreach (var trapItem in traps)
        {
            PlaceSingleTrap((JObject)trapItem);
        }
    }

    private void PlaceSingleTrap(JObject trap)
    {
        int x = trap.Value<int>("x");
        int y = trap.Value<int>("y");
        string trapType = trap.Value<string>("trapType");

        EnqueuePlacementOrder(x, y, trapType);
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
                    Vector3 spawnPosition = new Vector3(x + 0.5f, y - 0.25f, 0);
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
