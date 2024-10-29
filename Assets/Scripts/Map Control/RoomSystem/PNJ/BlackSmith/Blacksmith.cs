using UnityEngine;
using TMPro;

public class Blacksmith : InteractableObject
{
    private string[] dialogues = {
    "Bienvenue, aventurier. Tu n'es pas le premier a passer ici, ni le dernier, je le crains.",
    "Ce corridor... un lieu etrange ou le temps s'entremêle. J'ai vu tant d'ames s'y egare.",
    "Les murs semblent immuables, mais... des echos d'autres vies persistent en eux.",
    "Ah, tu as survecu jusque-la. Ils disent que peu reviennent ici avec autant de determination.",
    "Les aventuriers passent, mais je reste. Eternel temoin d'un cycle sans fin...",
    "Avec chaque cycle, je vois des visages familiers. Vous, les nouveaux, avez le meme regard.",
    "Parfois, il semble que le corridor change, offrant des defis inconnus et perilleux.",
    "Le temps est etrange ici. Le passe, le futur... ils se rejoignent dans une danse infinie.",
    "Si tu cherches un vrai defi, il y a un mannequin. Certains disent qu'il defie les dieux...",
    "Le corridor n'est pas un simple passage. C'est une prison douce, une illusion sans fin.",
    "Plus tu avances, plus les tenebres se resserrent. Ne te perds pas dans leurs promesses.",
    "Il y a des voix dans le vent ici, murmures de ceux qui ont echoue avant toi.",
    "Les epreuves changent, mais les visages restent. Est-ce que tout ceci a un but, crois-tu?",
    "On dit que la fin est la, au bout du corridor. Mais personne ne sait si elle existe vraiment.",
    "Ce n'est pas la premiere fois que je te vois ici... ou quelqu'un comme toi. Le cycle reprend.",
    "Si tu es blesse par les dieux, un totem pourrait apaiser tes maux... si tu le trouves.",
    "Le corridor est capricieux. Certains sortent, d'autres tournent en boucle... pour toujours.",
    "Ton regard me dit que tu cherches des reponses. Mais ici, les reponses sont des pieges.",
    "J'ai vu des aventuriers fuir les epreuves des dieux. Mais toi... tu sembles pret.",
    "Si le cycle continue, peut-etre reviendras-tu. Avec un jour, un espoir de liberte."
    };


    private int dialogueIndex = 0;
    private bool isPlayerNearby = false;
    private bool interactionDisabled = false;

    public GameObject exclamationPoint;
    public GameObject interactionCanvas;
    public GameObject dialogueCanvas;
    public TextMeshProUGUI dialogueText;

    void Start()
    {
        if (GameDataManager.instance != null)
        {
            RestoreState(GameDataManager.instance.gameData);
        }
        exclamationPoint.SetActive(!interactionDisabled);  
        interactionCanvas.SetActive(false);
        dialogueCanvas.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !interactionDisabled)
        {
            Interact(FindObjectOfType<Player>());
        }
    }

    public override void Interact(Player player)
    {
        
        dialogueText.text = dialogues[dialogueIndex];
        dialogueCanvas.SetActive(true);

      
        dialogueIndex++;
        if (dialogueIndex >= dialogues.Length)
        {
            dialogueIndex = dialogues.Length - 1;  
        }
        hasInteracted = true;
        interactionDisabled = true;
        interactionCanvas.SetActive(false);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !interactionDisabled)
        {
            isPlayerNearby = true;
            exclamationPoint.SetActive(false);
            interactionCanvas.SetActive(true);
            Debug.Log("Le joueur est proche du forgeron. Appuyez sur 'E' pour interagir.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionCanvas.SetActive(false);
            exclamationPoint.SetActive(!interactionDisabled);  
            dialogueCanvas.SetActive(false);  
            Debug.Log("Le joueur s'éloigne du forgeron.");
        }
        if (hasInteracted)hasInteracted = !hasInteracted;
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
        interactionDisabled = hasInteracted;
        exclamationPoint.SetActive(!interactionDisabled);  
    }
}
