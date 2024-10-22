using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;
    public GameData gameData = new GameData();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        foreach (InteractableObject interactable in FindObjectsOfType<InteractableObject>())
        {
            interactable.SaveState(gameData);
        }
        //TODO Ecrire ici toute la data en JSON pour retrieve
    }


    public void LoadGame()
    {
        foreach (InteractableObject interactable in FindObjectsOfType<InteractableObject>())
        {
            interactable.RestoreState(gameData);
        }
         //TODO Lire ici toute la data en JSON pour retrieve
    }
}
