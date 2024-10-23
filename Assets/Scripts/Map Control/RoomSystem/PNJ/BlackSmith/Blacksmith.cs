using System.Diagnostics;
using UnityEngine;

public class Blacksmith : InteractableObject
{
    private string[] dialogues = { "Bienvenue, aventurier.", "Tu es revenu me voir !", "Je n'ai rien de plus a te dire." };
    private int dialogueIndex = 0;

    private bool isPlayerNearby = false;
    public GameObject exclamationPoint;
    public GameObject interactionCanvas;


    void Start(){
        exclamationPoint.SetActive(true);
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
        if (!hasInteracted)
        {
          
            UnityEngine.Debug.Log(dialogues[dialogueIndex]);
            dialogueIndex++;
            if (dialogueIndex >= dialogues.Length) dialogueIndex = dialogues.Length - 1;  
            hasInteracted = true;
        }
        else
        {
            UnityEngine.Debug.Log(dialogues[dialogueIndex]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            exclamationPoint.SetActive(false);
            interactionCanvas.SetActive(true);
            UnityEngine.Debug.Log("Le joueur est proche du forgeron. Appuyez sur 'E' pour interagir.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionCanvas.SetActive(false);
            exclamationPoint.SetActive(true);
            UnityEngine.Debug.Log("Le joueur s'éloigne du forgeron.");
        }
    }
    

    public override void SaveState(GameData gameData)
    {
        gameData.blacksmithInteracted = hasInteracted;
        gameData.blacksmithDialogueIndex = dialogueIndex;
    }

    public override void RestoreState(GameData gameData)
    {
        hasInteracted = gameData.blacksmithInteracted;
        dialogueIndex = gameData.blacksmithDialogueIndex;
    }
}
