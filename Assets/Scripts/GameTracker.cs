using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTracker : MonoBehaviour
{
    // Variables pour timer et affichage
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI stageText;

    // Temps écoulé
    private float elapsedTime = 0f;

    // Compteur de stages (salles traversées)
    private int stageCounter = 0;

    // Prefabs des événements (forgeron et autel)
    public GameObject blacksmithPrefab;
    public GameObject altarPrefab;
    public GameObject RPSPrefab;

    public GameObject waitingRoomPrefab;

    // Points de spawn (ils seront trouvés dynamiquement)
    private Transform blacksmithSpawnPoint;
    private Transform altarSpawnPoint;

    private Transform RPSSpawnPoint;

    // Références vers les instances actuelles du forgeron et de l'autel
    private GameObject currentBlacksmith;
    private GameObject currentAltar;
    private GameObject currentRPS;

    private GameObject waitingRoom;

    void Start()
    {
        stageText.text = "Stage: " + stageCounter;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


    public void NextStage()
    {
        if (stageCounter == 0)
        {
            instantiateWaitingRoom();
        }
        stageCounter++;
        stageText.text = "Stage: " + stageCounter;
    }

    private void instantiateWaitingRoom()
    {
        waitingRoom = GameObject.Find("WaitingRoom");
        if (waitingRoom == null && waitingRoomPrefab != null)
        {
            Instantiate(waitingRoomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }


    public void SpawnCorridorEvents()
    {
        altarSpawnPoint = GameObject.Find("AltarSpawnPoint")?.transform;
        blacksmithSpawnPoint = GameObject.Find("BlacksmithSpawnPoint")?.transform;
        RPSSpawnPoint =  GameObject.Find("RPSSpawnPoint")?.transform;
        

        if (blacksmithSpawnPoint == null || altarSpawnPoint == null || RPSSpawnPoint == null)
        {
            return;
        }


        // Apparition du forgeron au stage 5
        if (stageCounter >= 2 && blacksmithPrefab != null)
        {
            currentBlacksmith = Instantiate(blacksmithPrefab, blacksmithSpawnPoint.position, Quaternion.identity);
            
        
        }

        if (stageCounter >= 8 && RPSPrefab != null)
        {
            currentRPS = Instantiate(RPSPrefab, RPSSpawnPoint.position, Quaternion.identity);
            
        }

        // Apparition de l'autel au stage 10, et réinitialisation de ses objets
        if (stageCounter >= 15 && altarPrefab != null)
        {
            currentAltar = Instantiate(altarPrefab, altarSpawnPoint.position, Quaternion.identity);
            Altar altarScript = currentAltar.GetComponent<Altar>();
            if (altarScript != null)
            {
                altarScript.ResetItems();
            }
        }
    }

    public void ClearCorridorEvents()
    {
        if (currentBlacksmith != null)
        {
            Destroy(currentBlacksmith);
            currentBlacksmith = null;
        }

        if (currentAltar != null)
        {
            Destroy(currentAltar);
            currentAltar = null;
        }

          if (currentRPS != null)
        {
            Destroy(currentRPS);
            currentRPS = null;
        }
    }
}
