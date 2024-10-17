using System.Diagnostics;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject newRoomPrefab;  // Le prefab de la nouvelle pièce à générer
    public Transform player;          // Le joueur à téléporter
    private GameObject currentRoom;   // La pièce actuelle

    private void Start()
    {
        // Initialise la pièce actuelle, ici on suppose que la scène commence avec une room
        currentRoom = GameObject.FindGameObjectWithTag("Map");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Le joueur est passé la porte, création de la nouvelle room...");

            // 1. Génère la nouvelle room
            GameObject newRoom = Instantiate(newRoomPrefab);

            // 2. Place le joueur dans la nouvelle room
            PlacePlayerInNewRoom(newRoom);

            // 3. Supprime l'ancienne room
            Destroy(currentRoom);

            // 4. Téléporte la nouvelle room au centre du monde
            CenterNewRoom(newRoom);

            // Met à jour la référence à la pièce actuelle
            currentRoom = newRoom;
        }
    }

    // Fonction pour déplacer le joueur dans la nouvelle pièce
    void PlacePlayerInNewRoom(GameObject newRoom)
    {
        // Téléporte le joueur à la position de spawn de la nouvelle room (supposée être un point spécifique dans la room)
        Transform spawnPoint = newRoom.transform.Find("SpawnPoint");
        if (spawnPoint != null)
        {
            player.position = spawnPoint.position;
        }
        else
        {
            UnityEngine.Debug.LogError("SpawnPoint non trouvé dans la nouvelle pièce.");
        }
    }

    // Fonction pour téléporter la nouvelle room au centre du monde
    void CenterNewRoom(GameObject newRoom)
    {
        Vector3 roomOffset = newRoom.transform.position;
        newRoom.transform.position = Vector3.zero;

        // // Ajuster la position du joueur en fonction du décalage
        // player.position -= roomOffset;
    }
}
