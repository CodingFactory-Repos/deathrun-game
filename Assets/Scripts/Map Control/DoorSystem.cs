using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform teleportDestination; 
    public GameObject coridorPrefab;     

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CreateCoridor();
            TeleportPlayer(other.gameObject);
          
        }
    }

    // Fonction pour téléporter le joueur à l'emplacement de la grille Coridor
    void TeleportPlayer(GameObject player)
    {
        player.transform.position = teleportDestination.position;

    }
    // Fonction pour créer la grille Coridor
    void CreateCoridor()
    {
        if (coridorPrefab != null)
        {
            Instantiate(coridorPrefab, teleportDestination.position, Quaternion.identity);
        }
    }
}
