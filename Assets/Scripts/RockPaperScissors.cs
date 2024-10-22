using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;

public class RockPaperScissors : MonoBehaviour
{
    public enum Choice { Rock, Paper, Scissors, None }

    public Choice playerChoice = Choice.None;

    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;

    private SocketIOUnity clientSocket;
    public GameObject canvas;

    private Choice savedChoice = Choice.None;

    void Start()
    {
        clientSocket = SocketManager.Instance.ClientSocket;

        rockButton.onClick.AddListener(() => OnPlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => OnPlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => OnPlayerChoice(Choice.Scissors));
    }

    void OnPlayerChoice(Choice choice)
    {
        playerChoice = choice;
        SaveChoice();
        SendChoiceToBackend();
        CloseCanvas();
    }

    void SaveChoice()
    {
        savedChoice = playerChoice;
    }

    async void SendChoiceToBackend()
    {
        await clientSocket.EmitAsync("game:rockpaperscissors", FormatChoice(GetSavedChoice()));
    }

    string FormatChoice(Choice choice)
    {
        switch (choice)
        {
            case Choice.Rock:
                return "rock";
            case Choice.Paper:
                return "paper";
            case Choice.Scissors:
                return "scissors";
            default:
                return "none";
        }
    }

    void CloseCanvas()
    {
        canvas.SetActive(false);
    }

    public Choice GetSavedChoice()
    {
        return savedChoice;
    }
}
