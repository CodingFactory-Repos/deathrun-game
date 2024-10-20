using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public string selectedScene;

   public void QuitGame(){

    Application.Quit();
   }

   public void StartRoom(){

      SceneManager.LoadScene(selectedScene);

   }
}
