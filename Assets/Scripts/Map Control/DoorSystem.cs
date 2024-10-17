using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject newRoomPrefab;  // Le prefab de la nouvelle pièce à générer
    public Transform player;          // Le joueur à téléporter
    private GameObject currentRoom;   // La pièce actuelle


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject newRoom = Instantiate(newRoomPrefab, new Vector3(4.1f,2.8f,0.0f), Quaternion.identity);
            currentRoom = GameObject.FindGameObjectWithTag("Map");
            DeleteCurrentRoom();
            PlacePlayerInNewRoom(newRoom);
            currentRoom = newRoom;
        }
    }

    void PlacePlayerInNewRoom(GameObject newRoom)
    {

        Transform spawnPoint = newRoom.transform.Find("SpawnPoint");

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
