using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissors : MonoBehaviour
{
   
    public enum Choice { Rock, Paper, Scissors, None }

    public Choice playerChoice = Choice.None;

    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;

    public GameObject canvas;

    private Choice savedChoice = Choice.None;

    void Start()
    {
        rockButton.onClick.AddListener(() => OnPlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => OnPlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => OnPlayerChoice(Choice.Scissors));
    }

  
    void OnPlayerChoice(Choice choice)
    {
        playerChoice = choice;
        SaveChoice(); 
        CloseCanvas();
        
    }

    void SaveChoice()
    {
        savedChoice = playerChoice;
        UnityEngine.Debug.Log("Choice saved: " + savedChoice);
    }

    void CloseCanvas(){
        canvas.SetActive(false);
    }

    public Choice GetSavedChoice()
    {
        return savedChoice;
    }
}
