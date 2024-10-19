using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject newRoomPrefab;  // Le prefab de la nouvelle pièce à générer
    private Transform player;          // Le joueur à téléporter
    private GameObject currentRoom;   // La pièce actuelle

     private void Start()
    {
        // Trouver le joueur au début, en supposant qu'il a le tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            UnityEngine.Debug.LogError("Le joueur avec le tag 'Player' n'a pas été trouvé !");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            currentRoom = GameObject.FindGameObjectWithTag("Map");
            DeleteCurrentRoom();
            Vector3 newRoomPosition = new Vector3(4.1f, 2.8f, 0.0f);
            GameObject newRoom = Instantiate(newRoomPrefab, newRoomPosition, Quaternion.identity);
            PlacePlayerInNewRoom(newRoom);
            currentRoom = newRoom;
        }
    }

    void PlacePlayerInNewRoom(GameObject newRoom)
    {

        Transform spawnPoint = newRoom.transform.Find("SpawnPoint");
        bool test =  newRoom.transform.Find("SpawnPoint");
        Vector3 spawnPosition = spawnPoint.position;
        player.position = spawnPosition;
        //GameObject spawnPointObject = GameObject.FindWithTag("Respawn");
        //UnityEngine.Debug.Log(spawnPointObject.transform.position);
        //Vector3 spawnPosition = spawnPointObject.transform.position;
        //UnityEngine.Debug.Log(spawnPosition);
        //player.position = spawnPosition;

    }

    void DeleteCurrentRoom()
    {
        Destroy(currentRoom);
    }

}
