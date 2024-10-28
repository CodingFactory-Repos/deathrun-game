using System.Collections.Generic;
using UnityEngine;

public class Altar : InteractableObject
{
    private List<string> availableItems = new List<string> { "epee legendaire", "Potion de vie", "Amulette de force" };
    private List<string> playerReceivedItems = new List<string>();

    private bool isPlayerNearby = false;
    public GameObject interactionCanvas;

    private PlayerHealth healthManager;

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
        healthManager = FindObjectOfType<PlayerHealth>();
        healthManager.Heal(1);
        Destroy(transform.parent.gameObject);
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
