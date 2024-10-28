using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class RockPaperScissors : MonoBehaviour
{
    public enum Choice { Rock, Paper, Scissors, None }

    public Choice playerChoice = Choice.None;

    public Button rockButton;
    public Button paperButton;
    public Button scissorsButton;

    private SocketIOUnity clientSocket;
    public GameObject canvas;

    private string TexteGodResponse;

    public TextMeshProUGUI resultsText;

    private Choice savedChoice = Choice.None;

    void Start()
    {
        clientSocket = SocketManager.Instance.ClientSocket;

        rockButton.onClick.AddListener(() => OnPlayerChoice(Choice.Rock));
        paperButton.onClick.AddListener(() => OnPlayerChoice(Choice.Paper));
        scissorsButton.onClick.AddListener(() => OnPlayerChoice(Choice.Scissors));

        // Assurer que le texte est initialement vide
        resultsText.text = "";
    }

    void Update()
    {
        clientSocket.On("rps:results", response =>
        {
            Debug.Log("Results received!");

            JArray jsonResponse = JArray.Parse(response.ToString());

            foreach (var result in jsonResponse)
            {
                Debug.Log("Result: " + result);
                Debug.Log("Result type: " + result.GetType());
                Debug.Log("Result type: " + result["result"]);

                TexteGodResponse = result["result"].ToString();
            }

            resultsText.text = TexteGodResponse;
            StartCoroutine(HideTextAfterDelay(5)); // Cache le texte après 5 secondes
        });

        clientSocket.On("rps:lose", response =>
        {
            Debug.Log("You lost!");
        });
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
        var data = new Dictionary<string, string>
        {
            { "move", FormatChoice(playerChoice) }
        };

        await clientSocket.EmitAsync("rps:select", data);
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


    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultsText.text = ""; 
    }
}
