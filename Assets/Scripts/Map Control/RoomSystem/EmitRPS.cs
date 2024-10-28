using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class EmitRPS : InteractableObject
{
    public GameObject interactionCanvas;
    public GameObject RPSCanvas;
    private bool isPlayerNearby = false;    
    private SocketIOUnity clientSocket;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = GameObject.Find("RockPaperCissorGame");
        if (parent != null)
        {
            // Recherche l'enfant RockPaperCissor à partir du parent
            RPSCanvas = parent.transform.Find("RockPaperCissor")?.gameObject;
        }
        clientSocket = SocketManager.Instance.ClientSocket;
        interactionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact(FindObjectOfType<Player>());  
        }
    }

      public override void Interact(Player player)
    {
        RPSCanvas.SetActive(true);
        clientSocket.Emit("rps:start");
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionCanvas.SetActive(true); 
            isPlayerNearby = true;
        }
    }

     private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactionCanvas.SetActive(false);  
            isPlayerNearby = false;
           
        }
    }

    
    public override void SaveState(GameData gameData)
    {
       
    }

   
    public override void RestoreState(GameData gameData)
    {
       
    }
    
}
