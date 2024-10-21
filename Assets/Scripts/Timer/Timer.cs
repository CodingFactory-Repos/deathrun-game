using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;

    float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = (int)elapsedTime / 60;
        int seconds = (int)elapsedTime % 60;
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
