using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TrapManager : MonoBehaviour
{

    public List<GameObject> trapPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> trapPrefabDictionary = new Dictionary<string, GameObject>();
    private int gridSizeX = 9;
    private int gridSizeY = 9;
    private string[,] grid;

    void Start()
    {

        grid = new string[gridSizeX, gridSizeY];

       
        trapPrefabDictionary.Add("crossbow_down_prefab", trapPrefabs[0]);
        trapPrefabDictionary.Add("crossbow_up_prefab", trapPrefabs[1]);
        trapPrefabDictionary.Add("crossbow_side_prefab", trapPrefabs[2]);

        // Exemple 
        SpawnTrapAtPosition(3, 5, "crossbow_down_prefab");
    }


    public void SpawnTrapAtPosition(int x, int y, string trapType)
    {

        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY && grid[x, y] == null)
        {
        
            if (trapPrefabDictionary.ContainsKey(trapType))
            {
                GameObject prefabToSpawn = trapPrefabDictionary[trapType];

                Vector3 spawnPosition = new Vector3(x, y, 0); 
                Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

       
                grid[x, y] = trapType;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"Le type de prefab '{trapType}' n'existe pas.");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Position ({x}, {y}) est d�j� occup�e ou hors de la grille.");
        }
    }

    public void ReceivePlacementOrder(int x, int y, string trapType)
    {
        // M�thode pour recevoir des commandes WebSocket et placer les pi�ges
        SpawnTrapAtPosition(x, y, trapType);
    }
}
