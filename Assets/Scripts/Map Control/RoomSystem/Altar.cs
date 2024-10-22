using System.Collections.Generic;
using UnityEngine;

public class Altar : InteractableObject
{
    private List<string> availableItems = new List<string> { "epee legendaire", "Potion de vie", "Amulette de force" };
    private List<string> playerReceivedItems = new List<string>();

    private bool isPlayerNearby = false;
    public GameObject interactionCanvas;

    void Start()
    {
   
        interactionCanvas.SetActive(false);
    }

    void Update()
    {
     
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact(FindObjectOfType<Player>());  
        }
    }

    public override void Interact(Player player)
    {
        if (availableItems.Count > 0)
        {
       
            string item = availableItems[0];
            player.AddItemToInventory(item);  // Ajouter l'objet à l'inventaire du joueur
            playerReceivedItems.Add(item);    // Ajouter à la liste des objets reçus
            availableItems.RemoveAt(0);       // Retirer l'objet de la liste des objets disponibles
            Debug.Log("L'autel vous a donné : " + item);
        }
        else
        {
            Debug.Log("L'autel n'a plus rien à vous offrir.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionCanvas.SetActive(true); 
            isPlayerNearby = true;
          
        }
    }

    // Détection de la sortie du joueur de la zone de l'autel
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionCanvas.SetActive(false);  
            isPlayerNearby = false;
           
        }
    }

   
    public void ResetItems()
    {
   
        availableItems = new List<string> { "epee legendaire", "Potion de vie", "Amulette de force" };
        playerReceivedItems.Clear();  
        Debug.Log("Autel réinitialisé avec de nouveaux objets.");
    }

    public override void SaveState(GameData gameData)
    {
        gameData.altarAvailableItems = new List<string>(availableItems);  
        gameData.altarPlayerReceivedItems = new List<string>(playerReceivedItems);  
    }

   
    public override void RestoreState(GameData gameData)
    {
        availableItems = new List<string>(gameData.altarAvailableItems); 
        playerReceivedItems = new List<string>(gameData.altarPlayerReceivedItems); 
    }
}
