using System.Collections;
using UnityEngine;

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
      
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameAfterInitialization());
    }

    private IEnumerator LoadGameAfterInitialization()
    {
        yield return null;

        foreach (InteractableObject interactable in FindObjectsOfType<InteractableObject>())
        {
            interactable.RestoreState(gameData);
        }

        Debug.Log("Game loaded.");
    }
}
